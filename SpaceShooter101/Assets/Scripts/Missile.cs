using UnityEngine;

namespace praveen.One
{
    public class Missile : Bullet
    {
        [SerializeField] GameObject m_ExplosionParticle;

        public override void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 1, 0), 0.1f);
            if (Vector3.Distance(transform.position, new Vector3(0, 1, 0)) < 0.1f)
            {
                InvokeExplosion();
            }

        }

        /// <summary>
        /// Trigger explosion
        /// </summary>
        void InvokeExplosion()
        {
            GameObject explosion = Instantiate(m_ExplosionParticle);
            explosion.transform.position = this.transform.position;
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(0f, 1f), 5);
            int i = 0;
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].gameObject.tag == "Enemy")
                {
                    hitColliders[i].SendMessage("MissileDamage");
                }
                i++;
            }

            BulletController.RecycleMissile(gameObject);
        }
    }
}

