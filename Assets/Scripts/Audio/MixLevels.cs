/*
 * reference - https://www.assetstore.unity3d.com/kr/#!/content/40756
 */

using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

namespace DivisionLike
{
    public class MixLevels : MonoBehaviour
    {
        public AudioMixer _masterMixer;

        public void SetSfxLvl( float sfxLvl )
        {
            _masterMixer.SetFloat( "sfxVol", sfxLvl );
        }

        public void SetMusicLvl( float musicLvl )
        {
            _masterMixer.SetFloat( "musicVol", musicLvl );
        }
    }
}
