/*
 * reference - https://www.assetstore.unity3d.com/kr/#!/content/40756
 */

using UnityEngine;
using System.Collections;

namespace DivisionLike
{
    public class RandomParticlePoint : MonoBehaviour
    {
        [Range( 0f, 1f )]
        public float m_NormalizedTime;


        void OnValidate()
        {
            GetComponent<ParticleSystem>().Simulate( m_NormalizedTime, true, true );
        }
    }
}