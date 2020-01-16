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

/*
 * reference - https://www.youtube.com/watch?v=gN1BmP4ONSQ&list=PLfxIz_UlKk7IwrcF2zHixNtFmh0lznXow&t=4140s&index=4
 */

using UnityEngine;
using System.Collections;
using WanzyeeStudio;

namespace DivisionLike
{
    /// <summary>
    /// 사운드 처리
    /// </summary>
    public class SoundController : MonoBehaviour
    {
        public static SoundController instance
        {
            get { return Singleton<SoundController>.instance; }
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