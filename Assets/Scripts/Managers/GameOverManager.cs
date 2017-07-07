/*
 * reference - https://www.assetstore.unity3d.com/kr/#!/content/40756
 */

using UnityEngine;

namespace DivisionLike
{
    public class GameOverManager : MonoBehaviour
    {
        public PlayerHealth playerHealth;       // Reference to the player's health.
        public bool isImortalMode = false;

        Animator anim;                          // Reference to the animator component.


        void Awake()
        {
            // Set up the reference.
            anim = GetComponent<Animator>();
        }


        void Update()
        {
            if ( isImortalMode == true )
                return;

            // If the player has run out of health...
            if ( playerHealth.currentHealth <= 0 )
            {
                // ... tell the animator the game is over.
                anim.SetTrigger( "GameOver" );
            }
        }
    }
}