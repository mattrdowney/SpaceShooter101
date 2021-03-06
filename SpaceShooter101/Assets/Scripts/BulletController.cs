﻿using praveen.One.util;
using UnityEngine;

namespace praveen.One
{
    public enum BulletOwner
    {
        player,
        enemy,
        none
    }

    public class BulletController : MonoBehaviour
    {

        #region singleton stuff
        private static BulletController m_Instance;

        public static BulletController Instance
        {
            get { return m_Instance; }
        }
        #endregion


        #region StaticFields
        static ObjectPool m_LaserBullets;
        static ObjectPool m_Missiles;
        #endregion

        #region SerializedFields
        [SerializeField] GameObject m_Bullets;
        [SerializeField] GameObject m_Missile;
        [SerializeField] Transform  m_BulletHolder;
        [SerializeField] Transform  m_MissileHolder;
        [SerializeField] Sprite[]   m_BulletSkin;
        #endregion


        void Awake()
        {
            m_Instance = this;
            m_LaserBullets  = new ObjectPool(m_Bullets, m_BulletHolder, 100, false, null, null);
            m_Missiles      = new ObjectPool(m_Missile, m_MissileHolder, 10, false, null, null);
        }

        /// <summary>
        /// Get a bullet from object pool
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public GameObject GetBullet(BulletOwner type)
        {
            GameObject bullet = m_LaserBullets.Spawn();
            bullet.transform.rotation = Quaternion.identity;
            bullet.transform.localPosition = Vector3.zero;
            bullet.GetComponent<SpriteRenderer>().sprite = m_BulletSkin[(int)type];
            bullet.SetActive(false);
            return bullet;
        }

        /// <summary>
        /// Store back in object pool
        /// </summary>
        /// <param name="bullet"></param>
        public static void RecycleBullet(GameObject bullet)
        {
            m_LaserBullets.Recycle(bullet);
        }

        /// <summary>
        /// Get missile bullet from object pool
        /// </summary>
        /// <returns></returns>
        public GameObject GetMissile()
        {
            if (GameManager.Instance.OnUseOneMissile())
            {
                GameObject missile = m_Missiles.Spawn();
                missile.transform.rotation = Quaternion.identity;
                missile.transform.localPosition = Vector3.zero;
                missile.SetActive(false);
                return missile;
            }
            return null;
            
        }


        /// <summary>
        /// Put the used missile back to object pool
        /// </summary>
        /// <param name="missile"></param>
        public static void RecycleMissile(GameObject missile)
        {
            m_Missiles.Recycle(missile);
        }
    }
}



