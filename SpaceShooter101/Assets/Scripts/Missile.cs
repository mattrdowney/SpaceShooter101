using UnityEngine;

// REVIEW (reusability): I prefer to make the namespace for the project itself. In this case it might be SpaceShooter or praveenOne.SpaceShooter
namespace praveen.One
{
    // REVIEW (correctness): You need [System.Serializable] on classes (and structs) whenever you use [SerializeField] ( https://docs.unity3d.com/Manual/script-Serialization.html )
    public class Missile : Bullet
    {
        // REVIEW (correctness): to my knowledge you cannot serialize GameObjects -- https://stackoverflow.com/a/36853733 and also the above documentation where they mention specific Unity types work but others don't
        // REVIEW (correctness): If you make this object public it will be exposed in the inspector and should do what you want it to (so you can link the Prefab).
        [SerializeField] GameObject m_ExplosionParticle;

        // REVIEW (correctness, framerate consistency): physics (including Transform movement) should generally go in FixedUpdate. -- a good link: https://www.youtube.com/watch?v=MfIsp28TYAQ
        // REVIEW (efficiency): distance checks should 1) (best option) use Colliders/Triggers 2) (if slow enough) use Coroutines (or maybe InvokeRepeating)
        public override void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 1, 0), 0.1f);
            // REVIEW (efficiency): "new Vector3(0, 1, 0)" operation causes work for garbage collector (something to be aware of) -- since the field is only read / not copied
            // REVIEW (readability): "new Vector3(0, 1, 0)" operation can be replaced with Vector3.up (sort of like the Flyweight Pattern) which is easier to understand and harder to make mistakes with.
            // REVIEW (efficiency): "Vector3.Distance" operation causes more work compared to "(transform.position - Vector3.up).sqrMagnitude" (something to be aware of), note: you would have to use 0.01f^2
            if (Vector3.Distance(transform.position, new Vector3(0, 1, 0)) < 0.1f)
            {
                // REVIEW (compliment): good job making a separate function to make the code more intuitive.
                InvokeExplosion();
            }
        }

        // REVIEW (compliment): using autogerated function signatures with the triple slash is a good habit (keep doing it).
        // REVIEW (readability): try to remove comments like these. I would write: "Mutator (other objects, self-destructor) -- Find nearby objects and apply damage to them, then self-destruct."
        // REVIEW (self-consistency): for Update you specify public explicitly, but here you specify private implicitly. While it's true you don't have to be explicit, I prefer to always specify one of public/private/protected with fields and methods.
        /// <summary>
        /// Trigger explosion
        /// </summary>
        void InvokeExplosion()
        {
            GameObject explosion = Instantiate(m_ExplosionParticle);
            // REVIEW (readability): probably a good idea to use "Instantiate(m_ExplosionParticle, this.transform.position, Quaternion.identity);" to save a line of code (it's more idiomatic Unity)
            explosion.transform.position = this.transform.position;
            // REVIEW (readability): like above, prefer "Vector2.right"
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(0f, 1f), 5);
            int i = 0;
            // REVIEW (readability): prefer a foreach loop when the collection has a fixed size and every element is visited.
            while (i < hitColliders.Length)
            {
                // REVIEW (efficiency): prefer the physics "Layer" system in Unity (tags are inefficient, and often involve writing extra, unnecessary code)
                if (hitColliders[i].gameObject.tag == "Enemy")
                {
                    // REVIEW (efficiency): instead of Sending a message (which is slow), most Unity programmers use hitColliders[i].GetComponent<HealthComponent>() and apply the damage if it is found.
                    // REVIEW (efficiency, advanced technique): if you find you are constantly using .GetComponent<HealthComponent>() you can create a cache that looks like Dictionary<Collider, HealthComponent>
                    hitColliders[i].SendMessage("MissileDamage");
                }
                // REVIEW (readability, extremely opinionated): I prefer "i += 1" instead of i++ or ++i
                i++;
            }

            // REVIEW (compliment): nice on using an Object Pooling, it's a very efficient way of doing things
            BulletController.RecycleMissile(gameObject);
        }
    }
}

