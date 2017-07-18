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

        public float _blastRadius = 6f;
        public float _lifeTime = 5f;
        public int _damage = 50;
        
        
        // Use this for initialization
        void Awake()
        {
            _explosion = transform.Find( "Explosion" ).gameObject;
            _outline = transform.Find( "WPN_MK2Grenade" ).GetComponent<cakeslice.Outline>();
            _explosion.SetActive( false );

            Invoke( "Explode", _lifeTime );
        }

        private void OnEnable()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnCollisionEnter( Collision collision )
        {
            // as soon as grenade lands on, draw explosion sphere so that player can know how big explosion of grenade is
        }

        private void Explode()
        {
            _outline.gameObject.SetActive( false );
            _explosion.SetActive( true );

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

