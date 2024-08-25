using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DivisionLike
{
    /// <summary>
    /// 인트로 씬
    /// </summary>
    public class IntroScene : MonoBehaviour
    {
        [SerializeField] private IntroGUI m_IntroGUI;

        private void Awake()
        {
        }

        private void Update()
        {
            ProcessKeyInput();
        }

        /// <summary>
        /// 키 입력 처리
        /// </summary>
        private void ProcessKeyInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape) == true)
            {
                Application.Quit();
            }

            if (Input.GetKeyDown(KeyCode.Space) == true || Input.GetKeyDown(KeyCode.Return) == true)
            {
                m_IntroGUI.LoadPlayScene(true);
            }
        }
    }
}