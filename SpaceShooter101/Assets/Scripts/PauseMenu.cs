using UnityEngine;
using UnityEngine.SceneManagement;

namespace praveen.One
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] Animator m_Animator;

        public void OnClickPauseBtn()
        {
            m_Animator.SetTrigger("PauseIn");
        }

        public void OnClickHomeButton()
        {
            Time.timeScale = 1;
            GameManager.Instance.ForceGameOver();
        }

        public void OnClickReStartBtn()
        {
            Time.timeScale = 1;
            m_Animator.SetTrigger("PauseOut");
        }

    }
}


