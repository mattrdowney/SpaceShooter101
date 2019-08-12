using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace praveen.One
{
    public struct ShooterSession
    {
        public bool IsActive;
        public int Level;
        public int Score;
        public int Hp;
        public int EnemiesKilled;

        public ShooterSession(bool isActive, int level, int score, int hp, int enemiesKilled)
        {
            this.IsActive = isActive;
            this.Level = level;
            this.Score = score;
            this.Hp = hp;
            this.EnemiesKilled = enemiesKilled;
        }
    }

    [System.Serializable]
    public struct ShooterData
    {
        public int HighScore;
        public int CoinsInHand;
        public ShooterAmor Amor;
        public Shield CurrentShield;

        public ShooterData(int highScore, int coins, ShooterAmor amor, Shield shield)
        {
            this.HighScore = highScore;
            this.CoinsInHand = coins;
            this.Amor = amor;
            this.CurrentShield = shield;
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
        bool m_IsNewRecord;
        ShooterAmor m_ShooterAmor;
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

            PlayerPrefs.DeleteAll() ;

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
                ShooterData data    = new ShooterData(0, 0, amor, shield);

                m_ShooterData = data;
            }
            else
            {
                m_ShooterData = JsonUtility.FromJson<ShooterData>(shooterData);
            }

        }


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
                m_Session = new ShooterSession(true,1, 0 ,3, 0);
            }

            HudController.Instance.SetCoins(m_ShooterData.CoinsInHand);
            HudController.Instance.SetScore(m_Session.Score);
            HudController.Instance.EnemiesKilled(m_Session.EnemiesKilled);
            HudController.Instance.SetMissileData(m_ShooterData.Amor.MissileCount, m_ShooterData.Amor.MissileCount);

            SessionManager.Instance.StartSession();
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

        public void AddScore(int score)
        {
            m_Session.Score += score;
            HudController.Instance.SetScore(m_Session.Score);
        }

        public void UpdateEnemiesKilled()
        {
            m_Session.EnemiesKilled += 1;
            HudController.Instance.EnemiesKilled(m_Session.EnemiesKilled);
        }

        public void OnPlayerHit()
        {
            m_Session.Hp -= 1;
            HudController.Instance.SetPlayerHelth(m_Session.Hp);

            if(m_Session.Hp < 1)
            {
                GameOver();
            }
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

        //public float GetShieldTime()
        //{
        //    return m_ShieldTime;
        //}

        //public float GetShieldActTime()
        //{
        //    return m_ShieldActTime;
        //}

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
        public int GetHighScore()
        {
            return m_ShooterData.HighScore;
        }


        private void GameOver()
        {
           m_IsNewRecord = false;

           if(m_Session.Score > m_ShooterData.HighScore)
           {
                m_IsNewRecord           = true;
                m_ShooterData.HighScore = m_Session.Score;
           }

            m_Session.IsActive = false;

            SaveData();

            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);

        }

        public void LoadShopScene()
        {
            SceneManager.LoadScene("ShopMenu", LoadSceneMode.Single);
        }

        public GameOverUI GetGameOverUI()
        {
            return new GameOverUI(m_Session.Score, m_ShooterData.HighScore, m_ShooterData.CoinsInHand, m_IsNewRecord);
        }

        /// <summary>
        /// Returns equiped amour
        /// </summary>
        /// <returns></returns>
        public ShooterAmor GetCurrentAmorData()
        {
            return m_ShooterAmor;
        }


        /// <summary>
        /// Returns available coin count
        /// </summary>
        /// <returns></returns>
        public int GetCoinsInHand()
        {
            return m_ShooterData.CoinsInHand;
        }

        public void UpgradeGun(System.Action<bool> callback)
        {

            int nextGunLvl = Shop.GetNextGunLevel(m_ShooterAmor.GunLevel);

            if (nextGunLvl == -1)
                return;

            int upgradeCost = Shop.GetGunUpgradeCost(nextGunLvl) ;
            if (m_ShooterData.CoinsInHand >= upgradeCost)
            {
                m_ShooterData.CoinsInHand -= upgradeCost;
                m_ShooterAmor.GunLevel = nextGunLvl;
                SaveData();
                callback.Invoke(true);
            }
            
        }

        public void UpgradeMagazine(System.Action<bool> callback)
        {
            int nextMagLevel = Shop.GetNextMissileMagLvl(m_ShooterAmor.MissileMagazineLvl);

            if (nextMagLevel == -1)
                return;

            int upgradeCost = Shop.GetMissileMagUpgrdCost(nextMagLevel);
            if (m_ShooterData.CoinsInHand >= upgradeCost)
            {
                m_ShooterData.CoinsInHand -= upgradeCost;
                m_ShooterAmor.MissileMagazineLvl = nextMagLevel;
                SaveData();
                callback.Invoke(true);
            }
        }

        public void BuyMissile(System.Action<bool> callback)
        {
            int mcost = Shop.GetMissileCost();
            if (m_ShooterData.CoinsInHand >= mcost)
            {
                m_ShooterData.CoinsInHand -= mcost;
                m_ShooterAmor.MissileCount += 1;
                SaveData();
                callback.Invoke(true);
            }
        }


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
    }
}


