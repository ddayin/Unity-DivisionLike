using UnityEngine;
using System.Collections;

namespace DivisionLike
{
    /// <summary>
    /// 랜덤한 시간으로 파티클을 재생한다.
    /// </summary>
    public class RandomParticlePoint : MonoBehaviour
    {
        [Range(0f, 1f)] public float m_NormalizedTime;


        void OnValidate()
        {
            GetComponent<ParticleSystem>().Simulate(m_NormalizedTime, true, true);
        }
    }
}