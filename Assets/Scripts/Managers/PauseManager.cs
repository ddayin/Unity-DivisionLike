/*
MIT License

Copyright (c) 2019 ddayin

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
        public AudioMixerSnapshot m_Paused;
        public AudioMixerSnapshot m_Unpaused;

        private Canvas m_Canvas;
        private Button m_InstructionButton;
        private Image m_InstructionImage;
        private bool m_IsVisibleInstruction = false;

        public bool m_IsPaused = false;

        private void Awake()
        {
            m_Canvas = GetComponent<Canvas>();

            m_InstructionButton = transform.Find( "PausePanel/InstructionButton" ).GetComponent<Button>();
            m_InstructionButton.onClick.AddListener( ShowIntruction );
            m_InstructionImage = transform.Find( "InstructionPanel" ).GetComponent<Image>();
            m_InstructionImage.enabled = false;
        }

        private void OnEnable()
        {
            m_InstructionImage.enabled = false;
        }

        private void OnDisable()
        {
            m_InstructionImage.enabled = false;
        }

        void Update()
        {
            if ( Input.GetKeyDown( KeyCode.Escape ) )
            {
                m_Canvas.enabled = !m_Canvas.enabled;
                
                Pause();
            }

            if ( Input.GetMouseButtonDown( 0 ) == true )
            {
                if ( m_IsVisibleInstruction == true )
                {
                    m_InstructionImage.enabled = false;
                }
            }
        }

        /// <summary>
        /// 일시 정지
        /// </summary>
        public void Pause()
        {
            m_IsPaused = !m_IsPaused;

            if ( m_IsPaused == true )
            {
                Time.timeScale = 0;
            }
            else if ( m_IsPaused == false )
            {
                Time.timeScale = 1;
            }

            ShowMouseCursor( m_IsPaused );

            Lowpass();

        }

        /// <summary>
        /// 
        /// </summary>
        void Lowpass()
        {
            if ( m_IsPaused == true )
            {
                m_Paused.TransitionTo( .01f );
            }
            else if ( m_IsPaused == false )
            {
                m_Unpaused.TransitionTo( .01f );
            }
        }

        /// <summary>
        /// 마우스 커서를 보일 것인지
        /// </summary>
        /// <param name="isShow"></param>
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

        /// <summary>
        /// 어플리케이션 종료
        /// </summary>
        public void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
		    Application.Quit();
#endif
        }

        /// <summary>
        /// 안내 이미지 표시
        /// </summary>
        private void ShowIntruction()
        {
            m_IsVisibleInstruction = true;
            
            m_InstructionImage.enabled = m_IsVisibleInstruction;
            

        }
    }
}