using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DivisionLike
{
    public class EffectManager : MonoBehaviour
    {
        public static EffectManager instance = null;
        public GameObject[] _particlePrefabs = new GameObject[ 8 ];
        private Transform _particleParent;

        private Dictionary<int, ParticleSystem> _effectDic = new Dictionary<int, ParticleSystem>();
        private int _indexDic = 0;

        private void Awake()
        {
            instance = this;

            _particleParent = transform.Find( "ParticleParent" );
        }

        public void CreateParticle( int index, Vector3 position )
        {
            //GameObject obj = Instantiate( particlePrefabs[ index ], position, Quaternion.identity, particleParent );
            GameObject obj = Lean.LeanPool.Spawn( _particlePrefabs[ index ], position, Quaternion.identity, _particleParent );
            ParticleSystem particle = obj.transform.Find( "Particle System" ).GetComponent<ParticleSystem>();
            
            _effectDic.Add( _indexDic, particle );

            _indexDic++;
        }

        public ParticleSystem GetParticle( int index )
        {
            return _effectDic[ index ];
        }
    }
}