using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    /// <summary>
    /// 적 캐릭터의 HP
    /// </summary>
    public class EnemyHealth : MonoBehaviour
    {
        public const int m_StartingHealth = 100; // The amount of health the enemy starts the game with.
        public int m_CurrentHealth; // The current health the enemy has.
        public float m_SinkSpeed = 2.5f; // The speed at which the enemy sinks through the floor when dead.
        public AudioClip m_DeathClip; // The sound to play when the enemy dies.

        private EnemyStats m_Stats;
        private EnemyUI m_ui;

        private Animator m_Animator; // Reference to the animator.
        private AudioSource m_EnemyAudio; // Reference to the audio source.
        private ParticleSystem m_HitParticles; // Reference to the particle system that plays when the enemy is damaged.
        private CapsuleCollider m_CapsuleCollider; // Reference to the capsule collider.

        private bool m_IsDead; // Whether the enemy is dead.
        private bool m_IsSinking; // Whether the enemy has started sinking through the floor.

        public cakeslice.Outline m_Outline;
        private Transform m_Transform;

        void Awake()
        {
            m_Transform = transform;

            // Setting up the references.
            m_Stats = GetComponent<EnemyStats>();
            m_Animator = GetComponent<Animator>();
            m_EnemyAudio = GetComponent<AudioSource>();
            m_HitParticles = GetComponentInChildren<ParticleSystem>();
            m_CapsuleCollider = GetComponent<CapsuleCollider>();
            m_ui = transform.Find("EnemyCanvas").GetComponent<EnemyUI>();

            // Setting the current health when the enemy first spawns.
            m_CurrentHealth = m_StartingHealth;
        }

        private void OnEnable()
        {
            m_Transform.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
            m_Outline.enabled = false;
        }


        void Update()
        {
            // If the enemy should be sinking...
            if (m_IsSinking == true)
            {
                // ... move the enemy down by the sinkSpeed per second.
                m_Transform.Translate(-Vector3.up * m_SinkSpeed * Time.deltaTime);
            }
        }

        /// <summary>
        /// 일정량의 데미지를 받는다.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="hitPoint"></param>
        public void TakeDamage(int amount, Vector3 hitPoint)
        {
            // If the enemy is dead...
            if (m_IsDead == true)
                // ... no need to take damage so exit the function.
                return;

            m_Outline.enabled = true;
            Invoke("DisableOutline", 1f);

            // PlayHDRP the hurt sound effect.
            m_EnemyAudio.Play();

            // Reduce the current health by the amount of damage sustained.
            m_CurrentHealth -= amount;

            m_ui.SetHealthSlider(m_CurrentHealth);

            m_ui.CreateDamageText(amount.ToString());

            // Set the position of the particle system to where the hit was sustained.
            m_HitParticles.transform.position = hitPoint;

            // And play the particles.
            m_HitParticles.Play();

            // If the current health is less than or equal to zero...
            if (m_CurrentHealth <= 0)
            {
                // ... the enemy is dead.
                Death();
            }
        }

        /// <summary>
        /// 아웃라인을 비활성화 시킨다.
        /// </summary>
        private void DisableOutline()
        {
            m_Outline.enabled = false;
        }

        /// <summary>
        /// 죽는다.
        /// </summary>
        void Death()
        {
            // The enemy is dead.
            m_IsDead = true;

            // Turn the collider into a trigger so shots can pass through it.
            m_CapsuleCollider.isTrigger = true;

            // Tell the animator that the enemy is dead.
            m_Animator.SetTrigger("Dead");

            ScreenHUD.instance.CalculateExpSlider(m_Stats.m_XpWhenDie);

            EffectManager.instance.CreateParticle(2, transform.position);

            // Change the audio clip of the audio source to the death clip and play it (this will stop the hurt clip playing).
            m_EnemyAudio.clip = m_DeathClip;
            m_EnemyAudio.Play();
        }

        /// <summary>
        /// 아래로 가라앉는다.
        /// </summary>
        public void StartSinking()
        {
            // Find and disable the Nav Mesh Agent.
            GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;

            // Find the rigidbody component and make it kinematic (since we use Translate to sink the enemy).
            GetComponent<Rigidbody>().isKinematic = true;

            // The enemy should no sink.
            m_IsSinking = true;

            // After 2 seconds destory the enemy.
            //Destroy( gameObject, 2f );
            Lean.LeanPool.Despawn(gameObject, 2f);

            Invoke("PrepareRebirth", 2f);
        }


        /// <summary>
        /// because we have object pooling system 
        /// </summary>
        private void PrepareRebirth()
        {
            m_IsSinking = false;
            m_Transform.GetComponent<Rigidbody>().isKinematic = false;

            m_IsDead = false;
            m_CapsuleCollider.isTrigger = false;
            m_CurrentHealth = m_StartingHealth;
        }
    }
}