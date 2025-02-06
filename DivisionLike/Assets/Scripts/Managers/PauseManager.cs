using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;
#if UNITY_EDITOR
using UnityEditor;
#endif
using WanzyeeStudio;

namespace DivisionLike
{
    /// <summary>
    /// 일시 정지 관리자
    /// </summary>
    public class PauseManager : BaseSingleton<PauseManager>
    {
        private Canvas m_Canvas;
        private Button m_InstructionButton;
        private Button m_QuitButton;
        private Button m_ResumeButton;
        private Image m_InstructionImage;
        private bool m_IsVisibleInstruction = false;

        public bool m_IsPaused = false;

        protected override void Awake()
        {
            base.Awake();

            m_Canvas = GetComponent<Canvas>();
            m_InstructionButton = transform.Find("PausePanel/InstructionButton").GetComponent<Button>();
            m_QuitButton = transform.Find("PausePanel/QuitButton").GetComponent<Button>();
            m_ResumeButton = transform.Find("PausePanel/ResumeButton").GetComponent<Button>();
            m_InstructionImage = transform.Find("InstructionPanel").GetComponent<Image>();
            m_InstructionImage.enabled = false;

            m_InstructionButton.onClick.AddListener(ShowIntruction);
            m_QuitButton.onClick.AddListener(ReturnToIntroScene);
            m_ResumeButton.onClick.AddListener(Resume);
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
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                m_Canvas.enabled = !m_Canvas.enabled;

                Pause();
            }

            if (Input.GetMouseButtonDown(0) == true)
            {
                if (m_IsVisibleInstruction == true)
                {
                    m_InstructionImage.enabled = false;
                }
            }
        }

        public void Resume()
        {
            m_IsPaused = false;
            Time.timeScale = 1;
            ShowMouseCursor(false);
        }

        /// <summary>
        /// 일시 정지
        /// </summary>
        public void Pause()
        {
            m_IsPaused = !m_IsPaused;

            if (m_IsPaused == true)
            {
                Time.timeScale = 0;
            }
            else if (m_IsPaused == false)
            {
                Time.timeScale = 1;
            }

            ShowMouseCursor(m_IsPaused);
        }

        /// <summary>
        /// 마우스 커서를 보일 것인지
        /// </summary>
        /// <param name="isShow"></param>
        private void ShowMouseCursor(bool isShow)
        {
            if (isShow == true)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else if (isShow == false)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void ReturnToIntroScene()
        {
            Time.timeScale = 1;
            //SceneController.instance.LoadScene(SceneName.Intro);
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