using UnityEngine;
using UnityEngine.SceneManagement;

namespace praveen.One
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] Animator m_Animator;

        /// <summary>
        /// Click Pause Button
        /// </summary>
        public void OnClickPauseBtn()
        {
            m_Animator.SetTrigger("PauseIn");
        }

        /// <summary>
        /// Click home button
        /// </summary>
        public void OnClickHomeButton()
        {
            Time.timeScale = 1;
            GameManager.Instance.ForceGameOver();
        }

        /// <summary>
        /// On Click restart button
        /// </summary>
        public void OnClickReStartBtn()
        {
            Time.timeScale = 1;
            m_Animator.SetTrigger("PauseOut");
        }

    }
}


