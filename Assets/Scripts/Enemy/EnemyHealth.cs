/*
 * reference - https://www.assetstore.unity3d.com/kr/#!/content/40756
 */

using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    public class EnemyHealth : MonoBehaviour
    {
        public const int _startingHealth = 100;            // The amount of health the enemy starts the game with.
        public int _currentHealth;                   // The current health the enemy has.
        public float _sinkSpeed = 2.5f;              // The speed at which the enemy sinks through the floor when dead.
        public AudioClip _deathClip;                 // The sound to play when the enemy dies.

        private EnemyStats _stats;
        private EnemyUI _ui;

        private Animator _anim;                              // Reference to the animator.
        private AudioSource _enemyAudio;                     // Reference to the audio source.
        private ParticleSystem _hitParticles;                // Reference to the particle system that plays when the enemy is damaged.
        private CapsuleCollider _capsuleCollider;            // Reference to the capsule collider.

        private bool _isDead;                                // Whether the enemy is dead.
        private bool _isSinking;                             // Whether the enemy has started sinking through the floor.

        public cakeslice.Outline _outline;
        private Transform _transform;

        void Awake()
        {
            _transform = transform;

            // Setting up the references.
            _stats = GetComponent<EnemyStats>();
            _anim = GetComponent<Animator>();
            _enemyAudio = GetComponent<AudioSource>();
            _hitParticles = GetComponentInChildren<ParticleSystem>();
            _capsuleCollider = GetComponent<CapsuleCollider>();
            _ui = transform.Find( "EnemyCanvas" ).GetComponent<EnemyUI>();
            
            // Setting the current health when the enemy first spawns.
            _currentHealth = _startingHealth;

            
        }

        private void OnEnable()
        {
            _transform.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
            _outline.enabled = false;
        }


        void Update()
        {
            // If the enemy should be sinking...
            if ( _isSinking == true )
            {
                // ... move the enemy down by the sinkSpeed per second.
                _transform.Translate( -Vector3.up * _sinkSpeed * Time.deltaTime );
            }
        }


        public void TakeDamage( int amount, Vector3 hitPoint )
        {
            // If the enemy is dead...
            if ( _isDead == true )
                // ... no need to take damage so exit the function.
                return;

            _outline.enabled = true;
            Invoke( "DisableOutline", 1f );

            // Play the hurt sound effect.
            _enemyAudio.Play();

            // Reduce the current health by the amount of damage sustained.
            _currentHealth -= amount;

            _ui.SetHealthSlider( _currentHealth );

            _ui.CreateDamageText( amount.ToString() );

            // Set the position of the particle system to where the hit was sustained.
            _hitParticles.transform.position = hitPoint;

            // And play the particles.
            _hitParticles.Play();

            // If the current health is less than or equal to zero...
            if ( _currentHealth <= 0 )
            {
                // ... the enemy is dead.
                Death();
            }
        }

        private void DisableOutline()
        {
            _outline.enabled = false;
        }


        void Death()
        {
            // The enemy is dead.
            _isDead = true;

            // Turn the collider into a trigger so shots can pass through it.
            _capsuleCollider.isTrigger = true;

            // Tell the animator that the enemy is dead.
            _anim.SetTrigger( "Dead" );

            ScreenHUD.instance.CalculateExpSlider( _stats._xpWhenDie );

            EffectManager.instance.CreateParticle( 2, transform.position );

            // Change the audio clip of the audio source to the death clip and play it (this will stop the hurt clip playing).
            _enemyAudio.clip = _deathClip;
            _enemyAudio.Play();
        }


        public void StartSinking()
        {
            // Find and disable the Nav Mesh Agent.
            GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;

            // Find the rigidbody component and make it kinematic (since we use Translate to sink the enemy).
            GetComponent<Rigidbody>().isKinematic = true;

            // The enemy should no sink.
            _isSinking = true;

            // After 2 seconds destory the enemy.
            //Destroy( gameObject, 2f );
            Lean.LeanPool.Despawn( gameObject, 2f );

            Invoke( "PrepareRebirth", 2f );
        }

        // because we have object pooling system
        private void PrepareRebirth()
        {
            _isSinking = false;
            _transform.GetComponent<Rigidbody>().isKinematic = false;
            
            _isDead = false;
            _capsuleCollider.isTrigger = false;
            _currentHealth = _startingHealth;
        }
    }
}