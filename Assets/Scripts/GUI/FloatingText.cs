/*
 * reference - https://www.youtube.com/watch?v=fbUOG7f3jq8&t=19s
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DivisionLike
{
    public class FloatingText : MonoBehaviour
    {
        public Animator _animator;
        private Text _damageText;

        void OnEnable()
        {
            AnimatorClipInfo[] clipInfo = _animator.GetCurrentAnimatorClipInfo( 0 );
            Destroy( gameObject, clipInfo[ 0 ].clip.length );
            _damageText = _animator.GetComponent<Text>();
        }

        public void SetText( string text )
        {
            _damageText.text = text;
        }

        public void SetColor( bool isCritical )
        {
            if ( isCritical == true )
            {
                _damageText.color = Color.red;
            }
            
        }
    }
}