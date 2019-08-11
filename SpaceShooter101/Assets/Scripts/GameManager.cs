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
        int m_PlayerHp;
        int m_Level;
        int m_Coins;
        float m_ShieldTime;
        float m_ShieldActTime;
        int m_Score;
        int m_EnemiesKilled;
        #endregion

        private void Awake()
        {
            if (m_Instance != null && m_Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                m_Instance = this;
            }
            DontDestroyOnLoad(this.gameObject);
            m_PlayerHp = 3;
        }


        public void AddScore(int score)
        {
            m_Score += score;
            HudController.Instance.SetScore(m_Score);
        }

        public void UpdateEnemiesKilled()
        {
            m_EnemiesKilled += 1;
            HudController.Instance.EnemiesKilled(m_EnemiesKilled);
        }

        public void OnPlayerHit()
        {
            m_PlayerHp -= 1;
            HudController.Instance.SetPlayerHelth(m_PlayerHp);
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

        public float GetShieldTime()
        {
            return m_ShieldTime;
        }

        public float GetShieldActTime()
        {
            return m_ShieldActTime;
        }

        public void AddCoin()
        {
            m_Coins++;
            HudController.Instance.SetCoins(m_Coins);
        }

        public int GetHighScore()
        {
            return 1988;
        }
    }
}


