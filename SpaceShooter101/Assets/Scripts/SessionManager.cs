using UnityEngine;

namespace praveen.One
{
    public class SessionManager : MonoBehaviour
    {
        #region singleton stuff
        private static SessionManager m_Instance;

        public static SessionManager Instance
        {
            get { return m_Instance; }
        }
        #endregion


        #region SerializedFields
        [SerializeField] GameObject m_Player;
        [SerializeField] GameObject m_ScriptRef;
        [SerializeField] GameObject m_OpemomgCountdown;
        #endregion

        #region PrivateFields
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
        }

        private void Start()
        {
            GameManager.Instance.StartNewGameOrContinue();
        }

        /// <summary>
        /// starts a session
        /// </summary>
        public void StartSession()
        {
            m_OpemomgCountdown.SetActive(true);
        }

        /// <summary>
        /// starts a game
        /// </summary>
        public void StartGame()
        {
            m_Player.gameObject.SetActive(true);
            m_ScriptRef.gameObject.SetActive(true);
            //m_OpemomgCountdown.SetActive(false);
        }

    }

}


