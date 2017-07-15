using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    public class CrosshairAR : CrosshairHandler
    {
        public Image[] crosshairs;
        private float walkSize;

        private void Awake()
        {
            gameObject.SetActive( false );

            walkSize = crosshairs[ 0 ].rectTransform.localPosition.y;
        }

        private void OnEnable()
        {
            walkSize = 10f;

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
                crosshairs[ i ].color = color;
            }
        }

        private void UpdateCrosshair()
        {
            // y+ x+ x- y- 
            float crossHairSize = calculateCrossHair();

            crosshairs[ 0 ].rectTransform.localPosition = Vector3.Slerp( crosshairs[ 0 ].rectTransform.localPosition, new Vector3( 0f, crossHairSize, 0f ), Time.deltaTime * 8f );
            crosshairs[ 1 ].rectTransform.localPosition = Vector3.Slerp( crosshairs[ 1 ].rectTransform.localPosition, new Vector3( crossHairSize, 0f, 0f ), Time.deltaTime * 8f );
            crosshairs[ 2 ].rectTransform.localPosition = Vector3.Slerp( crosshairs[ 2 ].rectTransform.localPosition, new Vector3( -crossHairSize, 0f, 0f ), Time.deltaTime * 8f );
            crosshairs[ 3 ].rectTransform.localPosition = Vector3.Slerp( crosshairs[ 3 ].rectTransform.localPosition, new Vector3( 0f, -crossHairSize, 0f ), Time.deltaTime * 8f );
        }

        public float calculateCrossHair()
        {
            float size = walkSize * Player.instance._weaponHandler.currentWeapon.weaponSettings.crossHairSize;

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