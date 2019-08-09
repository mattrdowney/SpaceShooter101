using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace praveen.One
{
    public class SpaceShip : MonoBehaviour
    {
        // Bullets
        // Reload Time
        public int HitPoints;
        public int GunPower;
        public int BulletCount;

        public virtual void Init(int hitpoints, Sprite skin)
        {
            HitPoints = hitpoints;
            GetComponent<SpriteRenderer>().sprite = skin;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public virtual void Shoot()
        {

        }


    }
}


