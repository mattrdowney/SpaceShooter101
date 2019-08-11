using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace praveen.One
{

    public class StartMenu : MonoBehaviour
    {
        [SerializeField] Text m_HighScoreText;

        void Start()
        {
            m_HighScoreText.text = GameManager.Instance.GetHighScore().ToString();
        }

        public void OnPressedStartBtn()
        {
            SceneManager.LoadScene("Game",LoadSceneMode.Single);
        }
    }
}


