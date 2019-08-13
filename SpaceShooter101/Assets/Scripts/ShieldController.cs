using UnityEngine;
using UnityEngine.UI;

namespace praveen.One
{
    public class ShieldController : MonoBehaviour
    {

        private float m_TimeSinceLastHit;
        private float m_ShiledTime;

        [SerializeField] Button m_ShieldButton;
        [SerializeField] Player m_Player;

        Shield m_CurrentShield;
        bool m_OnShiledActive;


        void Start()
        {
            Player.OnPlayerHit += OnPlayerHit;
            m_CurrentShield = GameManager.Instance.GetCurrentShield();
            m_ShiledTime = m_CurrentShield.Duration;
        }

        /// <summary>
        /// Triggers when player hit
        /// </summary>
        void OnPlayerHit()
        {
            if (m_OnShiledActive)
                return;

            m_TimeSinceLastHit = 0f;
        }


        // Update is called once per frame
        void Update()
        {
            if (m_OnShiledActive)
            {
                m_ShieldButton.interactable = false;

                m_ShiledTime -= Time.deltaTime;

                if (m_ShiledTime < 0)
                {
                    m_OnShiledActive = false;
                    m_Player.ShieldSetActive(m_OnShiledActive);
                }

                HudController.Instance.SetShieldActiveProgress(m_ShiledTime / m_CurrentShield.Duration);
            }
            else
            {
                m_TimeSinceLastHit += Time.deltaTime;

                HudController.Instance.SetShieldActiveProgress(m_TimeSinceLastHit/10);

                if (m_TimeSinceLastHit > 10)
                {
                    // enable shiled button
                    m_ShieldButton.interactable = true;
                }
                else
                {
                    // disable shield button
                    m_ShieldButton.interactable = false;
                }
            }

            
        }

        /// <summary>
        /// Enabled the available shield
        /// </summary>
        public void OnEnableShield()
        {
            m_ShiledTime = m_CurrentShield.Duration;
            m_OnShiledActive = true;
            m_Player.ShieldSetActive(m_OnShiledActive);
        }


    }
}
