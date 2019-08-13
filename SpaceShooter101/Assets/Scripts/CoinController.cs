using praveen.One.util;
using UnityEngine;

namespace praveen.One
{
    public class CoinController : MonoBehaviour
    {
        #region StaticFields
        static ObjectPool m_CoinPool;
        #endregion

        #region SerializedFields
        [SerializeField] GameObject m_Coin;
        [SerializeField] Transform m_CoinHolder;
        #endregion

        void Awake()
        {
            m_CoinPool = new ObjectPool(m_Coin, m_CoinHolder, 10, false, null, null);
        }

        /// <summary>
        /// Get coin from object pool
        /// </summary>
        /// <returns></returns>
        public static GameObject GetCoin()
        {
            GameObject coin = m_CoinPool.Spawn();
            coin.transform.rotation = Quaternion.identity;
            coin.transform.localPosition = Vector3.zero;
            coin.SetActive(false);
            return coin;
        }

        /// <summary>
        /// put back into the object pool
        /// </summary>
        /// <param name="coin"></param>
        public static void Recycle(GameObject coin)
        {
            m_CoinPool.Recycle(coin);
        }

        /// <summary>
        /// Spawn coin after ship destroied
        /// </summary>
        /// <param name="position"> Ship Position </param>
        public static void SpawnCoin(Vector3 position)
        {
            GameObject coin = CoinController.GetCoin();
            coin.SetActive(true);
            coin.transform.parent = null;
            coin.transform.position = position;
            coin.GetComponent<Coin>().Init();
        }
    }
}


