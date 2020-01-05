using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace praveen.One
{
    // REVIEW (architecture): unless you're planning to have multiple players, it's probably better to just add a static respawn function to the Player class in my opinion.
    public class PlayerController : MonoBehaviour
    {
        #region singleton stuff
        private static PlayerController m_Instance;

        public static PlayerController Instance
        {
            get { return m_Instance; }
        }
        #endregion

        #region serialized fields
        [SerializeField] GameObject m_Player;
        #endregion

        private void Awake()
        {
            m_Instance = this;
        }

        public void SpawnPlayer()
        {
            GameObject go = Instantiate(m_Player);
            go.transform.position = new Vector3(0, -2.5f, 0);

        }

    }
}


