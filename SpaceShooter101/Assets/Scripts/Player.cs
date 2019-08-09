using UnityEngine;

namespace praveen.One
{
    public class Player : MonoBehaviour
    {
        [SerializeField] float m_Speed = 100;
        private Transform m_PlayerTransform;

        void Start()
        {
            m_PlayerTransform = this.transform;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");


            if ((h > 0 || v > 0) || (h < 0 || v < 0))
            {
                Vector3 tempVect = new Vector3(h, v, 0);
                tempVect = tempVect.normalized * m_Speed * Time.deltaTime;


                Vector3 newPos = m_PlayerTransform.transform.position + tempVect;
                CheckBoundary(newPos);
            }
        }

        void CheckBoundary(Vector3 newPos)
        {

            Vector3 camViewPoint = Camera.main.WorldToViewportPoint(newPos);

            //Apply limit
            camViewPoint.x = Mathf.Clamp(camViewPoint.x, 0f, 1f);
            camViewPoint.y = Mathf.Clamp(camViewPoint.y, 0f, 0.24f);

            //Convert to world point then apply result to the target object
            m_PlayerTransform.position = Camera.main.ViewportToWorldPoint(camViewPoint);
        }
    }

}

