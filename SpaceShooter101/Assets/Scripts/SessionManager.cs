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


        #region MetaData
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

        public void StartSession()
        {

        }

    }

}


