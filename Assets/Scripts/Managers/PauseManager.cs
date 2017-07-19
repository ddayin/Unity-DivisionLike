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
        public AudioMixerSnapshot _paused;
        public AudioMixerSnapshot _unpaused;

        private Canvas _canvas;

        private Button _instructionButton;
        private Image _instructionImage;
        private bool _isVisibleInstruction = false;

        public bool _isPaused = false;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();

            _instructionButton = transform.Find( "PausePanel/InstructionButton" ).GetComponent<Button>();
            _instructionButton.onClick.AddListener( ShowIntruction );
            _instructionImage = transform.Find( "InstructionPanel" ).GetComponent<Image>();
            _instructionImage.enabled = false;
        }

        private void OnEnable()
        {
            _instructionImage.enabled = false;


        }

        private void OnDisable()
        {
            _instructionImage.enabled = false;
        }

        void Update()
        {
            if ( Input.GetKeyDown( KeyCode.Escape ) )
            {
                _canvas.enabled = !_canvas.enabled;
                
                Pause();
            }

            if ( Input.GetMouseButtonDown( 0 ) == true )
            {
                if ( _isVisibleInstruction == true )
                {
                    _instructionImage.enabled = false;
                }
            }
        }

        public void Pause()
        {
            _isPaused = !_isPaused;

            if ( _isPaused == true )
            {
                Time.timeScale = 0;
            }
            else if ( _isPaused == false )
            {
                Time.timeScale = 1;
            }

            ShowMouseCursor( _isPaused );

            Lowpass();

        }

        void Lowpass()
        {
            if ( _isPaused == true )
            {
                _paused.TransitionTo( .01f );
            }
            else if ( _isPaused == false )
            {
                _unpaused.TransitionTo( .01f );
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

        private void ShowIntruction()
        {
            _isVisibleInstruction = true;
            
            _instructionImage.enabled = _isVisibleInstruction;
            

        }
    }
}