/*
 * reference - https://www.youtube.com/watch?v=gN1BmP4ONSQ&list=PLfxIz_UlKk7IwrcF2zHixNtFmh0lznXow&t=4140s&index=4
 */

using UnityEngine;
using System.Collections;

namespace DivisionLike
{
    public class SoundController : MonoBehaviour
    {
        public static SoundController instance = null;

        private void Awake()
        {
            instance = this;
        
        }

        /// <summary>
        /// 해당 사운드를 재생한다.
        /// </summary>
        /// <param name="audioS"></param>
        /// <param name="clip"></param>
        /// <param name="randomizePitch"></param>
        /// <param name="randomPitchMin"></param>
        /// <param name="randomPitchMax"></param>
        public void PlaySound( AudioSource audioS, AudioClip clip, bool randomizePitch = false, float randomPitchMin = 1, float randomPitchMax = 1 )
        {

            audioS.clip = clip;

            if ( randomizePitch == true )
            {
                audioS.pitch = Random.Range( randomPitchMin, randomPitchMax );
            }

            audioS.Play();
        }

        /// <summary>
        /// 해당 오디오 클립을 생성하여 재생한다.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="clip"></param>
        /// <param name="time"></param>
        /// <param name="randomizePitch"></param>
        /// <param name="randomPitchMin"></param>
        /// <param name="randomPitchMax"></param>
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
}