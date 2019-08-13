using UnityEngine;

namespace praveen.One
{
    public class Coin : MonoBehaviour
    {
        private float m_LifeTime = 3f;

        /// <summary>
        /// Initialize the coin
        /// </summary>
        public void Init()
        {
            GameManager.Instance.AddCoin();
            Invoke("Recycle", m_LifeTime);
        }

        /// <summary>
        /// Put back to the object pool
        /// </summary>
        void Recycle()
        {
            CoinController.Recycle(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log(collision.gameObject.tag);
            if(collision.gameObject.tag == "CoinBullet")
            {
                Recycle();
            }
        }

    }
}


