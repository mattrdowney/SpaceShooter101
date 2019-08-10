using UnityEngine;

namespace praveen.One
{
    public class Coin : MonoBehaviour
    {
        private float m_LifeTime = 3f;

        public void Init()
        {
            Invoke("Recycle", m_LifeTime);
        }

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


