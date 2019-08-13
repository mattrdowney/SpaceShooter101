using UnityEngine;
using UnityEngine.UI;

namespace praveen.One
{
    public class HighScoreWindow : MonoBehaviour
    {
        Animator m_Anim;
        [SerializeField] InputField m_NameInput;

        private void Start()
        {
            m_Anim = GetComponent<Animator>();
        }

        /// <summary>
        /// Open the window
        /// </summary>
        public void PopUp()
        {
            m_Anim.SetTrigger("PopUp");
        }

        /// <summary>
        /// Click OK and save the name
        /// </summary>
        public void OnClickOKBtn()
        {
            if(m_NameInput.text.Length > 0)
            {
                GameManager.Instance.UpdateHiScoreTable(GameManager.Instance.GetCurrentSession().Score, m_NameInput.text);
                m_Anim.SetTrigger("OnClose");
            }
        }


        /// <summary>
        /// On click Cancel button
        /// </summary>
        public void OnClickCancelBtn()
        {
            m_Anim.SetTrigger("OnClose");
        }
    }
}


