using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace praveen.One
{
    public struct ShooterSession
    {
        public bool IsActive;
        public int Level;
        public int Score;
        public int Lifes;
        public float HP;
        public int EnemiesKilled;

        public ShooterSession(bool isActive, int level, int score, int lifes, float hp, int enemiesKilled)
        {
            this.IsActive   = isActive;
            this.Level      = level;
            this.Score      = score;
            this.Lifes      = lifes;
            this.HP         = hp;
            this.EnemiesKilled = enemiesKilled;
        }
    }

    [System.Serializable]
    public struct HighScore
    {
        public int Score;
        public string Name;

        public HighScore(int score, string name)
        {
            this.Score = score;
            this.Name = name;
        }
    }

    [System.Serializable]
    public struct ShooterData
    {
        public int CoinsInHand;
        public ShooterAmor Amor;
        public Shield CurrentShield;
        public List<HighScore> m_HighScoreTable;

        public ShooterData(int coins, ShooterAmor amor, Shield shield, List<HighScore> highScoreTable)
        {
            this.CoinsInHand = coins;
            this.Amor = amor;
            this.CurrentShield = shield;
            this.m_HighScoreTable = highScoreTable;
        }
    }

    public class GameManager : MonoBehaviour
    {
        #region singleton stuff
        private static GameManager m_Instance;

        public static GameManager Instance
        {
            get { return m_Instance; }
        }
        #endregion


        #region MetaData
        string m_ShooterDataKey = "SHOOTER.DATA";
        #endregion

        #region PrivateFields
        ShooterData m_ShooterData;
        ShooterSession m_Session;
        #endregion

        private void Awake()
        {
            if (m_Instance != null && m_Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                m_Instance = this;
            }
            DontDestroyOnLoad(this.gameObject);

            //PlayerPrefs.DeleteAll() ;

            ReadSavedData();
        }

        /// <summary>
        /// Get Saved Data
        /// </summary>
        void ReadSavedData()
        {
            string shooterData = PlayerPrefs.GetString(m_ShooterDataKey, null);

            if (string.IsNullOrEmpty(shooterData))
            {
                ShooterAmor amor    = new ShooterAmor(1, 1, 1, 1);
                Shield shield       = new Shield(0, 3);
                ShooterData data    = new ShooterData(0, amor, shield, LoadDummyData());

                m_ShooterData = data;
            }
            else
            {
                m_ShooterData = JsonUtility.FromJson<ShooterData>(shooterData);
            }

        }

        /// <summary>
        /// Demonostration purpose only!
        /// </summary>
        /// <returns></returns>
        List<HighScore> LoadDummyData(){
            List<HighScore> dummyList = new List<HighScore>();
            dummyList.Add(new HighScore(9, "John"));
            dummyList.Add(new HighScore(8, "Tim"));
            dummyList.Add(new HighScore(6, "Sara"));
            dummyList.Add(new HighScore(4, "Mark"));
            dummyList.Add(new HighScore(1, "Aura"));
            return dummyList;
        }

        /// <summary>
        /// Start new game session or continue previous session
        /// </summary>
        public void StartNewGameOrContinue()
        {
            if (m_Session.IsActive)
            {
                // Continue Current Session
                m_Session.Level += 1;
            }
            else
            {
                // Start New Game
                m_Session = new ShooterSession(true,1, 0 ,3 , 100 , 0);
            }

            UpdateHUD();

            SessionManager.Instance.StartSession();
        }


        /// <summary>
        /// Update Heads on Display
        /// </summary>
        void UpdateHUD()
        {
            HudController.Instance.SetPlayerLifes(m_Session.Lifes);
            HudController.Instance.SetPlayerHP(m_Session.HP/ 100);
            HudController.Instance.SetCoins(m_ShooterData.CoinsInHand);
            HudController.Instance.SetScore(m_Session.Score);
            HudController.Instance.EnemiesKilled(m_Session.EnemiesKilled);
            HudController.Instance.SetMissileData(m_ShooterData.Amor.MissileCount, m_ShooterData.Amor.MagazineCapacity);
        }

        /// <summary>
        /// Save Data in to disk
        /// </summary>
        void SaveData()
        {
            string shooterData = JsonUtility.ToJson(m_ShooterData);
            PlayerPrefs.SetString(m_ShooterDataKey, shooterData);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Adds score
        /// </summary>
        /// <param name="score"></param>
        public void AddScore(int score)
        {
            m_Session.Score += score;
            HudController.Instance.SetScore(m_Session.Score);
        }

        /// <summary>
        /// Update Enemy kill count
        /// </summary>
        public void UpdateEnemiesKilled()
        {
            m_Session.EnemiesKilled += 1;
            HudController.Instance.EnemiesKilled(m_Session.EnemiesKilled);
        }

        /// <summary>
        /// Triggerd when player got hit
        /// </summary>
        /// <param name="damage"></param>
        public void OnPlayerHit(float damage, Player player)
        {
            m_Session.HP -= damage;
            HudController.Instance.SetPlayerHP(m_Session.HP / 100);

            if(m_Session.HP < 0)
            {
                player.Destroy();
                if (m_Session.Lifes > 1)
                {
                    m_Session.HP = 100f;
                    m_Session.Lifes -= 1;
                    PlayerController.Instance.SpawnPlayer();
                    HudController.Instance.SetPlayerLifes(m_Session.Lifes);
                }
                else
                {
                    GameOver();
                }
            }
        }

        /// <summary>
        /// Get the current session data
        /// </summary>
        /// <returns></returns>
        public ShooterSession GetCurrentSession()
        {
            return m_Session;
        }

        /// <summary>
        /// Returns Level
        /// </summary>
        /// <returns></returns>
        public int GetLevel()
        {
            return m_Session.Level;
        }

        /// <summary>
        /// Returns world position of Upper Left Screen boundry
        /// </summary>
        /// <returns></returns>
        public Vector3 GetUpperLeftScreenBoundry()
        {
            return Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
        }

        /// <summary>
        /// Returns world position of Upper Right Screen boundry
        /// </summary>
        /// <returns></returns>
        public Vector3 GetUpperRightScreenBoundry()
        {
            return Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        }

        /// <summary>
        /// Returns Lower Screen Y
        /// </summary>
        /// <returns></returns>
        public float GetLowerScreenY()
        {
            return Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        }

        /// <summary>
        /// Returns Upper Screen Y
        /// </summary>
        /// <returns></returns>
        public float GetUpperScreenY()
        {
            return Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0)).y;
        }

        /// <summary>
        /// Get the current shield data
        /// </summary>
        /// <returns></returns>
        public Shield GetCurrentShield()
        {
            return m_ShooterData.CurrentShield;
        }

        /// <summary>
        /// Add Coins
        /// </summary>
        public void AddCoin()
        {
            m_ShooterData.CoinsInHand +=1;
            HudController.Instance.SetCoins(m_ShooterData.CoinsInHand);
        }

        /// <summary>
        /// Return the high Score
        /// </summary>
        /// <returns></returns>
        public List<HighScore> GetHighScore()
        {
            return m_ShooterData.m_HighScoreTable;
        }

        /// <summary>
        /// Lifes available for this session
        /// </summary>
        /// <returns></returns>
        public int LifesLeft()
        {
            return m_Session.Lifes;
        }

        /// <summary>
        /// Quit the gameplay before game over
        /// </summary>
        public void ForceGameOver()
        {
            m_Session.IsActive = false;
            SaveData();
            SceneManager.LoadScene("Start", LoadSceneMode.Single);
        }

        /// <summary>
        /// Game over
        /// </summary>
        private void GameOver()
        {
            m_Session.IsActive = false;

            SaveData();

            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);

        }

        /// <summary>
        /// Loads the shop scene after each round
        /// </summary>
        public void LoadShopScene()
        {
            SceneManager.LoadScene("ShopMenu", LoadSceneMode.Single);
        }

        /// <summary>
        /// Returns equiped amour
        /// </summary>
        /// <returns></returns>
        public ShooterAmor GetCurrentAmorData()
        {
            return m_ShooterData.Amor;
        }


        /// <summary>
        /// Returns available coin count
        /// </summary>
        /// <returns></returns>
        public int GetCoinsInHand()
        {
            return m_ShooterData.CoinsInHand;
        }

        /// <summary>
        /// Upgrade player gun
        /// </summary>
        /// <param name="callback"></param>
        public void UpgradeGun(System.Action<bool> callback)
        {

            int nextGunLvl = Shop.GetNextGunLevel(m_ShooterData.Amor.GunLevel);

            if (nextGunLvl == -1)
                return;

            int upgradeCost = Shop.GetGunUpgradeCost(nextGunLvl) ;
            if (m_ShooterData.CoinsInHand >= upgradeCost)
            {
                m_ShooterData.CoinsInHand -= upgradeCost;
                m_ShooterData.Amor.GunLevel = nextGunLvl;
                SaveData();
                callback.Invoke(true);
            }
            
        }

        /// <summary>
        /// Upgrade Rocket Launcher / Magazine
        /// </summary>
        /// <param name="callback"></param>
        public void UpgradeMagazine(System.Action<bool> callback)
        {
            int nextMagLevel = Shop.GetNextMissileMagLvl(m_ShooterData.Amor.MissileMagazineLvl);

            if (nextMagLevel == -1)
                return;

            int upgradeCost = Shop.GetMissileMagUpgrdCost(nextMagLevel);
            if (m_ShooterData.CoinsInHand >= upgradeCost)
            {
                m_ShooterData.CoinsInHand -= upgradeCost;
                m_ShooterData.Amor.MissileMagazineLvl = nextMagLevel;
                m_ShooterData.Amor.MagazineCapacity = Shop.GetMagazineCapacityByLvl(nextMagLevel);
                SaveData();
                callback.Invoke(true);
            }
        }

        /// <summary>
        /// Buy Missile using coins
        /// </summary>
        /// <param name="callback"></param>
        public void BuyMissile(System.Action<bool> callback)
        {
            int mcost = Shop.GetMissileCost();
            if (m_ShooterData.CoinsInHand >= mcost)
            {
                m_ShooterData.CoinsInHand -= mcost;
                m_ShooterData.Amor.MissileCount += 1;
                SaveData();
                callback.Invoke(true);
            }
        }

        /// <summary>
        /// Buy shield using coins
        /// </summary>
        /// <param name="callback"></param>
        public void BuyShield(System.Action<bool> callback)
        {
            int currentLvl = Shop.GetCurrentShieldLevel(m_ShooterData.CurrentShield.Duration);
            Shield shield = Shop.GetNextShieldDataByLvl(currentLvl+1);

            if(shield.Cost > -1)
            {
                if (m_ShooterData.CoinsInHand >= shield.Cost)
                {
                    m_ShooterData.CoinsInHand -= shield.Cost;
                    m_ShooterData.CurrentShield = shield;
                    SaveData();
                    callback.Invoke(true);
                }
            }
            
        }

        /// <summary>
        /// Buy life using coins
        /// </summary>
        /// <param name="LifeCost"></param>
        /// <param name="callback"></param>
        public void BuyLife(int LifeCost, System.Action<bool> callback)
        {
            if (m_Session.Lifes == 3)
                return;

            if (m_ShooterData.CoinsInHand >= LifeCost)
            {
                m_ShooterData.CoinsInHand -= LifeCost;
                m_Session.Lifes += 1;
                SaveData();
                callback.Invoke(true);
            }
        }

        /// <summary>
        /// Keep track of missiles in hand
        /// </summary>
        /// <returns></returns>
        public bool OnUseOneMissile()
        {
            if (m_ShooterData.Amor.MissileCount > 0)
            {
                m_ShooterData.Amor.MissileCount -= 1;
                UpdateHUD();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check its a record or not
        /// </summary>
        /// <param name="newScore"></param>
        /// <returns></returns>
        public bool IsNewRecord(int newScore)
        {
            if(m_ShooterData.m_HighScoreTable.Count < 6)
            {
                return true;
            }

            if (m_ShooterData.m_HighScoreTable[m_ShooterData.m_HighScoreTable.Count - 1].Score < newScore)
            {
                return true;
            }
            return false;

        }

        /// <summary>
        /// Add new entry in to high score table
        /// </summary>
        /// <param name="score"></param>
        /// <param name="name"></param>
        public void UpdateHiScoreTable(int score, string name)
        {
            m_ShooterData.m_HighScoreTable.Add(new HighScore(score, name));
            // sort and create new list
            List<HighScore> newTable = m_ShooterData.m_HighScoreTable.OrderByDescending(i => i.Score).Take(5).ToList();
            m_ShooterData.m_HighScoreTable = newTable;

            SaveData();
        }
    }
}


