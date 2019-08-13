using UnityEngine;


namespace praveen.One
{
    public class PauseWindow : MonoBehaviour
    {
        public void OnAnimationComplete()
        {
            Time.timeScale = 0;
        }
    }
}

