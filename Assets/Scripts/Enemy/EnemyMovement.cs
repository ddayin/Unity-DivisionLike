/*
MIT License

Copyright (c) 2020 ddayin

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
    /// 적 캐릭터의 이동
    /// </summary>
    public class EnemyMovement : MonoBehaviour
    {
        private Transform m_Player;               // Reference to the player's position.
        private PlayerHealth m_PlayerHealth;      // Reference to the player's health.
        private EnemyHealth m_EnemyHealth;        // Reference to this enemy's health.
        private UnityEngine.AI.NavMeshAgent m_Nav;               // Reference to the nav mesh agent.
        private float m_NavTimer = 0f;

        void Awake()
        {
            // Set up the references.
            m_Player = GameObject.FindGameObjectWithTag( "Player" ).transform;
            m_PlayerHealth = m_Player.GetComponent<PlayerHealth>();
            m_EnemyHealth = transform.GetComponent<EnemyHealth>();
            m_Nav = transform.GetComponent<UnityEngine.AI.NavMeshAgent>();

            
        }

        private void OnEnable()
        {
            m_Nav.SetDestination( m_Player.position );
        }


        void Update()
        {
            // If the enemy and the player have health left...
            if ( m_EnemyHealth.m_CurrentHealth > 0 && Player.instance.m_Stats.m_CurrentHealth > 0 )
            {
                m_NavTimer += Time.deltaTime;

                if ( m_NavTimer > 0.3f )
                {
                    m_NavTimer = 0f;

                    // ... set the destination of the nav mesh agent to the player.
                    m_Nav.SetDestination( m_Player.position );
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