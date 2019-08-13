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
        [SerializeField] GameObject m_NewHighScore;

        ShooterSession m_LastSession;

        void Start()
        {
            m_LastSession = GameManager.Instance.GetCurrentSession();
            SetUI();
        }

        void SetUI()
        {
            m_NewHighScore.gameObject.SetActive(false);


            m_Score.text = m_LastSession.Score.ToString();
            m_Coins.text = GameManager.Instance.GetCoinsInHand().ToString();

            if (GameManager.Instance.IsNewRecord(m_LastSession.Score))
            {
                GameManager.Instance.UpdateHiScoreTable(m_LastSession.Score, "Praveen");
            }

            //if (goUI.isRecord)
            //{
            //    m_NewHighScore.SetActive(true);
            //}
        }

        public void OnPressedStartBtn()
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }

        public void OnPressHomeBtn()
        {
            SceneManager.LoadScene("Start", LoadSceneMode.Single);
        }
    }
}

