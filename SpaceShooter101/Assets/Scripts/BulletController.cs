using praveen.One.util;
using UnityEngine;

namespace praveen.One
{
    public class BulletController : MonoBehaviour
    {
        #region StaticFields
        static ObjectPool m_LaserBullets;
        #endregion

        #region SerializedFields
        [SerializeField] GameObject m_Bullets;
        [SerializeField] Transform m_BulletHolder;
        #endregion


        void Awake()
        {
            m_LaserBullets = new ObjectPool(m_Bullets, m_BulletHolder, 100, false, null, null);
        }

        public static GameObject GetBullet()
        {
            GameObject bullet = m_LaserBullets.Spawn();
            bullet.transform.rotation = Quaternion.identity;
            bullet.transform.localPosition = Vector3.zero;
            bullet.SetActive(false);
            return bullet;
        }

        public static void RecycleBullet(GameObject bullet)
        {
            m_LaserBullets.Recycle(bullet);
        }
    }
}



