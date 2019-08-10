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

        public static GameObject GetCoin()
        {
            GameObject coin = m_CoinPool.Spawn();
            coin.transform.rotation = Quaternion.identity;
            coin.transform.localPosition = Vector3.zero;
            coin.SetActive(false);
            return coin;
        }

        public static void Recycle(GameObject coin)
        {
            m_CoinPool.Recycle(coin);
        }
    }
}


