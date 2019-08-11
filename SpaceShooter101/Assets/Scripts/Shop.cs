using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace praveen.One
{
    [System.Serializable]
    public struct ShooterAmor
    {
        public int GunLevel;
        public int RocketLevel;

        public ShooterAmor(int gunLvl, int rocketLvl)
        {
            this.GunLevel = gunLvl;
            this.RocketLevel = rocketLvl;
        }
    }

    public class Shop : MonoBehaviour
    {
        static Dictionary<int, int> m_GunPowerDict     = new Dictionary<int, int>();
        static Dictionary<int, int> m_RocketPowerDict  = new Dictionary<int, int>();

        ShooterAmor m_ShooterAmor;


        #region SerializedFields
        [SerializeField] Text m_GunPowerLevel;
        [SerializeField] Text m_GunPowerCost;
        [SerializeField] Text m_RocketLevel;
        [SerializeField] Text m_RocketCost;
        [SerializeField] Text m_Coins;
        [SerializeField] Slider m_GunPowerSlider;
        [SerializeField] Slider m_RocketSlider;
        [SerializeField] Button m_GunUpgradeBtn;
        [SerializeField] Button m_RocketUpgradeBtn;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            SetUpgradeInfo();
            Init();
        }

        void SetUpgradeInfo()
        {
            m_GunPowerDict.Add(1, 0);
            m_GunPowerDict.Add(2, 10);
            m_GunPowerDict.Add(3, 15);
            m_GunPowerDict.Add(4, 27);
            m_GunPowerDict.Add(5, 50);

            m_RocketPowerDict.Add(1, 0);
            m_RocketPowerDict.Add(2, 11);
            m_RocketPowerDict.Add(3, 25);
            m_RocketPowerDict.Add(4, 47);
            m_RocketPowerDict.Add(5, 70);
        }

        void Init()
        {
            m_ShooterAmor = GameManager.Instance.GetAmorData();

            m_GunPowerSlider.value  = m_ShooterAmor.GunLevel;
            m_RocketSlider.value    = m_ShooterAmor.RocketLevel;
            m_Coins.text            = GameManager.Instance.GetCoinCount().ToString();

            m_GunPowerLevel.text    = GetNextGunData(m_ShooterAmor.GunLevel);
            m_RocketLevel.text      = GetNextGunData(m_ShooterAmor.RocketLevel);

            int nextGunPowerCost = GetNextGunCost(m_ShooterAmor.GunLevel);
            int nextRocketCost = GetNextRocketCost(m_ShooterAmor.RocketLevel);

            if (nextGunPowerCost != -1)
            {
                m_GunPowerCost.text = nextGunPowerCost.ToString();

                // set the color of the text
                if (nextGunPowerCost <= GameManager.Instance.GetCoinCount())
                {
                    m_GunPowerCost.color = Color.green;
                }
                else
                {
                    m_GunPowerCost.color = Color.red;
                }
                
            }

            if (nextRocketCost != -1)
            {
                m_RocketCost.text = nextRocketCost.ToString();

                // set the color of the text
                if (nextRocketCost <= GameManager.Instance.GetCoinCount())
                {
                    m_RocketCost.color = Color.green;
                }
                else
                {
                    m_RocketCost.color = Color.red;
                }
            }

        }

        string GetNextGunData(int curLevel)
        {
            if (curLevel == m_GunPowerDict.OrderByDescending(x => x.Value).First().Key)
            {
                m_GunUpgradeBtn.interactable = false;
                return "Already Upgraded to MAX level";
            }
            return "Upgrade to Lvel "+ (curLevel +1) +" for:";
        }

        int GetNextGunCost(int curLevel)
        {
            if(curLevel == m_GunPowerDict.OrderByDescending(x => x.Value).First().Key)
            {
                return -1;
            }
            return m_GunPowerDict[curLevel+1];
        }

        string GetNextRocketData(int curLevel)
        {
            if (curLevel == m_RocketPowerDict.OrderByDescending(x => x.Value).First().Key)
            {
                m_RocketUpgradeBtn.interactable = false;
                return "Already Upgraded to MAX level";
            }
            return "Upgrade to Lvel " + (curLevel + 1) + " for:";
        }

        int GetNextRocketCost(int curLevel)
        {
            if (curLevel == m_RocketPowerDict.OrderByDescending(x => x.Value).First().Key)
            {
                return -1;
            }
            return m_RocketPowerDict[curLevel + 1];
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
            Debug.Log("Upgrade Gun Btn");
            GameManager.Instance.UpgradeGun();
            Init();
        }

        public void OnClickUpgradeRocket()
        {
            Debug.Log("Upgrade Rocket Btn");
            GameManager.Instance.UpgradeRocket();
            Init();
        }

        public static int GetGunUpgradeCost(int lvl)
        {
            return m_GunPowerDict[lvl];
        }

        public static int GetRocketUpgradeCost(int lvl)
        {
            return m_RocketPowerDict[lvl];
        }
    }
}

