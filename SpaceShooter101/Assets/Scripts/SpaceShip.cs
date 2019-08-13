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

        /// <summary>
        /// Initialize the sapace ship
        /// </summary>
        /// <param name="hitpoints"></param>
        /// <param name="skin"></param>
        public virtual void Init(int hitpoints, Sprite skin)
        {
            HitPoints = hitpoints;
            GetComponent<SpriteRenderer>().sprite = skin;
            Shoot();
        }

        public virtual void MissileDamage() { }

        public virtual void Shoot() { }
    }
}


