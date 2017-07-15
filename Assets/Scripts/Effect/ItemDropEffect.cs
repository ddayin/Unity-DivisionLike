using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DivisionLike
{
    public class ItemDropEffect : MonoBehaviour
    {
        private LineRenderer _lineUp;

        private void Awake()
        {
            _lineUp = transform.GetComponent<LineRenderer>();
        }

        private void OnEnable()
        {
            _lineUp.SetPosition( 0, transform.position );

            Vector3 onePosition = transform.position;
            onePosition.y = 100f;
            _lineUp.SetPosition( 1, onePosition );
        }

        void OnTriggerEnter( Collider other )
        {
            if ( other.gameObject == Player.instance.gameObject )
            {
                int ammo = Random.Range( 1, 30 );
                if ( Player.instance._weaponHandler.currentWeapon.weaponName.Equals( "Makarov" ) == false )
                {
                    Player.instance._inventory.ObtainAmmo( ammo );
                    //Destroy( gameObject );
                    Lean.LeanPool.Despawn( gameObject );
                }
                
            }
        }

        void OnTriggerExit( Collider other )
        {
            
        }
    }
}