using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DivisionLike
{
    public class AmmoBox : MonoBehaviour
    {
        private cakeslice.Outline _outline;
        private bool _isInRange = false;

        private void Awake()
        {
            _outline = transform.GetComponent<cakeslice.Outline>();
            _outline.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            if ( Input.GetKey( KeyCode.V ) == true )
            {
                if ( _outline.enabled == true )
                {
                    _isInRange = true;
                }
            }
        }

        private void OnTriggerEnter( Collider other )
        {
            _outline.enabled = true;
        }

        private void OnTriggerStay( Collider other )
        {
            if ( _isInRange == true )
            {
                _isInRange = false;
                for ( int i = 0; i < Player.instance._weaponHandler.weaponsList.Count; i++ )
                {
                    Player.instance._weaponHandler.weaponsList[ i ].ammo.carryingAmmo = Player.instance._weaponHandler.weaponsList[ i ].ammo.carryingMaxAmmo;
                }
            }
        }

        private void OnTriggerExit( Collider other )
        {
            _outline.enabled = false;
            _isInRange = false;
        }
    }
}


