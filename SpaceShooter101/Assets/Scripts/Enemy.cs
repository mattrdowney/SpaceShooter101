using System.Collections;
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

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.tag == "Player")
            {
                collision.gameObject.SendMessage("Damage", m_Damage);
            }
        }
    }
}


