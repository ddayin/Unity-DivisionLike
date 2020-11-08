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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WanzyeeStudio;


namespace DivisionLike
{
    /// <summary>
    /// 인트로 화면 GUI
    /// </summary>
    public class IntroGUI : BaseSingleton<IntroGUI>
    {
        /// <summary>
        /// 게임 시작 버튼
        /// </summary>
        private Button m_PlayButton;

        /// <summary>
        /// 게임 어플리케이션 종료 버튼
        /// </summary>
        private Button m_QuitButton;

        /// <summary>
        /// 로딩 화면
        /// </summary>
        private GameObject m_LoadingScreen;

        #region MonoBehaviour
        protected override void Awake()
        {
            base.Awake();

            m_PlayButton = transform.Find( "Button_Play" ).GetComponent<Button>();
            m_PlayButton.onClick.AddListener( OnClickPlayButton );

            m_QuitButton = transform.Find( "Button_Quit" ).GetComponent<Button>();
            m_QuitButton.onClick.AddListener( OnClickQuitButton );

            m_LoadingScreen = transform.Find( "LoadingScreen" ).gameObject;
        }

        #endregion

        /// <summary>
        /// 게임 시작 버튼을 누르면 게임이 시작된다.
        /// </summary>
        private void OnClickPlayButton()
        {
            LoadPlayScene();
        }

        /// <summary>
        /// 종료 버튼을 클릭하면 게임 어플리케이션을 종료한다.
        /// </summary>
        private void OnClickQuitButton()
        {
            Application.Quit();
        }

        /// <summary>
        /// 플레이 씬을 불러들인다.
        /// </summary>
        public void LoadPlayScene()
        {
            m_LoadingScreen.SetActive( true );

            SceneManager.sceneLoaded += LoadedSceneComplete;

            StartCoroutine( LoadScene() );
        }

        IEnumerator LoadScene()
        {
            AsyncOperation operation = SceneController.instance.LoadSceneAsyn(SceneName.Play);
            operation.allowSceneActivation = false;

            float timer = 0f;
            while ( operation.isDone == false )
            {
                yield return null;
                timer += Time.deltaTime;

                if ( operation.progress < 0.9f )
                {
                    
                }
                else
                {
                    operation.allowSceneActivation = true;
                    
                    yield break;
                }
            }
        }

        private void LoadedSceneComplete(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.name == SceneController.instance.CurrentScene.ToString())
            {
                m_LoadingScreen.SetActive(false);
                SceneManager.sceneLoaded -= LoadedSceneComplete;
            }
        }
    }
}

