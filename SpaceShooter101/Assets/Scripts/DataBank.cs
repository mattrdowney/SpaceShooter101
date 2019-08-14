using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace praveen.One
{
    public class DataBank : MonoBehaviour
    {
        static Dictionary<int, int> m_GunPowerDict = new Dictionary<int, int>();
        static Dictionary<int, MissileMagazine> m_MissileMagazine = new Dictionary<int, MissileMagazine>();
        static Dictionary<int, Shield> m_ShieldDict = new Dictionary<int, Shield>();

        // Start is called before the first frame update
        void Start()
        {
            m_GunPowerDict.Add(1, 0);
            m_GunPowerDict.Add(2, 10);
            m_GunPowerDict.Add(3, 15);
            m_GunPowerDict.Add(4, 27);
            m_GunPowerDict.Add(5, 50);

            // Level                 cost capacity
            m_MissileMagazine.Add(1, new MissileMagazine(0, 1));
            m_MissileMagazine.Add(2, new MissileMagazine(20, 3));
            m_MissileMagazine.Add(3, new MissileMagazine(25, 7));
            m_MissileMagazine.Add(4, new MissileMagazine(40, 10));
            m_MissileMagazine.Add(5, new MissileMagazine(60, 20));

            // Level       cost  seconds
            m_ShieldDict.Add(1, new Shield(0, 3));
            m_ShieldDict.Add(2, new Shield(20, 7));
            m_ShieldDict.Add(3, new Shield(25, 12));
            m_ShieldDict.Add(4, new Shield(40, 10));
            m_ShieldDict.Add(5, new Shield(60, 20));

        }

        /// <summary>
        /// Returns gun power data
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, int> GetGunPowerData()
        {
            return m_GunPowerDict;
        }


        /// <summary>
        /// Returns the missile magazine data
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, MissileMagazine> GetMissileMagazineData()
        {
            return m_MissileMagazine;
        }

        /// <summary>
        /// Get the shield data dictionary
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, Shield> GetShieldData()
        {
            return m_ShieldDict;
        }

        /// <summary>
        /// Returns player damage by level
        /// </summary>
        /// <param name="lvl"></param>
        /// <returns></returns>
        public static int GetGunPowerDamageByLevel(int lvl)
        {
            return lvl * 2;
        }

    }
}


