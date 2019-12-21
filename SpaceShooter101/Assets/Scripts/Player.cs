using System.Collections;
using UnityEngine;

namespace praveen.One
{
    public class Player : SpaceShip
    {
        #region SerializedFields
        [SerializeField] float m_Speed = 100;
        [SerializeField] Transform m_Gun;
        [SerializeField] GameObject m_ExplosionParticle;
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
            StartCoroutine(ActivateInitialShield());
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            // REVIEW (readability): try to use horizontal and vertical here so it's easier to read.
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
        /// Activate the shield when player spawn
        /// </summary>
        /// <returns></returns>
        IEnumerator ActivateInitialShield()
        {
            ShieldSetActive(true);
            // REVIEW (readability, efficiency): where possible, you can create a variable "WaitForSeconds shield_duration = new WaitForSeconds(3);" in the class fields, that way you make the code more readable/expressive (and save the garbage collector from extra work).
            yield return new WaitForSeconds(3);
            ShieldSetActive(false);
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

        // REVIEW (compliment): good job on using Update for player inputs (that's super important so you don't miss player inputs).
        private void Update()
        {
            // REVIEW (readability): it's probably better to use the Unity idiom Input.GetButton(), even if the user would never reconfigure the control scheme.
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

            // REVIEW (compliment): good job with object pooling
            GameObject bullet = BulletController.Instance.GetBullet(BulletOwner.player);
            // REVIEW (correctness): generally bullets are not parented to the gun they were fired from (since they exist far away).
            bullet.transform.SetParent(m_Gun);
            // REVIEW (readability, efficiency): you should be able to set these in the object pool instead (most do not need to be reset).
            bullet.transform.localPosition = Vector3.zero;
            bullet.transform.localRotation = Quaternion.identity;
            bullet.layer = 10;
            bullet.tag = "PlayerBullet";

            int currentGunLvl = GameManager.Instance.GetCurrentAmorData().GunLevel;
            int bulletDamage = DataBank.GetGunPowerDamageByLevel(currentGunLvl);

            // REVIEW (compliment): good job on using .GetComponent<>(), which is generally better than .SendMessage()
            bullet.GetComponent<Bullet>().Program(10, bulletDamage, BulletOwner.player);
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


        public override void Damage(int damage)
        {
            if (m_IsShieldActive)
                return;

            GameManager.Instance.OnPlayerHit(damage, this);
            if (OnPlayerHit != null)
            {
                OnPlayerHit();
            }
        }

        public override void Destroy()
        {
            GameObject go = Instantiate(m_ExplosionParticle);
            go.transform.position = gameObject.transform.position;
            Destroy(gameObject);
        }

    }

}

