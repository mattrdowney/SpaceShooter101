using UnityEngine;


namespace praveen.One
{
    public class PauseWindow : MonoBehaviour
    {
        /// <summary>
        /// Triggers when animation complete
        /// </summary>
        public void OnAnimationComplete()
        {
            Time.timeScale = 0;
        }
    }
}

