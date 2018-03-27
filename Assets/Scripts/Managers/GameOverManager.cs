/*
 * reference - https://www.assetstore.unity3d.com/kr/#!/content/40756
 */

using UnityEngine;

namespace DivisionLike
{
    public class GameOverManager : MonoBehaviour
    {
        public bool m_IsImortalMode = false;
        private Animator m_Animator;                          // Reference to the animator component.

        void Awake()
        {
            // Set up the reference.
            m_Animator = GetComponent<Animator>();
        }

        void Update()
        {
            if ( m_IsImortalMode == true )
                return;

            // If the player has run out of health...
            if ( Player.instance.m_Stats.m_CurrentHealth <= 0 )
            {
                // ... tell the animator the game is over.
                m_Animator.SetTrigger( "GameOver" );
            }
        }
    }
}