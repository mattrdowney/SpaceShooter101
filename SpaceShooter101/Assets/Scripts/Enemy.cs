using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace praveen.One
{
    public class Enemy : SpaceShip
    {

        [SerializeField] float m_Speed;
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
            base.Shoot();
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


