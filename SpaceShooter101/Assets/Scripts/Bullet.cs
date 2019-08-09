using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace praveen.One
{
    public class Bullet : MonoBehaviour
    {
        int m_Speed = 10;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
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
    }
}


