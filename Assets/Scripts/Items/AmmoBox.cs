using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DivisionLike
{
    public class AmmoBox : MonoBehaviour
    {
        private cakeslice.Outline _outline;
        
        public enum AmmoBoxState
        {
            OutOfRange = 0,     // color green
            InRange,            // color blue
            Emtpy               // color red
        }

        public AmmoBoxState _state = AmmoBoxState.OutOfRange;

        private void Awake()
        {
            _outline = transform.GetComponent<cakeslice.Outline>();
            _outline.enabled = true;
        }
        
        // Update is called once per frame
        void Update()
        {
            CheckInput();
            SetOutlineColor();
        }

        private void OnTriggerEnter( Collider other )
        {
            if ( _state == AmmoBoxState.Emtpy)
            {
                return;
            }

            _state = AmmoBoxState.InRange;
        }

        private void OnTriggerStay( Collider other )
        {

        }

        private void OnTriggerExit( Collider other )
        {
            if ( _state == AmmoBoxState.Emtpy )
            {
                return;
            }

            _state = AmmoBoxState.OutOfRange;
        }

        private float _timer = 0f;
        public float _keyPressTime = 3f;

        private void CheckInput()
        {
            if ( _state == AmmoBoxState.OutOfRange || _state == AmmoBoxState.Emtpy )
            {
                return;
            }

            if ( Input.GetKey( KeyCode.V ) == true )
            {
                _timer += Time.deltaTime;

                ScreenHUD.instance.SetEnableLoadingCircle( true );

                float amount = _timer / _keyPressTime;
                ScreenHUD.instance.SetLoadingCircle( amount );

                if ( _timer > _keyPressTime )
                {
                    Debug.Log( "ammo box give you full ammo" );

                    _timer = 0f;

                    ScreenHUD.instance.SetEnableLoadingCircle( false );

                    // full ammo
                    for ( int i = 0; i < Player.instance._weaponHandler.weaponsList.Count; i++ )
                    {
                        Player.instance._weaponHandler.weaponsList[ i ].ammo.carryingAmmo = Player.instance._weaponHandler.weaponsList[ i ].ammo.carryingMaxAmmo;
                    }

                    _state = AmmoBoxState.Emtpy;
                }
            }
            else
            {
                _timer = 0f;
                ScreenHUD.instance.InitLoadingCircle();
            }
        }
        
        private void SetOutlineColor()
        {
            switch( _state )
            {
                case AmmoBoxState.OutOfRange:
                    _outline.color = 1;
                    break;

                case AmmoBoxState.InRange:
                    _outline.color = 2;
                    break;

                case AmmoBoxState.Emtpy:
                    _outline.color = 0;
                    break;

                default:
                    break;
            }
        }
    }
}


