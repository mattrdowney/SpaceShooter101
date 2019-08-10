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


        // Start is called before the first frame update
        void Start()
        {
            
        }

        void FixedUpdate()
        {
            transform.Translate(Vector3.down * Time.deltaTime * m_Speed);
        }

        void Update()
        {
            Recycle();
            
        }

        public override void Shoot()
        {
            StartCoroutine(EnemyShoot());
        }


        IEnumerator EnemyShoot()
        {
            for (int i = 0; i < 30; i++)
            {
                GameObject bullet = BulletController.Instance.GetBullet(BulletTypes.enemy);
                bullet.transform.SetParent(m_Gun);
                bullet.transform.localPosition = Vector3.zero;
                bullet.transform.localRotation = Quaternion.identity;
                bullet.layer = 11;
                bullet.tag = "EnemyBullet";
                bullet.SetActive(true);
                yield return new WaitForSeconds(1);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "PlayerBullet")
            {
                GameObject coin = CoinController.GetCoin();
                coin.SetActive(true);
                coin.transform.parent = null;
                coin.transform.position = this.transform.position;
                coin.GetComponent<Coin>().Init();
                EnemyController.RecycleEnemy(gameObject);
                BulletController.RecycleBullet(collision.gameObject);
                GameManager.Instance.AddScore(10);
            }

        }


        private void Recycle()
        {
            if (this.transform.position.y < GameManager.Instance.GetLowerScreenY() - 4f)
            {
                EnemyController.RecycleEnemy(this.gameObject);
            }
        }
    }
}


