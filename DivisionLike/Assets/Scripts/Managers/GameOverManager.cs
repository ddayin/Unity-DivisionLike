using UnityEngine;

namespace DivisionLike
{
    /// <summary>
    /// 게임 오버 관리자
    /// </summary>
    public class GameOverManager : MonoBehaviour
    {
        public bool m_IsImortalMode = false;
        private Animator m_Animator; // Reference to the animator component.

        void Awake()
        {
            // Set up the reference.
            m_Animator = GetComponent<Animator>();
        }

        void Update()
        {
            if (m_IsImortalMode == true)
                return;

            // If the player has run out of health...
            if (Player.instance.m_Stats.m_CurrentHealth <= 0)
            {
                // ... tell the animator the game is over.
                m_Animator.SetTrigger("GameOver");
            }
        }
    }
}