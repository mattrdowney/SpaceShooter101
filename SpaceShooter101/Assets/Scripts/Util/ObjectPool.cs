using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace praveen.One.util
{
    // Delegates for activation/deactivation
    public delegate void ActivateDelegate(GameObject gObject);
    public delegate void DeactivateDelegate(GameObject gObject);

    public class ObjectPool
    {
        #region private attributes
        private GameObject m_Prefab;
        private List<GameObject> m_PoolAvailable;
        private List<GameObject> m_PoolActive;
        private bool m_SetActiveRecursively;
        ActivateDelegate OnActivate = null;
        DeactivateDelegate OnDeactivate = null;
        #endregion

        #region public properties
        public int PoolSize
        {
            get { return m_PoolAvailable.Count; }
        }

        public int ActiveCount
        {
            get { return m_PoolActive.Count; }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Util.ObjectPool"/> class.
        /// </summary>
        /// <param name='prefab'>
        /// GameObject Prefab to instantiate objects.
        /// </param>
        /// <param name='initialSize'>
        /// Initial size of the pool.
        /// </param>
        /// <param name='setActiveRecursively'>
        /// to call SetActiveRecursively or .active
        /// </param>
        /// <param name='onActivate'>
        /// Delegate to call when object is spawned
        /// </param>
        /// <param name='onDeactivate'>
        /// Delegate to call when object is recycled
        /// </param>
        public ObjectPool(GameObject prefab, Transform parent, int initialSize, bool setActiveRecursively, ActivateDelegate onActivate = null, DeactivateDelegate onDeactivate = null)
        {
            m_Prefab = prefab;
            m_SetActiveRecursively = setActiveRecursively;
            OnActivate = onActivate;
            OnDeactivate = onDeactivate;
            m_PoolActive = new List<GameObject>();
            m_PoolAvailable = new List<GameObject>();

            for (int i = 0; i < initialSize; ++i)
            {
                GameObject gameObject = GameObject.Instantiate(m_Prefab) as GameObject;
                m_PoolAvailable.Add(gameObject);
                gameObject.transform.SetParent(parent);
                SetActive(gameObject, false);
            }

        }

        /// <summary>
        /// Activate an existing object or create a new one
        /// </summary>
        public GameObject Spawn()
        {
            GameObject gameObject = null;

            if (m_PoolAvailable.Count > 0)
            {
                foreach (GameObject go in m_PoolAvailable)
                {
                    if (!go.active)
                    {
                        gameObject = go;
                        break;
                    }
                }
            }

            // Create a new object if none is available
            if (gameObject == null)
            {
                gameObject = GameObject.Instantiate(m_Prefab) as GameObject;
                m_PoolAvailable.Add(gameObject);
            }

            m_PoolActive.Add(gameObject);
            SetActive(gameObject, true);

            // call after activity true
            if (OnActivate != null)
            {
                OnActivate(gameObject);
            }

            return gameObject;
        }

        /// <summary>
        /// Deactivate object after it's usability is complete
        /// </summary>
        /// <param name='gameObject'>
        /// GameObject to recycle
        /// </param>
        public void Recycle(GameObject gameObject)
        {
            int index = m_PoolActive.IndexOf(gameObject);
            if (index >= 0)
            {
                m_PoolActive.RemoveAt(index);

                // call before activity false
                if (OnDeactivate != null)
                {
                    OnDeactivate(gameObject);
                }

                SetActive(gameObject, false);
            }
            else
            {
                Debug.LogWarning("ObjectPool.Recycle() Active list does not contain object: " + gameObject.name);
            }
        }

        /// <summary>
        /// Recycle all elements in the pool
        /// </summary>
        public void RecycleAll()
        {
            foreach (GameObject gameObject in m_PoolActive)
            {
                if (OnDeactivate != null)
                {
                    OnDeactivate(gameObject);
                }

                SetActive(gameObject, false);
            }

            m_PoolActive.Clear();
        }

        /// <summary>
        /// Get list of active GameObjects
        /// </summary>
        /// <returns>
        /// The active list.
        /// </returns>
        public List<GameObject> GetActiveList()
        {
            return m_PoolActive;
        }

        /// <summary>
        /// Change OnActivate method
        /// </summary>
        /// <param name='_onActivate'>
        /// delegate method
        /// </param>
        public void SetOnActivateMethod(ActivateDelegate _onActivate)
        {
            OnActivate = _onActivate;
        }

        /// <summary>
        /// Change OnDeactivate method
        /// </summary>
        /// <param name='_onDeactivate'>
        /// delegate method
        /// </param>
        public void SetOnDeactivateMethod(DeactivateDelegate _onDeactivate)
        {
            OnDeactivate = _onDeactivate;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        private void SetActive(GameObject gameObject, bool isActive)
        {
            if (m_SetActiveRecursively)
            {
                gameObject.SetActiveRecursively(isActive);
            }
            else
            {
                gameObject.active = isActive;
            }
        }

    }

}