using UnityEngine;
using UnityEngine.UI;

namespace praveen.One
{
    public class HudController : MonoBehaviour
    {
        #region singleton stuff
        private static HudController m_Instance;

        public static HudController Instance
        {
            get { return m_Instance; }
        }
        #endregion




        #region StaticFields

        #endregion

        #region SerializedFields
        [SerializeField] Slider m_ShieldSlider;
        [SerializeField] Text m_Score;
        [SerializeField] Text m_Coins;
        [SerializeField] Text m_RocketsLeft;
        [SerializeField] Image[] m_HelthIcons;
        [SerializeField] Text m_EnemiesKilled;
        #endregion


        void Awake()
        {
            m_Instance = this;

        }

        public void SetPlayerHelth(int helth)
        {
            switch (helth)
            {
                case 0:
                    m_HelthIcons[0].gameObject.SetActive(false);
                    break;
                case 1:
                    m_HelthIcons[1].gameObject.SetActive(false);
                    break;
                case 2:
                    m_HelthIcons[2].gameObject.SetActive(false);
                    break;
            }
        }

        public void SetShieldActiveProgress(float value)
        {
            m_ShieldSlider.value = (value / 10);
        }

        /// <summary>
        /// Sets the score
        /// </summary>
        /// <param name="score"></param>
        public void SetScore(int score)
        {
            m_Score.text = score.ToString();
        }

        /// <summary>
        /// Sets the enemy killed count
        /// </summary>
        /// <param name="count"></param>
        public void EnemiesKilled(int count)
        {
            m_EnemiesKilled.text = count.ToString();
        }

        /// <summary>
        /// Sets the coins count (Current Session)
        /// </summary>
        /// <param name="count"></param>
        public void SetCoins(int count)
        {
            m_Coins.text = count.ToString();
        }

        /// <summary>
        /// Set Missile Data
        /// </summary>
        /// <param name="count"></param>
        /// <param name="capacity"></param>
        public void SetMissileData(int count, int capacity)
        {
            m_RocketsLeft.text = count.ToString() +"/"+ capacity.ToString();
        }


    }
}


