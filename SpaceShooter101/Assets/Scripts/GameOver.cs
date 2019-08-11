using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace praveen.One
{
    public struct GameOverUI
    {
        public int Score;
        public int HighScore;
        public int Coins;
        public bool isRecord;

        public GameOverUI(int score, int highScore, int coins, bool newRecord)
        {
            this.Score      = score;
            this.HighScore  = highScore;
            this.Coins      = coins;
            this.isRecord   = newRecord;
        }
    }

    public class GameOver : MonoBehaviour
    {
        [SerializeField] Text m_HighScoreText;
        [SerializeField] Text m_Score;
        [SerializeField] Text m_Coins;
        [SerializeField] GameObject m_NewHighScore;

        void Start()
        {
            SetUI();
        }

        void SetUI()
        {
            m_NewHighScore.gameObject.SetActive(false);

            GameOverUI goUI = GameManager.Instance.GetGameOverUI();
            m_HighScoreText.text = goUI.HighScore.ToString();
            m_Score.text = goUI.Score.ToString();
            m_Coins.text = goUI.Coins.ToString();

            if (goUI.isRecord)
            {
                m_NewHighScore.SetActive(true);
            }
        }

        public void OnPressedStartBtn()
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }
    }
}

