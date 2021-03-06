﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace praveen.One
{
    // REVIEW (architecture): this is the class with the most need for an architecture refactoring. I don't know the right way to go about it, though.
    // REVIEW (maintainability): I put structs/classes in their own file (unless I use the "internal" keyword instead of public/private/protected which is extremely niche in C#).
    // REVIEW (readability): somewhere between a typo and an incorrect name; ShooterArmor would fix the typo but not describe what the struct represents; consider "GunStats"
    // REVIEW (readability): In general, when classes get to ~200-500+ lines, you should start considering refactoring them into smaller files.
    [System.Serializable]
    public struct ShooterAmor
    {
        public int GunLevel;
        public int MissileMagazineLvl;
        public int MagazineCapacity;
        public int MissileCount;

        // REVIEW (readability): in this case you could probably get away with object initializers instead of a constructor ( https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/how-to-initialize-objects-by-using-an-object-initializer )
        public ShooterAmor(int gunLvl, int missileMagLvl, int magCapacity, int missileCount)
        {
            this.GunLevel           = gunLvl;
            this.MissileMagazineLvl = missileMagLvl;
            this.MagazineCapacity   = magCapacity;
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
        //static Dictionary<int, int> m_GunPowerDict                   = new Dictionary<int, int>();
        //static Dictionary<int, MissileMagazine> m_MissileMagazine    = new Dictionary<int, MissileMagazine>();
        //static Dictionary<int, Shield> m_ShieldDict                  = new Dictionary<int, Shield>();

        // REVIEW (compliment): nicely done on the composition (so you don't require code folding like below)
        ShooterAmor m_ShooterAmor;

        static int m_MissileCost;
        static int m_LifeCost;
        bool m_BoughtLife;

        // REVIEW (readability, extremely opinionated): I never use #regions or code folding because you can instead: 1) create data container objects 2) reduce filesize 3) create more methods
        // REVIEW (correctness): I don't think any of the below types can be serialized.
        #region SerializedFields
        [SerializeField] Text m_Coins;
        [SerializeField] Text m_MissileCount;

        [SerializeField] Text m_GunPowerInfo;
        [SerializeField] Text m_MagazineInfo;
        [SerializeField] Text m_MissileInfoText;
        [SerializeField] Text m_ShieldInfo;
        [SerializeField] Text m_LifeInfo;

        [SerializeField] Slider m_GunPowerSlider;
        [SerializeField] Slider m_MagazineSlider;
        [SerializeField] Slider m_MissileSlider;
        [SerializeField] Slider m_ShiledSlider;
        [SerializeField] Slider m_LifeSlider;

        [SerializeField] Button m_GunUpgradeBtn;
        [SerializeField] Button m_MagazineUpgrdBtn;
        [SerializeField] Button m_MissileAddBtn;
        [SerializeField] Button m_ShieldBtn;
        [SerializeField] Button m_LifeBtn;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            m_BoughtLife = false;
            m_MissileCost = 15;
            m_LifeCost = 30;
            SetUpgradeInfo();
            Init();
        }

        // REVIEW (readability): you have this code in version control, so just remove unused implementation (if you need it again you can always get it from version control).
        /// <summary>
        /// Initialize upgrade data
        /// </summary>
        void SetUpgradeInfo()
        {
            // clear previous data if exists
            //m_GunPowerDict.Clear();
            //m_MissileMagazine.Clear();
            //m_ShieldDict.Clear();


            //m_GunPowerDict.Add(1, 0);
            //m_GunPowerDict.Add(2, 10);
            //m_GunPowerDict.Add(3, 15);
            //m_GunPowerDict.Add(4, 27);
            //m_GunPowerDict.Add(5, 50);

            //                 // Level                 cost capacity
            //m_MissileMagazine.Add(1, new MissileMagazine(0, 1));
            //m_MissileMagazine.Add(2, new MissileMagazine(20, 3));
            //m_MissileMagazine.Add(3, new MissileMagazine(25, 7));
            //m_MissileMagazine.Add(4, new MissileMagazine(40, 10));
            //m_MissileMagazine.Add(5, new MissileMagazine(60, 20));

            //            // Level       cost  seconds
            //m_ShieldDict.Add(1, new Shield(0, 3));
            //m_ShieldDict.Add(2, new Shield(20, 7));
            //m_ShieldDict.Add(3, new Shield(25, 12));
            //m_ShieldDict.Add(4, new Shield(40, 10));
            //m_ShieldDict.Add(5, new Shield(60, 20));
        }

        /// <summary>
        /// Initialized the shop data
        /// </summary>
        void Init()
        {
            int coinsInHand = GameManager.Instance.GetCoinsInHand();
            m_ShooterAmor   = GameManager.Instance.GetCurrentAmorData();

            m_Coins.text = coinsInHand.ToString();

            int nextGunPowerCost = GetNextGunCost(m_ShooterAmor.GunLevel);
            int nextMissileMagCost = GetNextMissileMagazineCost(m_ShooterAmor.MissileMagazineLvl);

            // REVIEW (architecture): maybe it would be a good idea to add a helper function (or class) that deals with this maxValue, value, interactable, text pattern in the code.
            //////
            // Set Gun Power Related Stuff
            //////
            m_GunPowerSlider.maxValue   = DataBank.GetGunPowerData().Count();
            m_GunPowerSlider.value      = m_ShooterAmor.GunLevel;
            m_GunUpgradeBtn.interactable = SetButtonIntractable(nextGunPowerCost, coinsInHand);
            m_GunPowerInfo.text = GetGunPowerInfoText(m_ShooterAmor.GunLevel);

            //////
            // Set Missile Magazine Related Stuff
            //////
            m_MagazineSlider.maxValue   = DataBank.GetMissileMagazineData().Count();
            m_MagazineSlider.value = m_ShooterAmor.MissileMagazineLvl;
            m_MagazineUpgrdBtn.interactable = SetButtonIntractable(nextMissileMagCost,coinsInHand);
            m_MagazineInfo.text = GetMagazineInfoText(m_ShooterAmor.MissileMagazineLvl);

            //////
            // Set Missile Related Stuff
            //////
            int missileCount = m_ShooterAmor.MissileCount;
            int missileCap = m_ShooterAmor.MagazineCapacity;
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
            m_ShiledSlider.maxValue = DataBank.GetShieldData().Count();
            m_ShiledSlider.value = shieldLvl;
            m_ShieldInfo.text = GetShieldInfoText(shieldLvl);
            m_ShieldBtn.interactable = SetButtonIntractable(shieldCost, coinsInHand);

            //////
            // Set Life Related Stuff
            //////
            m_LifeSlider.value = GameManager.Instance.LifesLeft();
            m_LifeBtn.interactable = SetButtonIntractable(m_LifeCost, coinsInHand);
            m_LifeInfo.text = GetLifeInfoText(GameManager.Instance.LifesLeft());

        }

        // REVIEW (compliment): you have great function names in my opinion.
        // REVIEW (mutating getter): functions are either (inspectors -- readonly / const; mutators -- change global or local state somehow including input/output); you have a getter that is mutating the game state with "m_GunUpgradeBtn.interactable = false;"
        // REVIEW (commenting): the summary is redundant, I would personally write "Inspector (with above caveat) -- Fetches the gun's current power level e.g. to be printed on the user interface for the player."
        // REVIEW (maintainability): my personal opinion is that documenting parameters and return types is often more important than the summary (at least when you have good function names), since those are the easiest to misunderstand. E.g. this could be 1) the gun's level 2) the player's level 3) the world level
        /// <summary>
        /// Get the gun power information text
        /// </summary>
        /// <param name="currentLvl"></param>
        /// <returns></returns>
        string GetGunPowerInfoText(int currentLvl)
        {
            // REVIEW (information without recommendation): Linq is incredibly powerful, and useful (this code should be 100% fine); that being said in areas where performance is crucial, game developers are particularly reluctant to use Linq. 
            if (currentLvl == DataBank.GetGunPowerData().OrderByDescending(x => x.Value).First().Key)
            {
                m_GunUpgradeBtn.interactable = false;
                return "<color=#fd0000>Already Upgraded to MAX level</color>";
            }
            return "<color=#cfd2d4> Upgrade to Lvel " + (currentLvl + 1) + " for: </color>"+
                GetCostString(DataBank.GetGunPowerData()[currentLvl + 1]);
            
        }

        /// <summary>
        /// Get Magazine information text
        /// </summary>
        /// <param name="currentLvl"></param>
        /// <returns></returns>
        string GetMagazineInfoText(int currentLvl)
        {
            if (currentLvl == DataBank.GetMissileMagazineData().OrderByDescending(x => x.Key).First().Key)
            {
                m_MagazineUpgrdBtn.interactable = false;
                return "<color=#fd0000>Already Upgraded to MAX level</color>";
            }
            return "<color=#cfd2d4> Upgrade to Lvel " + (currentLvl + 1) + " for: </color>" +
                GetCostString(DataBank.GetMissileMagazineData()[currentLvl + 1].Cost);
        }

        /// <summary>
        /// Get missile capacity information text
        /// </summary>
        /// <returns></returns>
        string GetMissileCapacityInfoText()
        {
            if (m_ShooterAmor.MagazineCapacity <= m_ShooterAmor.MissileCount)
            {
                m_MissileAddBtn.interactable = false;
                return "<color=#fd0000>Storage Already Full !</color>";
            }
            return "<color=#cfd2d4> Buy One Missile for:</color>" +
                GetCostString(m_MissileCost);
        }

        // REVIEW (design pattern): there seems to be a lot of similar code here, which indicates you might be able to create a .ToString() or better .ToShopInfo() function for e.g. the Shield class (and all the other classes).
        /// <summary>
        /// Get Shield infomation text
        /// </summary>
        /// <param name="currentLvl"></param>
        /// <returns></returns>
        string GetShieldInfoText(int currentLvl)
        {
            int shieldLvl = GetCurrentShieldLevel(GameManager.Instance.GetCurrentShield().Duration);
            int shieldCost = GetNextShieldCost(shieldLvl);

            if (currentLvl == DataBank.GetShieldData().OrderByDescending(x => x.Key).First().Key)
            {
                m_ShieldBtn.interactable = false;
                return "<color=#fd0000>Already Upgraded to MAX level</color>";
            }
            return "<color=#cfd2d4> Upgrade to " + DataBank.GetShieldData()[(currentLvl + 1)].Duration + " sec. for :</color>" +
                GetCostString(shieldCost);
        }

        /// <summary>
        /// Get Life information text
        /// </summary>
        /// <param name="lives"></param>
        /// <returns></returns>
        string GetLifeInfoText(int lives)
        {
            if (m_BoughtLife)
            {
                m_LifeBtn.interactable = false;
                return "<color=#fd0000>Already bought!</color>";
            }
            if (lives == 3)
            {
                m_LifeBtn.interactable = false;
                return "<color=#fd0000>Has Max Life count!</color>";
            }
            return "<color=#cfd2d4> Buy One Life for : </color>" + GetCostString(m_LifeCost);
        }

        
        /// <summary>
        /// Get coin availability with formating
        /// </summary>
        /// <param name="upgradeCost"></param>
        /// <returns></returns>
        string GetCostString(int upgradeCost)
        {
            if(upgradeCost <= GameManager.Instance.GetCoinsInHand())
            {
                return "<color=#00dd25>" + upgradeCost + "</color>"; // green
            }
            return "<color=#fd0000>" + upgradeCost + "</color>";  // red
        }

        /// <summary>
        /// Get the next gun levels cost
        /// </summary>
        /// <param name="curLevel"></param>
        /// <returns></returns>
        int GetNextGunCost(int curLevel)
        {
            if (curLevel == DataBank.GetGunPowerData().OrderByDescending(x => x.Value).First().Key)
            {
                return -1;
            }
            return DataBank.GetGunPowerData()[curLevel + 1];
        }

        /// <summary>
        /// Get missile magazine cost
        /// </summary>
        /// <param name="curLevel"></param>
        /// <returns></returns>
        int GetNextMissileMagazineCost(int curLevel)
        {
            if (curLevel == DataBank.GetMissileMagazineData().OrderByDescending(x => x.Value.Cost).First().Key)
            {
                return -1;
            }
            return DataBank.GetMissileMagazineData()[curLevel + 1].Cost;
        }

        /// <summary>
        /// Get Shield Cost
        /// </summary>
        /// <param name="curLevel"></param>
        /// <returns></returns>
        int GetNextShieldCost(int curLevel)
        {
            if (curLevel == DataBank.GetShieldData().OrderByDescending(x => x.Value.Cost).First().Key)
            {
                return -1;
            }
            return DataBank.GetShieldData()[curLevel + 1].Cost;
        }

        /// <summary>
        /// Click the home button
        /// </summary>
        public void OnClickHomeButton()
        {
            GameManager.Instance.ForceGameOver();
        }

        /// <summary>
        /// Go back to the game
        /// </summary>
        public void OnClickBackToGame()
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }

        /// <summary>
        /// Set buttons intractable or not
        /// </summary>
        /// <param name="cost"></param>
        /// <param name="coinsInHand"></param>
        /// <returns></returns>
        bool SetButtonIntractable(int cost, int coinsInHand)
        {
            return cost <= coinsInHand;
        }

        // REVIEW (readability): I don't have a suggestion here, but this seems like a code smell that could be removed with better organization.
        /// <summary>
        /// Click upgrade gun button
        /// </summary>
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


        /// <summary>
        /// Click upgrade magazine button
        /// </summary>
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

        /// <summary>
        /// Onclick buy missile button
        /// </summary>
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

        /// <summary>
        /// On click shiled upgrade button
        /// </summary>
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

        /// <summary>
        /// On click buy life button
        /// </summary>
        public void OnClickBuyLife()
        {
            GameManager.Instance.BuyLife(30, (bool sucess) =>
            {
                if (sucess)
                {
                    
                    m_LifeBtn.interactable = false;
                    m_BoughtLife = true;
                    Init();
                }
            });
        }

        /// <summary>
        /// Get the gun upgrade cost
        /// </summary>
        /// <param name="lvl"></param>
        /// <returns></returns>
        public static int GetGunUpgradeCost(int lvl)
        {
            if (DataBank.GetGunPowerData().ContainsKey(lvl))
            {
                return DataBank.GetGunPowerData()[lvl];
            }

            return -1;
            
        }

        /// <summary>
        /// Get Missile magazine upgrade cost
        /// </summary>
        /// <param name="lvl"></param>
        /// <returns></returns>
        public static int GetMissileMagUpgrdCost(int lvl)
        {
            if (DataBank.GetMissileMagazineData().ContainsKey(lvl))
            {
                return DataBank.GetMissileMagazineData()[lvl].Cost;
            }

            return -1;
            
        }

        /// <summary>
        /// Get missile cost
        /// </summary>
        /// <returns></returns>
        public static int GetMissileCost()
        {
            return m_MissileCost;
        }

        /// <summary>
        /// Get next gun level
        /// </summary>
        /// <param name="lvl"></param>
        /// <returns></returns>
        public static int GetNextGunLevel(int lvl)
        {
            if (lvl == DataBank.GetGunPowerData().OrderByDescending(x => x.Key).First().Key)
            {
                return -1;
            }
            return lvl + 1;
        }

        /// <summary>
        /// Get next missile magazine level
        /// </summary>
        /// <param name="lvl"></param>
        /// <returns></returns>
        public static int GetNextMissileMagLvl(int lvl)
        {
            if (lvl == DataBank.GetMissileMagazineData().OrderByDescending(x => x.Key).First().Key)
            {
                return -1;
            }
            return lvl + 1;
        }

        /// <summary>
        /// Get Magazine capacity by level
        /// </summary>
        /// <param name="lvl"></param>
        /// <returns></returns>
        public static int GetMagazineCapacityByLvl(int lvl)
        {
            if (DataBank.GetMissileMagazineData().ContainsKey(lvl))
            {
                return DataBank.GetMissileMagazineData()[lvl].Capacity;
            }

            return -1;
        }

        /// <summary>
        /// Get Current shiled level by duration
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static int GetCurrentShieldLevel(int duration)
        {
            return DataBank.GetShieldData().FirstOrDefault(x => x.Value.Duration == duration).Key;
        }

        /// <summary>
        /// Get next shield data by level
        /// </summary>
        /// <param name="lvl"></param>
        /// <returns></returns>
        public static Shield GetNextShieldDataByLvl(int lvl)
        {
            if (lvl == DataBank.GetShieldData().OrderByDescending(x => x.Key).First().Key)
            {
                return new Shield(-1,-1);
            }
            return DataBank.GetShieldData()[lvl + 1];
        }
    }
}

