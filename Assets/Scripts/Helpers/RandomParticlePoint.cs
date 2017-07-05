/*
 * reference - https://www.assetstore.unity3d.com/kr/#!/content/40756
 */

using UnityEngine;
using System.Collections;

public class RandomParticlePoint : MonoBehaviour
{
    [Range( 0f, 1f )]
    public float normalizedTime;


    void OnValidate()
    {
        GetComponent<ParticleSystem>().Simulate( normalizedTime, true, true );
    }
}
