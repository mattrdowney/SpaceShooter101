using UnityEngine;


namespace praveen.One
{

    public class Bullet : MonoBehaviour
    {
        public int Speed;
        public int Damage;
        public BulletOwner Owner = BulletOwner.none;

        public virtual void Update()
        {
            transform.Translate(Vector3.up * Time.deltaTime * Speed);

            Recycle();
        }

        /// <summary>
        /// Set data before use (Init)
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="damage"></param>
        /// <param name="owner"></param>
        public virtual void Program(int speed, int damage, BulletOwner owner)
        {
            Speed = speed;
            Damage = damage;
            Owner = owner;
        }


        /// <summary>
        /// Recycle bullet and put in to the object pool
        /// </summary>
        private void Recycle()
        {
            if (Owner == BulletOwner.enemy)
            {
                if (this.transform.position.y < GameManager.Instance.GetLowerScreenY() - 4f)
                {
                    BulletController.RecycleBullet(this.gameObject);
                }
            }
            else
            {
                if (this.transform.position.y > GameManager.Instance.GetUpperScreenY() + 4f)
                {
                    BulletController.RecycleBullet(this.gameObject);
                }
            }

        }



        public virtual void OnCollisionEnter2D(Collision2D collision)
        {
            switch (Owner)
            {
                case BulletOwner.player:
                    if (collision.gameObject.tag == "Enemy")
                    {
                        BulletController.RecycleBullet(gameObject);
                        collision.gameObject.SendMessage("Damage", Damage);
                    }
                    break;

                case BulletOwner.enemy:

                    if (collision.gameObject.tag == "Player")
                    {
                        BulletController.RecycleBullet(gameObject);
                        collision.gameObject.SendMessage("Damage", Damage);
                    }
                    break;
            }

        }

    }
}


