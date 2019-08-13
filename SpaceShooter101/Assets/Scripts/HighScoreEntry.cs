using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace praveen.One
{

    public class HighScoreEntry : MonoBehaviour
    {
        [SerializeField] Text m_Score;
        [SerializeField] Text m_PlayerName;
        [SerializeField] GameObject[] m_Star;

        /// <summary>
        /// Initialized the high score entry
        /// </summary>
        /// <param name="star"></param>
        /// <param name="score"></param>
        /// <param name="name"></param>
        public void SetData(int star, int score, string name)
        {
            m_Score.text = score.ToString();

            if(name.Length> 4)
            {
                m_PlayerName.text = name.Substring(0, 4);
            }
            else
            {
                m_PlayerName.text = name;
            }
            

            switch (star)
            {
                case 0:
                    m_Star[0].SetActive(true);
                    break;
                case 1:
                    m_Star[1].SetActive(true);
                    break;
                case 2:
                    m_Star[2].SetActive(true);
                    break;
            }
        }
    }
}

