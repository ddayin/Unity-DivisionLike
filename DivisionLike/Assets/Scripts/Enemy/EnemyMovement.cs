using UnityEngine;
using System.Collections;

namespace DivisionLike
{
    /// <summary>
    /// 적 캐릭터의 이동
    /// </summary>
    public class EnemyMovement : MonoBehaviour
    {
        private Transform m_Player; // Reference to the player's position.
        private PlayerHealth m_PlayerHealth; // Reference to the player's health.
        private EnemyHealth m_EnemyHealth; // Reference to this enemy's health.
        private UnityEngine.AI.NavMeshAgent m_Nav; // Reference to the nav mesh agent.
        private float m_NavTimer = 0f;

        void Awake()
        {
            // Set up the references.
            m_Player = GameObject.FindGameObjectWithTag("Player").transform;
            m_PlayerHealth = m_Player.GetComponent<PlayerHealth>();
            m_EnemyHealth = transform.GetComponent<EnemyHealth>();
            m_Nav = transform.GetComponent<UnityEngine.AI.NavMeshAgent>();
        }

        private void OnEnable()
        {
            m_Nav.SetDestination(m_Player.position);
        }


        void Update()
        {
            // If the enemy and the player have health left...
            if (m_EnemyHealth.m_CurrentHealth > 0 && Player.instance.m_Stats.m_CurrentHealth > 0)
            {
                m_NavTimer += Time.deltaTime;

                if (m_NavTimer > 0.3f)
                {
                    m_NavTimer = 0f;

                    // ... set the destination of the nav mesh agent to the player.
                    m_Nav.SetDestination(m_Player.position);
                }
            }
            // Otherwise...
            else
            {
                // ... disable the nav mesh agent.
                m_Nav.enabled = false;
            }
        }
    }
}