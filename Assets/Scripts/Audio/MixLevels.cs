/*
 * reference - https://www.assetstore.unity3d.com/kr/#!/content/40756
 */

using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class MixLevels : MonoBehaviour
{

    public AudioMixer masterMixer;

    public void SetSfxLvl( float sfxLvl )
    {
        masterMixer.SetFloat( "sfxVol", sfxLvl );
    }

    public void SetMusicLvl( float musicLvl )
    {
        masterMixer.SetFloat( "musicVol", musicLvl );
    }
}
