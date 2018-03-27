/*
 * reference - https://www.assetstore.unity3d.com/kr/#!/content/40756
 */

using UnityEngine;
using System.Collections;

namespace DivisionLike
{
    /// <summary>
    /// 
    /// </summary>
    public class RandomAnimationPoint : MonoBehaviour
    {
        public bool m_Randomize;
        [Range( 0f, 1f )] public float m_NormalizedTime;


        void OnValidate()
        {
            GetComponent<Animator>().Update( 0f );
            GetComponent<Animator>().Play( "Walk", 0, m_Randomize ? Random.value : m_NormalizedTime );
        }
    }
}