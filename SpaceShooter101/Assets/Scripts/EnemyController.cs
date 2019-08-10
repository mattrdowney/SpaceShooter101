using System.Collections;
using System.Collections.Generic;
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

        // Start is called before the first frame update
        void Start()
        {
            m_EnemyPool = new ObjectPool(m_Enemy, m_EnemyHolder, 30, false, null, CallDeactivate);
            StartCoroutine(SpawnEnemies());
        }

        IEnumerator SpawnEnemies()
        {
            for (int i = 0; i < 30; i++)
            {
                GameObject enemyShip = m_EnemyPool.Spawn();
                SetEnemyData(enemyShip);
                enemyShip.transform.position = GetSpawnPoint();
                yield return new WaitForSeconds(1.3f);
            }
            
        }

        void CallDeactivate(GameObject go)
        {
            number ++;
            if(number > 29)
            {
                // Shop Menu
            }
        }

        Vector3 GetSpawnPoint()
        {
            float y = GameManager.Instance.GetUpperScreenY() + 3f;
            float x = Random.Range(GameManager.Instance.GetUpperLeftScreenBoundry().x,
                GameManager.Instance.GetUpperRightScreenBoundry().x);
            return new Vector3(x,y,0);
        }


        void SetEnemyData(GameObject ship)
        {
            int enemyHP = GetEnemyHitPointByLevel(GameManager.Instance.GetLevel());
            Sprite shipSkin = m_EnemySkin[GameManager.Instance.GetLevel() % 10];
            ship.GetComponent<Enemy>().Init(enemyHP, shipSkin);
        }

        int GetEnemyHitPointByLevel(int level)
        {
            return (level * 2) - 1;
        }

        public static void RecycleEnemy(GameObject enemy)
        {
            m_EnemyPool.Recycle(enemy);
        }
    }
}


