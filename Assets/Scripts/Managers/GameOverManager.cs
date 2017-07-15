/*
 * reference - https://www.assetstore.unity3d.com/kr/#!/content/40756
 */

using UnityEngine;

namespace DivisionLike
{
    public class GameOverManager : MonoBehaviour
    {
        public bool _isImortalMode = false;

        private Animator _anim;                          // Reference to the animator component.


        void Awake()
        {
            // Set up the reference.
            _anim = GetComponent<Animator>();
        }


        void Update()
        {
            if ( _isImortalMode == true )
                return;

            // If the player has run out of health...
            if ( Player.instance._health._currentHealth <= 0 )
            {
                // ... tell the animator the game is over.
                _anim.SetTrigger( "GameOver" );
            }
        }
    }
}