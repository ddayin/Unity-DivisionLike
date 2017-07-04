/*
 * reference - https://www.youtube.com/watch?v=gN1BmP4ONSQ&list=PLfxIz_UlKk7IwrcF2zHixNtFmh0lznXow&t=4140s&index=4
 */

using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour
{

    public void PlaySound( AudioSource audioS, AudioClip clip, bool randomizePitch = false, float randomPitchMin = 1, float randomPitchMax = 1 )
    {

        audioS.clip = clip;

        if ( randomizePitch == true )
        {
            audioS.pitch = Random.Range( randomPitchMin, randomPitchMax );
        }

        audioS.Play();
    }

    public void InstantiateClip( Vector3 pos, AudioClip clip, float time = 2f, bool randomizePitch = false, float randomPitchMin = 1, float randomPitchMax = 1 )
    {
        GameObject clone = new GameObject( "one shot audio" );
        clone.transform.position = pos;
        AudioSource audio = clone.AddComponent<AudioSource>();
        audio.spatialBlend = 1;
        audio.clip = clip;
        audio.Play();

        Destroy( clone, time );
    }
}
