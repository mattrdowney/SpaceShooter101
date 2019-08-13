using System.Collections;
using UnityEngine;

namespace praveen.One
{
    public class Player : SpaceShip
    {
        #region SerializedFields
        [SerializeField] float m_Speed = 100;
        [SerializeField] Transform m_Gun;
        #endregion

        bool m_IsGunReloaded;
        bool m_IsMissileReloaded;

        private Transform m_PlayerTransform;

        private bool m_IsShieldActive;

        #region DeligateStuff
        public delegate void PlayerEventHandler();

        public static event PlayerEventHandler OnPlayerHit;
        #endregion

        void Start()
        {
            m_IsGunReloaded = true;
            m_IsMissileReloaded = true;
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

        /// <summary>
        /// Check the boundries to move
        /// </summary>
        /// <param name="newPos"></param>
        void CheckBoundary(Vector3 newPos)
        {

            Vector3 camViewPoint = Camera.main.WorldToViewportPoint(newPos);

            //Apply limit
            camViewPoint.x = Mathf.Clamp(camViewPoint.x, 0f, 1f);
            camViewPoint.y = Mathf.Clamp(camViewPoint.y, 0.1f, 0.25f);

            //Convert to world point then apply result to the target object
            m_PlayerTransform.position = Camera.main.ViewportToWorldPoint(camViewPoint);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (m_IsShieldActive)
                return;

            if(collision.gameObject.tag == "EnemyBullet" || collision.gameObject.tag == "Enemy")
            {
                GameManager.Instance.OnPlayerHit(10);
                if (OnPlayerHit != null)
                {
                    OnPlayerHit();
                }

                if (collision.gameObject.tag == "EnemyBullet"){
                    BulletController.RecycleBullet(collision.gameObject);
                }
                
            }
            
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                Shoot();
            }

            if (Input.GetMouseButton(1))
            {
                LaunchMissile();
            }
        }

        /// <summary>
        /// Enable / Disable shield
        /// </summary>
        /// <param name="state"></param>
        public void ShieldSetActive(bool state)
        {
            m_IsShieldActive = state;
        }

        /// <summary>
        /// Player shoot
        /// </summary>
        public override void Shoot()
        {
            if (!m_IsGunReloaded)
                return;

            m_IsGunReloaded = false;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            m_Gun.transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);

            GameObject bullet = BulletController.Instance.GetBullet(BulletTypes.player);
            bullet.transform.SetParent(m_Gun);
            bullet.transform.localPosition = Vector3.zero;
            bullet.transform.localRotation = Quaternion.identity;
            bullet.layer = 10;
            bullet.tag = "PlayerBullet";
            bullet.SetActive(true);

            StartCoroutine(ReloadGun());
        }

        /// <summary>
        /// Player Launches a missile
        /// </summary>
        private void LaunchMissile()
        {
            if (!m_IsMissileReloaded)
                return;

            m_IsMissileReloaded = false;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width/2, Screen.height/2));
            m_Gun.transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);

            GameObject missile = BulletController.Instance.GetMissile();

            if (missile == null)
                return;

            missile.transform.SetParent(m_Gun);
            missile.transform.localPosition = Vector3.zero;
            missile.transform.localRotation = Quaternion.identity;
            missile.layer = 10;
            missile.SetActive(true);

            StartCoroutine(ReloadRocket());
        }

        /// <summary>
        /// Refrest gun - Rocket launcher
        /// </summary>
        /// <returns></returns>
        IEnumerator ReloadRocket()
        {
            yield return new WaitForSeconds(1f);
            m_IsMissileReloaded = true;
        }

        /// <summary>
        /// Refresh normal gun
        /// </summary>
        /// <returns></returns>
        IEnumerator ReloadGun()
        {
            yield return new WaitForSeconds(2f);
            m_IsGunReloaded = true;
        }

    }

}

