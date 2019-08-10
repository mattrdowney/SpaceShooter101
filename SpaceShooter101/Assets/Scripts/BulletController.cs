using praveen.One.util;
using UnityEngine;

namespace praveen.One
{
    public enum BulletTypes
    {
        player,
        enemy,
        coin
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
        #endregion

        #region SerializedFields
        [SerializeField] GameObject m_Bullets;
        [SerializeField] Transform  m_BulletHolder;
        [SerializeField] Sprite[]   m_BulletSkin;
        #endregion


        void Awake()
        {
            m_Instance = this;
            m_LaserBullets = new ObjectPool(m_Bullets, m_BulletHolder, 100, false, null, null);
        }

        public GameObject GetBullet(BulletTypes type)
        {
            GameObject bullet = m_LaserBullets.Spawn();
            bullet.transform.rotation = Quaternion.identity;
            bullet.transform.localPosition = Vector3.zero;
            bullet.GetComponent<SpriteRenderer>().sprite = m_BulletSkin[(int)type];
            bullet.SetActive(false);
            return bullet;
        }

        public static void RecycleBullet(GameObject bullet)
        {
            m_LaserBullets.Recycle(bullet);
        }
    }
}



