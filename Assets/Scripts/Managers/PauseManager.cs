/*
 * reference - https://www.assetstore.unity3d.com/kr/#!/content/40756
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DivisionLike
{
    public class PauseManager : MonoBehaviour
    {

        public AudioMixerSnapshot paused;
        public AudioMixerSnapshot unpaused;

        Canvas canvas;

        void Start()
        {
            canvas = GetComponent<Canvas>();

            
        }

        void Update()
        {
            if ( Input.GetKeyDown( KeyCode.Escape ) )
            {
                canvas.enabled = !canvas.enabled;
                Pause();
            }
        }

        public void Pause()
        {
            //Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;

            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
            Lowpass();

        }

        void Lowpass()
        {
            if ( Time.timeScale == 0 )
            {
                paused.TransitionTo( .01f );
            }

            else

            {
                unpaused.TransitionTo( .01f );
            }
        }

        public void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
        }
    }
}