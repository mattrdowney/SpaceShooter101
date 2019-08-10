using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace praveen.One
{
    public class Bullet : MonoBehaviour
    {
        int m_Speed = 10;

        void Update()
        {
            transform.Translate(Vector3.up * Time.deltaTime * m_Speed);

            Recycle();
        }


        private void Recycle()
        {
            if (this.transform.position.y < GameManager.Instance.GetLowerScreenY() - 4f)
            {
                BulletController.RecycleBullet(this.gameObject);
            }
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            
        }
    }
}


