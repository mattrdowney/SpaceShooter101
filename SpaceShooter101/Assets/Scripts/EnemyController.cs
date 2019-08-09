using System.Collections;
using System.Collections.Generic;
using praveen.One.util;
using UnityEngine;

namespace praveen.One
{
    public class EnemyController : MonoBehaviour
    {

        [SerializeField] GameObject m_Enemy;
        [SerializeField] Transform m_EnemyHolder;

        #region StaticFields
        static ObjectPool m_EnemyPool;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            m_EnemyPool = new ObjectPool(m_Enemy,m_EnemyHolder, 30, false, null, null);
            StartCoroutine(SpawnEnemies());
        }

        IEnumerator SpawnEnemies()
        {
            for (int i = 0; i < 30; i++)
            {
                GameObject enemyShip = m_EnemyPool.Spawn();
                enemyShip.transform.position = GetSpawnPoint();
                yield return new WaitForSeconds(1);
            }
            
        }

        Vector3 GetSpawnPoint()
        {
            float y = GameManager.Instance.GetUpperScreenY() + 3f;
            float x = Random.Range(GameManager.Instance.GetUpperLeftScreenBoundry().x,
                GameManager.Instance.GetUpperRightScreenBoundry().x);
            return new Vector3(x,y,0);
        }

        public static void RecycleEnemy(GameObject enemy)
        {
            m_EnemyPool.Recycle(enemy);
        }
    }
}


