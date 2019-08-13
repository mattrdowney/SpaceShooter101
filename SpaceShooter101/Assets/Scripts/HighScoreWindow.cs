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

        public void PopUp()
        {
            m_Anim.SetTrigger("PopUp");
        }

        public void OnClickOKBtn()
        {
            if(m_NameInput.text.Length > 0)
            {
                GameManager.Instance.UpdateHiScoreTable(GameManager.Instance.GetCurrentSession().Score, m_NameInput.text);
                m_Anim.SetTrigger("OnClose");
            }
        }

        public void OnClickCancelBtn()
        {
            m_Anim.SetTrigger("OnClose");
        }
    }
}


