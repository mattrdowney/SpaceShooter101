using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace praveen.One
{
    public class OpeningCountdown : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Animator>().enabled = true;
        }

        public void EndOfAnimation()
        {
            SessionManager.Instance.StartGame();
        }
    }
}

