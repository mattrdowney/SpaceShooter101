using UnityEngine;
using UnityEngine.UI;

namespace praveen.One
{
    // REVIEW (architecture): It would be nice if there were a way to combine the variable shared between this and GameManager into a common data class. My first thought was to look for a common interface for each object e.g. SetText() and SetValue() which would look into a dictionary. No good recommendations but it's something to think about.
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
        [SerializeField] Slider m_HPSlider;
        [SerializeField] Text m_Score;
        [SerializeField] Text m_Coins;
        [SerializeField] Text m_RocketsLeft;
        [SerializeField] Image[] m_HelthIcons;
        [SerializeField] Text m_EnemiesKilled;
        [SerializeField] Text m_CountdownDisplay;
        #endregion


        void Awake()
        {
            m_Instance = this;

        }

        /// <summary>
        /// Set player lifes in HUD
        /// </summary>
        /// <param name="lifes"></param>
        public void SetPlayerLifes(int lifes)
        {
            switch (lifes)
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

        /// <summary>
        /// Set player HP
        /// </summary>
        /// <param name="value"></param>
        public void SetPlayerHP(float value)
        {
            m_HPSlider.value = value;
        }

        /// <summary>
        /// Update Shield Progress bar
        /// </summary>
        /// <param name="value"></param>
        public void SetShieldActiveProgress(float value)
        {
            m_ShieldSlider.value = (value);
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

        /// <summary>
        /// Display countdown timer in game
        /// </summary>
        /// <param name="timeLeft"></param>
        public void SetCountdownDisplay(int timeLeft)
        {
            m_CountdownDisplay.text = timeLeft.ToString();
        }


    }
}


