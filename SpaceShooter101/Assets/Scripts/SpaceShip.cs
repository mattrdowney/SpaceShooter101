using praveen.One.util;
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
            Shoot();
        }

        public virtual void MissileDamage()
        {

        }

        public virtual void Shoot()
        {

        }
    }
}


