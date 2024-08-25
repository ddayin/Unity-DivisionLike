using System.Collections.Generic;
using UnityEngine;

namespace DivisionLike
{
    /// <summary>
    /// 이펙트 관리자
    /// </summary>
    public class EffectManager : MonoBehaviour
    {
        public static EffectManager instance { get; private set; }

        public GameObject[] m_ParticlePrefabs = new GameObject[8];
        private Transform m_ParticleParent;

        private Dictionary<int, ParticleSystem> m_EffectDic = new Dictionary<int, ParticleSystem>();
        private int m_IndexDic = 0;

        private void Awake()
        {
            instance = this;
            m_ParticleParent = transform.Find("ParticleParent");
        }

        /// <summary>
        /// 지정된 위치에 해당 인덱스의 파티클을 생성한다.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="position"></param>
        public void CreateParticle(int index, Vector3 position)
        {
            //GameObject obj = Instantiate( particlePrefabs[ index ], position, Quaternion.identity, particleParent );
            GameObject obj = Lean.LeanPool.Spawn(m_ParticlePrefabs[index], position, Quaternion.identity,
                m_ParticleParent);
            ParticleSystem particle = obj.transform.Find("Particle System").GetComponent<ParticleSystem>();

            m_EffectDic.Add(m_IndexDic, particle);

            m_IndexDic++;
        }

        /// <summary>
        /// 해당 인덱스의 파티클을 반환한다.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ParticleSystem GetParticle(int index)
        {
            return m_EffectDic[index];
        }
    }
}