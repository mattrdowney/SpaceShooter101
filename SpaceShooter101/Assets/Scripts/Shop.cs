using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace praveen.One
{
    public class Shop : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnClickHomeButton()
        {
            SceneManager.LoadScene("Start", LoadSceneMode.Single);
        }

        public void OnClickBackToGame()
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }

        public void OnClickUpgradeGun()
        {

        }

        public void OnClickUpgradeRocket()
        {

        }
    }
}

