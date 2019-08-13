using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace praveen.One
{
    public class OpeningCountdown : MonoBehaviour
    {
        [SerializeField] GameObject m_PauseBtn;

        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Animator>().enabled = true;
        }

        public void EndOfAnimation()
        {
            m_PauseBtn.SetActive(true);
            SessionManager.Instance.StartGame();
        }
    }
}

