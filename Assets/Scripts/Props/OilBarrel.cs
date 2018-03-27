using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DivisionLike
{
    public class OilBarrel : MonoBehaviour
    {
        public int m_CurrentHealth = 100;
        public int m_MaxHealth = 100;
        public GameObject m_ExplosionPrefab;
        public float m_BlastRadius = 8f;
        public int m_Damage = 70;

        private cakeslice.Outline m_Outline;
        private Collider[] m_HitColliders;
        private bool m_IsExploded = false;
        private Rigidbody m_Rigidbody;

        private void Awake()
        {
            m_Outline = transform.GetComponent<cakeslice.Outline>();
            m_Rigidbody = transform.GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter( Collision collision )
        {
            
        }

        /// <summary>
        /// 데미지를 받는다.
        /// </summary>
        /// <param name="amount"></param>
        public void TakeDamage( int amount )
        {
            if ( m_IsExploded == true )
            {
                return;
            }
            m_Outline.enabled = true;
            Invoke( "DisableOutline", 1f );

            m_CurrentHealth -= amount;

            if ( m_CurrentHealth <= 0 )
            {
                Explode();
            }
        }

        /// <summary>
        /// 아웃라인을 비활성화 시킨다.
        /// </summary>
        private void DisableOutline()
        {
            m_Outline.enabled = false;
        }

        /// <summary>
        /// 폭파시킨다.
        /// </summary>
        private void Explode()
        {
            if ( m_IsExploded == true )
            {
                return;
            }

            m_Rigidbody.AddExplosionForce( 600f, transform.position, m_BlastRadius, 300f );

            m_Outline.enabled = false;

            GameObject explosion = (GameObject) Instantiate( m_ExplosionPrefab, transform.position, Quaternion.identity, transform );

            m_HitColliders = Physics.OverlapSphere( transform.position, m_BlastRadius, LayerMask.GetMask( "Ragdoll" ) );

            foreach ( Collider hitCol in m_HitColliders )
            {
                EnemyHealth enemy = hitCol.GetComponent<EnemyHealth>();

                if ( enemy != null )
                {
                    Debug.Log( "enemy name " + hitCol.gameObject.name + " got damage " + m_Damage + " by grenade" );
                    enemy.TakeDamage( m_Damage, hitCol.ClosestPoint( transform.position ) );
                }

                Rigidbody rig = hitCol.GetComponent<Rigidbody>();
                if ( rig != null )
                {
                    rig.AddExplosionForce( 200f, transform.position, m_BlastRadius );
                }
            }

            Destroy( this.gameObject, 5f );

            m_IsExploded = true;
        }
    }
}


