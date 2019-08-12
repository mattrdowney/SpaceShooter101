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
        [SerializeField] Text m_Coins;
        [SerializeField] Text m_MissileCount;

        [SerializeField] Text m_GunPowerInfo;
        [SerializeField] Text m_MagazineInfo;
        [SerializeField] Text m_MissileInfoText;
        [SerializeField] Text m_ShieldInfo;
        
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
            int coinsInHand = GameManager.Instance.GetCoinsInHand();
            m_ShooterAmor   = GameManager.Instance.GetAmorData();

            m_Coins.text = coinsInHand.ToString();

            int nextGunPowerCost = GetNextGunCost(m_ShooterAmor.GunLevel);
            int nextMissileMagCost = GetNextMissileMagazineCost(m_ShooterAmor.MissileMagazineLvl);


            //////
            // Set Gun Power Related Stuff
            //////
            m_GunPowerSlider.maxValue   = m_GunPowerDict.Count();
            m_GunPowerSlider.value      = m_ShooterAmor.GunLevel;
            m_GunUpgradeBtn.interactable = SetButtonIntractable(nextGunPowerCost, coinsInHand);
            m_GunPowerInfo.text = GetGunPowerInfoText(m_ShooterAmor.GunLevel);

            //////
            // Set Missile Magazine Related Stuff
            //////
            m_MagazineSlider.maxValue   = m_MissileMagazine.Count();
            m_MagazineSlider.value = m_ShooterAmor.MissileMagazineLvl;
            m_MagazineUpgrdBtn.interactable = SetButtonIntractable(nextMissileMagCost,coinsInHand);
            m_MagazineInfo.text = GetMagazineInfoText(m_ShooterAmor.MissileMagazineLvl);

            //////
            // Set Missile Related Stuff
            //////
            int missileCount = m_ShooterAmor.MissileCount;
            int missileCap = GetMagazineCapacityBylvl(m_ShooterAmor.MissileMagazineLvl);
            m_MissileSlider.maxValue = missileCap;
            m_MissileSlider.value = missileCount;
            m_MissileCount.text = "Missile Count:" + missileCount + "/" + missileCap;
            m_MissileAddBtn.interactable = SetButtonIntractable(m_MissileCost, coinsInHand);
            m_MissileInfoText.text = GetMissileCapacityInfoText();


            //////
            // Set Shield Related Stuff
            //////
            int shieldLvl = GetCurrentShieldLevel(GameManager.Instance.GetCurrentShield().Duration);
            int shieldCost = GetNextShieldCost(shieldLvl);
            m_ShiledSlider.maxValue = m_ShieldDict.Count();
            m_ShiledSlider.value = shieldLvl;
            m_ShieldInfo.text = GetShieldInfoText(shieldLvl);
            m_ShieldBtn.interactable = SetButtonIntractable(shieldCost, coinsInHand);

        }

        string GetGunPowerInfoText(int currentLvl)
        {
            if (currentLvl == m_GunPowerDict.OrderByDescending(x => x.Value).First().Key)
            {
                m_GunUpgradeBtn.interactable = false;
                return "<color=#fd0000>Already Upgraded to MAX level</color>";
            }
            return "<color=#cfd2d4> Upgrade to Lvel " + (currentLvl + 1) + " for: </color>"+
                GetCostString(m_GunPowerDict[currentLvl + 1]);
            
        }

        string GetMagazineInfoText(int currentLvl)
        {
            if (currentLvl == m_MissileMagazine.OrderByDescending(x => x.Key).First().Key)
            {
                m_MagazineUpgrdBtn.interactable = false;
                return "<color=#fd0000>Already Upgraded to MAX level</color>";
            }
            return "<color=#cfd2d4> Upgrade to Lvel " + (currentLvl + 1) + " for: </color>" +
                GetCostString(m_MissileMagazine[currentLvl + 1].Cost);
        }

        string GetMissileCapacityInfoText()
        {
            if (GetMagazineCapacityBylvl(m_ShooterAmor.MissileMagazineLvl) <= m_ShooterAmor.MissileCount)
            {
                m_MissileAddBtn.interactable = false;
                return "<color=#fd0000>Storage Already Full !</color>";
            }
            return "<color=#cfd2d4> Buy One Missile for:</color>" +
                GetCostString(m_MissileCost);
        }

        string GetShieldInfoText(int currentLvl)
        {
            int shieldLvl = GetCurrentShieldLevel(GameManager.Instance.GetCurrentShield().Duration);
            int shieldCost = GetNextShieldCost(shieldLvl);

            if (currentLvl == m_ShieldDict.OrderByDescending(x => x.Key).First().Key)
            {
                m_ShieldBtn.interactable = false;
                return "<color=#fd0000>Already Upgraded to MAX level</color>";
            }
            return "<color=#cfd2d4> Upgrade to " + m_ShieldDict[(currentLvl + 1)].Duration + " sec. for :</color>" +
                GetCostString(shieldCost);
        }

        

        string GetCostString(int upgradeCost)
        {
            if(upgradeCost <= GameManager.Instance.GetCoinsInHand())
            {
                return "<color=#00dd25>" + upgradeCost + "</color>"; // green
            }
            return "<color=#fd0000>" + upgradeCost + "</color>";  // red
        }

        int GetNextGunCost(int curLevel)
        {
            if (curLevel == m_GunPowerDict.OrderByDescending(x => x.Value).First().Key)
            {
                return -1;
            }
            return m_GunPowerDict[curLevel + 1];
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


        bool SetButtonIntractable(int cost, int coinsInHand)
        {
            return cost <= coinsInHand;
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

