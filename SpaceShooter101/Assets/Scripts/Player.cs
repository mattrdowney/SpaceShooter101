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

        private float m_TimeSinceLastHit;
        private bool m_CanActivateShield;

        void Start()
        {
            m_CanActivateShield = false;
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
            if(collision.gameObject.tag == "EnemyBullet" || collision.gameObject.tag == "Enemy")
            {

                m_TimeSinceLastHit = 0;
                m_CanActivateShield = false;
                //GameManager.Instance.OnPlayerHit();

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

            m_TimeSinceLastHit += Time.deltaTime;
            //if (GameManager.Instance.GetShieldActTime() < m_TimeSinceLastHit)
            //{
            //    HudController.Instance.SetShieldActiveProgress(m_TimeSinceLastHit);
            //    m_CanActivateShield = true;
            //}

        }

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

        private void LaunchMissile()
        {
            if (!m_IsMissileReloaded)
                return;

            m_IsMissileReloaded = false;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width/2, Screen.height/2));
            m_Gun.transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);

            GameObject missile = BulletController.Instance.GetMissile();

            missile.transform.SetParent(m_Gun);
            missile.transform.localPosition = Vector3.zero;
            missile.transform.localRotation = Quaternion.identity;
            missile.layer = 10;
            missile.SetActive(true);

            StartCoroutine(ReloadRocket());
        }


        IEnumerator ReloadRocket()
        {
            yield return new WaitForSeconds(1f);
            m_IsMissileReloaded = true;
        }

        IEnumerator ReloadGun()
        {
            yield return new WaitForSeconds(2f);
            m_IsGunReloaded = true;
        }

    }

}

