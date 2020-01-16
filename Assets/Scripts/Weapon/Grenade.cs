/*
MIT License

Copyright (c) 2020 ddayin

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DivisionLike
{
    /// <summary>
    /// 슈류탄
    /// </summary>
    public class Grenade : MonoBehaviour
    {
        private Collider[] m_HitColliders;
        private GameObject m_Explosion;
        private cakeslice.Outline m_Outline;

        public GameObject m_ExplosionPrefab;

        public float m_BlastRadius = 8f;
        public float m_LifeTime = 4f;
        public int m_Damage = 70;
        
        
        // Use this for initialization
        void Awake()
        {
            //_explosion = transform.Find( "Explosion" ).gameObject;
            m_Outline = transform.Find( "WPN_MK2Grenade" ).GetComponent<cakeslice.Outline>();

            //_explosion.SetActive( false );
            
            Invoke( "Explode", m_LifeTime );
        }

        private void OnEnable()
        {
            
        }
        

        private void OnCollisionEnter( Collision collision )
        {
            // as soon as grenade lands on, draw explosion sphere so that player can know how big explosion of grenade is
        }

        /// <summary>
        /// 폭파시킨다.
        /// </summary>
        private void Explode()
        {
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
            }

            Destroy( gameObject, 2f );
        }

    }
}

