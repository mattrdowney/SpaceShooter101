﻿using System.Collections;
using UnityEngine;

namespace praveen.One
{
    public class Enemy : SpaceShip
    {
        #region SerializedFields
        [SerializeField] float m_Speed;
        [SerializeField] Transform m_Gun;
        #endregion

        int m_Damage = 100;

        void FixedUpdate()
        {
            transform.Translate(Vector3.down * Time.deltaTime * m_Speed);
        }

        void Update()
        {
            Recycle();
        }

        /// <summary>
        /// Enemy shoot
        /// </summary>
        public override void Shoot()
        {
            // REVIEW (compliment, architecture): using coroutines is a good architecture decision in my opinion (since you don't have to worry about e.g. tracking time manually).
            StartCoroutine(EnemyShoot());
        }

        /// <summary>
        /// Enemy shoot
        /// </summary>
        /// <returns></returns>
        IEnumerator EnemyShoot()
        {
            for (int i = 0; i < 30; i++)
            {
                GameObject bullet = BulletController.Instance.GetBullet(BulletOwner.enemy);
                bullet.transform.SetParent(m_Gun);
                bullet.transform.localPosition = Vector3.zero;
                bullet.transform.localRotation = Quaternion.identity;
                bullet.layer = 11;
                bullet.tag = "EnemyBullet";
                int bulletDamage = EnemyController.GetEnemyGunPowerByLvl(GameManager.Instance.GetLevel());
                // REVIEW (compliment, architecture): this is the good way to do object-oriented programming in Unity (as opposed to SendMessage which is better to avoid)
                bullet.GetComponent<Bullet>().Program(10, bulletDamage, BulletOwner.enemy);
                bullet.SetActive(true);
                yield return new WaitForSeconds(1);
            }
        }

        /// <summary>
        /// Get the damage from missile
        /// </summary>
        public override void MissileDamage()
        {
            Destroy();
        }

        /// <summary>
        /// Get Damage from bullet
        /// </summary>
        /// <param name="damage"></param>
        public override void Damage(int damage)
        {
            base.Damage(damage);
        }

        /// <summary>
        /// Destroies the ship
        /// </summary>
        public override void Destroy()
        {
            CoinController.SpawnCoin(this.transform.position);
            EnemyController.RecycleEnemy(gameObject);
            GameManager.Instance.AddScore(10);
            GameManager.Instance.UpdateEnemiesKilled();
        }

        /// <summary>
        /// Put back to the object pool
        /// </summary>
        private void Recycle()
        {
            if (this.transform.position.y < GameManager.Instance.GetLowerScreenY() - 4f)
            {
                EnemyController.RecycleEnemy(this.gameObject);
            }
        }

        // REVIEW (architecture): I consider tags versus layers to be a software architecture decision. Generally the less tags you use the simpler the game logic becomes.
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.tag == "Player")
            { 
                // REVIEW (compliment, architecture): I guess you only have one instance of .SendMessage() in your entire code, which is good, but you can always add more simple components that communicate through .GetComponent() if you find chances to do it.
                collision.gameObject.SendMessage("Damage", m_Damage);
            }
        }
    }
}


