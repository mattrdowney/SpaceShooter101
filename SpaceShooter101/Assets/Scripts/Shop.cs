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
        public int MissileMagazineLvl;
        public int MissileCount;

        public ShooterAmor(int gunLvl, int missileMagLvl, int missileCount)
        {
            this.GunLevel           = gunLvl;
            this.MissileMagazineLvl = missileMagLvl;
            this.MissileCount       = missileCount;
        }
    }

    public struct MissileMagazine
    {
        public int Cost;
        public int Capacity;

        public MissileMagazine(int cost, int capacity)
        {
            this.Cost       = cost;
            this.Capacity   = capacity;
        }
    }

    public struct Shield
    {
        public int Cost;
        public int Duration;

        public Shield(int cost, int duration)
        {
            this.Cost = cost;
            this.Duration = duration;
        }
    }

    public class Shop : MonoBehaviour
    {
        static Dictionary<int, int> m_GunPowerDict                   = new Dictionary<int, int>();
        static Dictionary<int, MissileMagazine> m_MissileMagazine    = new Dictionary<int, MissileMagazine>();
        static Dictionary<int, Shield> m_ShieldDict                  = new Dictionary<int, Shield>();

        ShooterAmor m_ShooterAmor;

        static int m_MissileCost;


        #region SerializedFields
        [SerializeField] Text m_GunPowerLevel;
        [SerializeField] Text m_GunPowerCost;
        [SerializeField] Text m_RocketLevel;
        [SerializeField] Text m_RocketCost;
        [SerializeField] Text m_ShieldCost;
        [SerializeField] Text m_ShieldLevel;
        [SerializeField] Text m_Coins;
        [SerializeField] Text m_MissileCount;
        [SerializeField] Text m_MissileCostTxt;
        [SerializeField] Slider m_GunPowerSlider;
        [SerializeField] Slider m_MagazineSlider;
        [SerializeField] Slider m_MissileSlider;
        [SerializeField] Slider m_ShiledSlider;
        [SerializeField] Button m_GunUpgradeBtn;
        [SerializeField] Button m_MagazineUpgrdBtn;
        [SerializeField] Button m_MissileAddBtn;
        [SerializeField] Button m_ShieldBtn;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            m_MissileCost = 15;
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

                             // Level                 cost capacity
            m_MissileMagazine.Add(1, new MissileMagazine(0, 1));
            m_MissileMagazine.Add(2, new MissileMagazine(20, 3));
            m_MissileMagazine.Add(3, new MissileMagazine(25, 7));
            m_MissileMagazine.Add(4, new MissileMagazine(40, 10));
            m_MissileMagazine.Add(5, new MissileMagazine(60, 20));

                        // Level       cost  seconds
            m_ShieldDict.Add(1, new Shield(0, 3));
            m_ShieldDict.Add(2, new Shield(20, 7));
            m_ShieldDict.Add(3, new Shield(25, 12));
            m_ShieldDict.Add(4, new Shield(40, 10));
            m_ShieldDict.Add(5, new Shield(60, 20));
        }

        void Init()
        {
            m_ShooterAmor = GameManager.Instance.GetAmorData();

            m_GunPowerSlider.maxValue   = m_GunPowerDict.Count();
            m_MagazineSlider.maxValue   = m_MissileMagazine.Count();
            m_ShiledSlider.maxValue     = m_ShieldDict.Count();


            m_GunPowerSlider.value  = m_ShooterAmor.GunLevel;
            m_MagazineSlider.value  = m_ShooterAmor.MissileMagazineLvl;
            m_Coins.text            = GameManager.Instance.GetCoinCount().ToString();

            m_GunPowerLevel.text    = GetNextGunData(m_ShooterAmor.GunLevel);
            m_RocketLevel.text      = GetNextGunData(m_ShooterAmor.MissileMagazineLvl);

            int nextGunPowerCost = GetNextGunCost(m_ShooterAmor.GunLevel);
            int nextMissileMagCost = GetNextMissileMagazineCost(m_ShooterAmor.MissileMagazineLvl);
            

            if (nextGunPowerCost != -1)
            {
                m_GunPowerCost.text = nextGunPowerCost.ToString();

                // set the color of the text
                if (nextGunPowerCost <= GameManager.Instance.GetCoinCount())
                {
                    m_GunPowerCost.color = Color.green;
                    m_GunUpgradeBtn.interactable = true;
                }
                else
                {
                    m_GunPowerCost.color = Color.red;
                    m_GunUpgradeBtn.interactable = false;
                }
                
            }

            if (nextMissileMagCost != -1)
            {
                m_RocketCost.text = nextMissileMagCost.ToString();

                // set the color of the text
                if (nextMissileMagCost <= GameManager.Instance.GetCoinCount())
                {
                    m_MagazineUpgrdBtn.interactable = true;
                    m_RocketCost.color = Color.green;
                }
                else
                {
                    m_MagazineUpgrdBtn.interactable = false;
                    m_RocketCost.color = Color.red;
                }
            }

            m_MissileCostTxt.text = m_MissileCost.ToString();
            if(m_MissileCost <= GameManager.Instance.GetCoinCount())
            {
                m_MissileAddBtn.interactable = true;
                m_MissileCostTxt.color = Color.green;
            }
            else
            {
                m_MissileAddBtn.interactable = false;
                m_MissileCostTxt.color = Color.red;
            }

            int missileCount = m_ShooterAmor.MissileCount;
            int missileCap = GetMagazineCapacityBylvl(m_ShooterAmor.MissileMagazineLvl);
            m_MissileCount.text = "Missile Count:"+ missileCount+"/"+ missileCap;

            m_MissileSlider.maxValue = missileCap;
            m_MissileSlider.value = missileCount;

            m_ShiledSlider.value = GetCurrentShieldLevel(GameManager.Instance.GetShieldDuration());

            int shieldLvl = GetCurrentShieldLevel(GameManager.Instance.GetShieldDuration());
            int shieldCost = GetNextShieldCost(shieldLvl);
            m_ShieldLevel.text = GetNextShieldData(shieldLvl);
            m_ShieldCost.text = shieldCost.ToString();

            if(shieldCost <= GameManager.Instance.GetCoinCount())
            {
                m_ShieldCost.color = Color.green;
                m_ShieldBtn.interactable = true;
            }
            else
            {
                m_ShieldCost.color = Color.red;
                m_ShieldBtn.interactable = false;
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

        string GetNextMissileMagazineData(int curLevel)
        {
            if (curLevel == m_MissileMagazine.OrderByDescending(x => x.Value).First().Key)
            {
                m_MagazineUpgrdBtn.interactable = false;
                return "Already Upgraded to MAX level";
            }
            return "Upgrade to Lvel " + (curLevel + 1) + " for:";
        }

        string GetNextShieldData(int curLevel)
        {
            if (curLevel == m_ShieldDict.OrderByDescending(x => x.Key).First().Key)
            {
                m_ShieldBtn.interactable = false;
                return "Already Upgraded to MAX level";
            }
            return "Upgrade to " + m_ShieldDict[(curLevel + 1)].Duration + " sec. for:";
        }

        int GetNextMissileMagazineCost(int curLevel)
        {
            if (curLevel == m_MissileMagazine.OrderByDescending(x => x.Value.Cost).First().Key)
            {
                return -1;
            }
            return m_MissileMagazine[curLevel + 1].Cost;
        }

        int GetNextShieldCost(int curLevel)
        {
            if (curLevel == m_ShieldDict.OrderByDescending(x => x.Value.Cost).First().Key)
            {
                return -1;
            }
            return m_ShieldDict[curLevel + 1].Cost;
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
            GameManager.Instance.UpgradeGun((bool sucess)=>
            {
                if (sucess)
                {
                    Init();
                }
            });
            
        }

        public void OnClickUpgrdMagazine()
        {
            GameManager.Instance.UpgradeMagazine((bool sucess) =>
            {
                if (sucess)
                {
                    Init();
                }
            });
        }

        public void OnClickBuyMissile()
        {
            GameManager.Instance.BuyMissile((bool sucess) =>
            {
                if (sucess)
                {
                    Init();
                }
            });
        }

        public void OnUpgradeShield()
        {
            GameManager.Instance.BuyShield((bool sucess) =>
            {
                if (sucess)
                {
                    Init();
                }
            });
        }


        public static int GetGunUpgradeCost(int lvl)
        {
            if (m_GunPowerDict.ContainsKey(lvl))
            {
                return m_GunPowerDict[lvl];
            }

            return -1;
            
        }

        public static int GetMagazineCapacityBylvl(int lvl)
        {
            if (m_MissileMagazine.ContainsKey(lvl))
            {
                return m_MissileMagazine[lvl].Capacity;
            }

            return -1;
        }

        public static int GetMissileMagUpgrdCost(int lvl)
        {
            if (m_MissileMagazine.ContainsKey(lvl))
            {
                return m_MissileMagazine[lvl].Cost;
            }

            return -1;
            
        }

        public static int GetMissileCost()
        {
            return m_MissileCost;
        }

        public static int GetNextGunLevel(int lvl)
        {
            if (lvl == m_GunPowerDict.OrderByDescending(x => x.Key).First().Key)
            {
                return -1;
            }
            return lvl + 1;
        }

        public static int GetNextMissileMagLvl(int lvl)
        {
            if (lvl == m_MissileMagazine.OrderByDescending(x => x.Key).First().Key)
            {
                return -1;
            }
            return lvl + 1;
        }

        public static int GetCurrentShieldLevel(int duration)
        {
            return m_ShieldDict.FirstOrDefault(x => x.Value.Duration == duration).Key;
        }

        public static Shield GetNextShieldDataByLvl(int lvl)
        {
            if (lvl == m_ShieldDict.OrderByDescending(x => x.Key).First().Key)
            {
                return new Shield(-1,-1);
            }
            return m_ShieldDict[lvl + 1];
        }
    }
}

