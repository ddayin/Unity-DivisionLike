/*
 * reference - https://www.assetstore.unity3d.com/kr/#!/content/40756
 */

using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    public class EnemyHealth : MonoBehaviour
    {
        public int startingHealth = 100;            // The amount of health the enemy starts the game with.
        public int currentHealth;                   // The current health the enemy has.
        public float sinkSpeed = 2.5f;              // The speed at which the enemy sinks through the floor when dead.
        public AudioClip deathClip;                 // The sound to play when the enemy dies.

        private EnemyStats stats;
        private EnemyUI ui;

        private Animator anim;                              // Reference to the animator.
        private AudioSource enemyAudio;                     // Reference to the audio source.
        private ParticleSystem hitParticles;                // Reference to the particle system that plays when the enemy is damaged.
        private CapsuleCollider capsuleCollider;            // Reference to the capsule collider.
        private bool isDead;                                // Whether the enemy is dead.
        private bool isSinking;                             // Whether the enemy has started sinking through the floor.
        
        

        void Awake()
        {
            // Setting up the references.
            stats = GetComponent<EnemyStats>();
            anim = GetComponent<Animator>();
            enemyAudio = GetComponent<AudioSource>();
            hitParticles = GetComponentInChildren<ParticleSystem>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            ui = transform.Find( "EnemyCanvas" ).GetComponent<EnemyUI>();
            
            // Setting the current health when the enemy first spawns.
            currentHealth = startingHealth;
        }


        void Update()
        {
            // If the enemy should be sinking...
            if ( isSinking )
            {
                // ... move the enemy down by the sinkSpeed per second.
                transform.Translate( -Vector3.up * sinkSpeed * Time.deltaTime );
            }
        }


        public void TakeDamage( int amount, Vector3 hitPoint )
        {
            // If the enemy is dead...
            if ( isDead )
                // ... no need to take damage so exit the function.
                return;

            // Play the hurt sound effect.
            enemyAudio.Play();

            // Reduce the current health by the amount of damage sustained.
            currentHealth -= amount;

            ui.SetHealthSlider( currentHealth );

            // Set the position of the particle system to where the hit was sustained.
            hitParticles.transform.position = hitPoint;

            // And play the particles.
            hitParticles.Play();

            // If the current health is less than or equal to zero...
            if ( currentHealth <= 0 )
            {
                // ... the enemy is dead.
                Death();
            }
        }


        void Death()
        {
            // The enemy is dead.
            isDead = true;

            // Turn the collider into a trigger so shots can pass through it.
            capsuleCollider.isTrigger = true;

            // Tell the animator that the enemy is dead.
            anim.SetTrigger( "Dead" );

            ScreenHUD.instance.CalculateExpSlider( stats.xpWhenDie );

            // Change the audio clip of the audio source to the death clip and play it (this will stop the hurt clip playing).
            enemyAudio.clip = deathClip;
            enemyAudio.Play();
        }


        public void StartSinking()
        {
            // Find and disable the Nav Mesh Agent.
            GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;

            // Find the rigidbody component and make it kinematic (since we use Translate to sink the enemy).
            GetComponent<Rigidbody>().isKinematic = true;

            // The enemy should no sink.
            isSinking = true;
            
            // After 2 seconds destory the enemy.
            Destroy( gameObject, 2f );
        }
    }
}