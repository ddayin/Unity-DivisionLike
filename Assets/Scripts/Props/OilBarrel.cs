using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DivisionLike
{
    public class OilBarrel : MonoBehaviour
    {
        public int _currentHealth = 100;
        public int _maxHealth = 100;
        public GameObject _explosionPrefab;
        public float _blastRadius = 8f;
        public int _damage = 70;

        private cakeslice.Outline _outline;
        private Collider[] _hitColliders;
        private bool _isExploded = false;
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _outline = transform.GetComponent<cakeslice.Outline>();
            _rigidbody = transform.GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter( Collision collision )
        {
            
        }

        public void TakeDamage( int amount )
        {
            if ( _isExploded == true )
            {
                return;
            }
            _outline.enabled = true;
            Invoke( "DisableOutline", 1f );

            _currentHealth -= amount;

            if ( _currentHealth <= 0 )
            {
                Explode();
            }
        }

        private void DisableOutline()
        {
            _outline.enabled = false;
        }


        private void Explode()
        {
            if ( _isExploded == true )
            {
                return;
            }

            _rigidbody.AddExplosionForce( 600f, transform.position, _blastRadius, 300f );

            _outline.enabled = false;

            GameObject explosion = (GameObject) Instantiate( _explosionPrefab, transform.position, Quaternion.identity, transform );

            _hitColliders = Physics.OverlapSphere( transform.position, _blastRadius, LayerMask.GetMask( "Ragdoll" ) );

            foreach ( Collider hitCol in _hitColliders )
            {
                EnemyHealth enemy = hitCol.GetComponent<EnemyHealth>();

                if ( enemy != null )
                {
                    Debug.Log( "enemy name " + hitCol.gameObject.name + " got damage " + _damage + " by grenade" );
                    enemy.TakeDamage( _damage, hitCol.ClosestPoint( transform.position ) );
                }

                Rigidbody rig = hitCol.GetComponent<Rigidbody>();
                if ( rig != null )
                {
                    rig.AddExplosionForce( 200f, transform.position, _blastRadius );
                }
            }

            Destroy( this.gameObject, 5f );

            _isExploded = true;
        }
    }
}


