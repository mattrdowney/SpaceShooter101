using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace praveen.One
{
    public class GameOver : MonoBehaviour
    {
        [SerializeField] Text m_HighScoreText;
        [SerializeField] Text m_Score;
        [SerializeField] Text m_Coins;
        [SerializeField] HighScoreWindow m_HighScoreWindow;

        ShooterSession m_LastSession;

        void Start()
        {
            m_LastSession = GameManager.Instance.GetCurrentSession();
            SetUI();
        }

        /// <summary>
        /// Set Game over GUI
        /// </summary>
        void SetUI()
        {

            m_Score.text = m_LastSession.Score.ToString();
            m_Coins.text = GameManager.Instance.GetCoinsInHand().ToString();

            if (GameManager.Instance.IsNewRecord(m_LastSession.Score))
            {
                m_HighScoreWindow.PopUp();
            }
        }

        /// <summary>
        /// Press start button
        /// </summary>
        public void OnPressedStartBtn()
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }

        /// <summary>
        /// Press Home button
        /// </summary>
        public void OnPressHomeBtn()
        {
            SceneManager.LoadScene("Start", LoadSceneMode.Single);
        }
    }
}

