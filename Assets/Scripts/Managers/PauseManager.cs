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

        public bool isPaused = false;

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
            isPaused = !isPaused;

            if ( isPaused == true )
            {
                Time.timeScale = 0;
            }
            else if ( isPaused == false )
            {
                Time.timeScale = 1;
            }

            ShowMouseCursor( isPaused );

            Lowpass();

        }

        void Lowpass()
        {
            if ( isPaused == true )
            {
                paused.TransitionTo( .01f );
            }
            else if ( isPaused == false )
            {
                unpaused.TransitionTo( .01f );
            }
        }

        private void ShowMouseCursor( bool isShow )
        {
            if ( isShow == true )
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else if ( isShow == false )
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
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