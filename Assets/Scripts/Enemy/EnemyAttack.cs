/*
MIT License

Copyright (c) 2019 ddayin

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

/*
 * reference - https://www.assetstore.unity3d.com/kr/#!/content/40756
 */

using UnityEngine;
using System.Collections;

namespace DivisionLike
{
    /// <summary>
    /// 적 캐릭터의 공격
    /// </summary>
    public class EnemyAttack : MonoBehaviour
    {
        public float m_TimeBetweenAttacks = 0.5f;     // The time in seconds between each attack.
        public int m_AttackDamage = 10;               // The amount of health taken away per attack.


        private Animator m_Animator;                              // Reference to the animator component.
        private GameObject m_Player;                          // Reference to the player GameObject.
        private PlayerHealth m_PlayerHealth;                  // Reference to the player's health.
        private EnemyHealth m_EnemyHealth;                    // Reference to this enemy's health.
        private bool m_IsPlayerInRange;                         // Whether player is within the trigger collider and can be attacked.
        private float m_Timer;                                // Timer for counting up to the next attack.


        void Awake()
        {
            // Setting up the references.
            m_Player = GameObject.FindGameObjectWithTag( "Player" );
            m_PlayerHealth = m_Player.GetComponent<PlayerHealth>();
            m_EnemyHealth = transform.GetComponent<EnemyHealth>();
            m_Animator = transform.GetComponent<Animator>();
        }


        void OnTriggerEnter( Collider other )
        {
            // If the entering collider is the player...
            if ( other.gameObject == m_Player )
            {
                // ... the player is in range.
                m_IsPlayerInRange = true;
            }
        }


        void OnTriggerExit( Collider other )
        {
            // If the exiting collider is the player...
            if ( other.gameObject == m_Player )
            {
                // ... the player is no longer in range.
                m_IsPlayerInRange = false;
            }
        }


        void Update()
        {
            // Add the time since Update was last called to the timer.
            m_Timer += Time.deltaTime;

            // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
            if ( m_Timer >= m_TimeBetweenAttacks && m_IsPlayerInRange == true && m_EnemyHealth.m_CurrentHealth > 0 )
            {
                // ... attack.
                Attack();
            }

            // If the player has zero or less health...
            if ( Player.instance.m_Stats.m_CurrentHealth <= 0 )
            {
                // ... tell the animator the player is dead.
                m_Animator.SetTrigger( "PlayerDead" );
            }
        }

        /// <summary>
        /// 공격
        /// </summary>
        void Attack()
        {
            // Reset the timer.
            m_Timer = 0f;

            // If the player has health to lose...
            if ( Player.instance.m_Stats.m_CurrentHealth > 0 )
            {
                // ... damage the player.
                m_PlayerHealth.TakeDamage( transform.forward, m_AttackDamage );
            }
        }
    }
}