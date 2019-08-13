using System.Collections;
using praveen.One.util;
using UnityEngine;

namespace praveen.One
{
    public class EnemyController : MonoBehaviour
    {
        #region SerializedFields
        [SerializeField] GameObject m_Enemy;
        [SerializeField] Transform m_EnemyHolder;
        [SerializeField] Sprite[] m_EnemySkin;
        #endregion


        #region StaticFields
        static ObjectPool m_EnemyPool;
        #endregion


        int number = 0;

        void Start()
        {
            m_EnemyPool = new ObjectPool(m_Enemy, m_EnemyHolder, 30, false, null, CallDeactivate);
            StartCoroutine(SpawnEnemies());
        }

        /// <summary>
        /// Spawn enemies
        /// </summary>
        /// <returns></returns>
        IEnumerator SpawnEnemies()
        {
            for (int i = 0; i < 30; i++)
            {
                GameObject enemyShip = m_EnemyPool.Spawn();
                SetEnemyData(enemyShip);
                enemyShip.transform.position = GetSpawnPoint();
                yield return new WaitForSeconds(1.8f);
            }
            
        }

        void CallDeactivate(GameObject go)
        {
            number ++;
            if(number > 29)
            {
                //GameManager.Instance.LoadShopScene();
            }
        }

        /// <summary>
        /// Get spawn point of the screen
        /// </summary>
        /// <returns></returns>
        Vector3 GetSpawnPoint()
        {
            float y = GameManager.Instance.GetUpperScreenY() + 3f;
            float x = Random.Range(GameManager.Instance.GetUpperLeftScreenBoundry().x,
                GameManager.Instance.GetUpperRightScreenBoundry().x);
            return new Vector3(x,y,0);
        }

        /// <summary>
        /// Initialize enemy
        /// </summary>
        /// <param name="ship"></param>
        void SetEnemyData(GameObject ship)
        {
            int enemyHP = GetEnemyHitPointByLevel(GameManager.Instance.GetLevel());
            Sprite shipSkin = m_EnemySkin[GameManager.Instance.GetLevel() % 10];
            ship.GetComponent<Enemy>().Init(enemyHP, shipSkin);
        }

        /// <summary>
        /// Enemy hitpoint level formula
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        int GetEnemyHitPointByLevel(int level)
        {
            return (level * 2) - 1;
        }


        /// <summary>
        /// Enemy put back to the object pool
        /// </summary>
        /// <param name="enemy"></param>
        public static void RecycleEnemy(GameObject enemy)
        {
            m_EnemyPool.Recycle(enemy);
        }
    }
}


