using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace praveen.One
{

    public class StartMenu : MonoBehaviour
    {
        [SerializeField] Transform m_ScrollContainer;
        [SerializeField] GameObject m_HighScoreEntry;

        List<HighScore> m_HighScoreList;

        void Start()
        {
            m_HighScoreList = GameManager.Instance.GetHighScore();
            SetHighScoreTabele();
        }

        /// <summary>
        /// draw high score table
        /// </summary>
        void SetHighScoreTabele()
        {
            int i = 0;
            foreach (var highScore in m_HighScoreList)
            {
                GameObject go = Instantiate(m_HighScoreEntry, m_ScrollContainer);
                go.GetComponent<HighScoreEntry>().SetData(i, highScore.Score, highScore.Name);
                i++;
            }
        }

        /// <summary>
        /// On press start button
        /// </summary>
        public void OnPressedStartBtn()
        {
            SceneManager.LoadScene("Game",LoadSceneMode.Single);
        }
    }
}


