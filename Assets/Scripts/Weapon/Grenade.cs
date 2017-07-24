using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DivisionLike
{
    public class Grenade : MonoBehaviour
    {
        private Collider[] _hitColliders;
        private GameObject _explosion;
        private cakeslice.Outline _outline;

        public GameObject _explosionPrefab;

        public float _blastRadius = 8f;
        public float _lifeTime = 4f;
        public int _damage = 70;
        
        
        // Use this for initialization
        void Awake()
        {
            //_explosion = transform.Find( "Explosion" ).gameObject;
            _outline = transform.Find( "WPN_MK2Grenade" ).GetComponent<cakeslice.Outline>();

            //_explosion.SetActive( false );
            
            Invoke( "Explode", _lifeTime );
        }

        private void OnEnable()
        {
            
        }
        

        private void OnCollisionEnter( Collision collision )
        {
            // as soon as grenade lands on, draw explosion sphere so that player can know how big explosion of grenade is
        }

        private void Explode()
        {
            _outline.gameObject.SetActive( false );

            GameObject explosion = (GameObject) Instantiate( _explosionPrefab, transform.position, Quaternion.identity, transform );
            //explosion.GetComponent<UnityStandardAssets.Effects.ParticleSystemMultiplier>().multiplier = 1f;
            //_explosion.SetActive( true );
            
            _hitColliders = Physics.OverlapSphere( transform.position, _blastRadius, LayerMask.GetMask( "Ragdoll" ) );
            
            foreach ( Collider hitCol in _hitColliders )
            {
                EnemyHealth enemy = hitCol.GetComponent<EnemyHealth>();
                
                if ( enemy != null )
                {
                    Debug.Log( "enemy name " + hitCol.gameObject.name + " got damage " + _damage + " by grenade" );
                    enemy.TakeDamage( _damage, hitCol.ClosestPoint( transform.position ) );
                }
            }

            Destroy( gameObject, 2f );
        }

    }
}

