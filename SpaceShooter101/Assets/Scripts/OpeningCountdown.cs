using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace praveen.One
{
    public class OpeningCountdown : MonoBehaviour
    {
        [SerializeField] GameObject[] m_ToActivated;

        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Animator>().enabled = true;
        }

        /// <summary>
        /// Triggers at end of the animation
        /// </summary>
        public void EndOfAnimation()
        {
            foreach (var go in m_ToActivated)
            {
                go.SetActive(true);
            }
            SessionManager.Instance.StartGame();
        }
    }
}

