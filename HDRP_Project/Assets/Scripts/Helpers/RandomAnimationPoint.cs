using UnityEngine;
using System.Collections;

namespace DivisionLike
{
    /// <summary>
    /// 랜덤한 시간으로 애니메이션을 재생한다.
    /// </summary>
    public class RandomAnimationPoint : MonoBehaviour
    {
        public bool m_Randomize;
        [Range(0f, 1f)] public float m_NormalizedTime;


        void OnValidate()
        {
            GetComponent<Animator>().Update(0f);
            GetComponent<Animator>().Play("Walk", 0, m_Randomize ? Random.value : m_NormalizedTime);
        }
    }
}