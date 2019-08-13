using UnityEngine;


namespace praveen.One
{
    public class Bullet : MonoBehaviour
    {
        public int m_Speed = 10;

        public virtual void Update()
        {
            transform.Translate(Vector3.up * Time.deltaTime * m_Speed);

            Recycle();
        }


        /// <summary>
        /// Recycle bullet and put in to the object pool
        /// </summary>
        private void Recycle()
        {
            if (this.transform.position.y < GameManager.Instance.GetLowerScreenY() - 4f)
            {
                BulletController.RecycleBullet(this.gameObject);
            }
        }

    }
}


