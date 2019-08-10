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
        [SerializeField] Image[] m_HelthIcons;
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

        public void SetScore(int score)
        {
            m_Score.text = score.ToString();
        }

    }
}


