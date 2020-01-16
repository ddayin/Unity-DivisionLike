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
using WanzyeeStudio;


namespace DivisionLike
{
    /// <summary>
    /// 이펙트 관리자
    /// </summary>
    public class EffectManager : MonoBehaviour
    {
        public static EffectManager instance
        {
            get { return Singleton<EffectManager>.instance; }
        }

        public GameObject[] m_ParticlePrefabs = new GameObject[ 8 ];
        private Transform m_ParticleParent;

        private Dictionary<int, ParticleSystem> m_EffectDic = new Dictionary<int, ParticleSystem>();
        private int m_IndexDic = 0;

        private void Awake()
        {
            m_ParticleParent = transform.Find( "ParticleParent" );
        }

        /// <summary>
        /// 지정된 위치에 해당 인덱스의 파티클을 생성한다.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="position"></param>
        public void CreateParticle( int index, Vector3 position )
        {
            //GameObject obj = Instantiate( particlePrefabs[ index ], position, Quaternion.identity, particleParent );
            GameObject obj = Lean.LeanPool.Spawn( m_ParticlePrefabs[ index ], position, Quaternion.identity, m_ParticleParent );
            ParticleSystem particle = obj.transform.Find( "Particle System" ).GetComponent<ParticleSystem>();
            
            m_EffectDic.Add( m_IndexDic, particle );

            m_IndexDic++;
        }

        /// <summary>
        /// 해당 인덱스의 파티클을 반환한다.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ParticleSystem GetParticle( int index )
        {
            return m_EffectDic[ index ];
        }
    }
}