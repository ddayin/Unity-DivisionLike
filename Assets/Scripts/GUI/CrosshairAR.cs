using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    public class CrosshairAR : CrosshairHandler
    {
        public Image[] _crosshairImages;
        private float _walkSize;

        private void Awake()
        {
            gameObject.SetActive( false );

            _walkSize = _crosshairImages[ 0 ].rectTransform.localPosition.y;
        }

        private void OnEnable()
        {
            _walkSize = 10f;

        }

        private void Update()
        {
            UpdateCrosshair();
        }

        public override void ChangeColor( Color color )
        {
            Debug.Log( "CrosshairAR.ChangeColor() overrided" );
            for ( int i = 0; i < 4; i++ )
            {
                _crosshairImages[ i ].color = color;
            }
        }

        private void UpdateCrosshair()
        {
            // y+ x+ x- y- 
            float crossHairSize = calculateCrossHair();

            _crosshairImages[ 0 ].rectTransform.localPosition = Vector3.Slerp( _crosshairImages[ 0 ].rectTransform.localPosition, new Vector3( 0f, crossHairSize, 0f ), Time.deltaTime * 8f );
            _crosshairImages[ 1 ].rectTransform.localPosition = Vector3.Slerp( _crosshairImages[ 1 ].rectTransform.localPosition, new Vector3( crossHairSize, 0f, 0f ), Time.deltaTime * 8f );
            _crosshairImages[ 2 ].rectTransform.localPosition = Vector3.Slerp( _crosshairImages[ 2 ].rectTransform.localPosition, new Vector3( -crossHairSize, 0f, 0f ), Time.deltaTime * 8f );
            _crosshairImages[ 3 ].rectTransform.localPosition = Vector3.Slerp( _crosshairImages[ 3 ].rectTransform.localPosition, new Vector3( 0f, -crossHairSize, 0f ), Time.deltaTime * 8f );
        }

        public float calculateCrossHair()
        {
            float size = _walkSize * Player.instance._weaponHandler.currentWeapon.weaponSettings.crossHairSize;

            if ( Player.instance._userInput._isSprinting == true )
            {
                size *= 2;
            }
            else
            {
                size /= 2;
            }

            return size;
        }
        
    }
}