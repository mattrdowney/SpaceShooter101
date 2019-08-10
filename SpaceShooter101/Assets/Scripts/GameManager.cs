using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace praveen.One
{
    public class GameManager : MonoBehaviour
    {
        #region singleton stuff
        private static GameManager m_Instance;

        public static GameManager Instance
        {
            get { return m_Instance; }
        }
        #endregion


        #region SerializedFields

        #endregion

        #region PrivateFields
        int m_Level;
        int m_Coins;
        #endregion

        private void Awake()
        {
            m_Instance = this;
        }


        /// <summary>
        /// Returns Level
        /// </summary>
        /// <returns></returns>
        public int GetLevel()
        {
            return m_Level;
        }

        /// <summary>
        /// Returns world position of Upper Left Screen boundry
        /// </summary>
        /// <returns></returns>
        public Vector3 GetUpperLeftScreenBoundry()
        {
            return Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
        }

        /// <summary>
        /// Returns world position of Upper Right Screen boundry
        /// </summary>
        /// <returns></returns>
        public Vector3 GetUpperRightScreenBoundry()
        {
            return Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        }

        /// <summary>
        /// Returns Lower Screen Y
        /// </summary>
        /// <returns></returns>
        public float GetLowerScreenY()
        {
            return Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        }

        /// <summary>
        /// Returns Upper Screen Y
        /// </summary>
        /// <returns></returns>
        public float GetUpperScreenY()
        {
            return Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0)).y;
        }

        public void AddCoin()
        {
            m_Coins++;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}


