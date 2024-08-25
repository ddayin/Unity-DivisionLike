using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DivisionLike
{
    /// <summary>
    /// 볼륨 조정
    /// </summary>
    public class VolumeHandler : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            if (GameObject.Find("EffectsSlider"))
                GameObject.Find("EffectsSlider").GetComponent<Slider>().onValueChanged.AddListener(SetVolume);
        }

        /// <summary>
        /// 지정된 볼륨으로 설정한다.
        /// </summary>
        /// <param name="volume"></param>
        void SetVolume(float volume)
        {
            GetComponent<AudioSource>().volume = volume;
        }

        void OnDestroy()
        {
            if (GameObject.Find("EffectsSlider"))
                GameObject.Find("EffectsSlider").GetComponent<Slider>().onValueChanged.RemoveListener(SetVolume);
        }
    }
}