using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace DivisionLike
{
    /// <summary>
    /// 인트로 화면 GUI
    /// </summary>
    public class IntroGUI : MonoBehaviour
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
        private void Awake()
        {
            m_PlayButton = transform.Find( "PanelMenu/Button_Play" ).GetComponent<Button>();
            m_PlayButton.onClick.AddListener( OnClickPlayButton );

            m_QuitButton = transform.Find("PanelMenu/Button_Quit").GetComponent<Button>();
            m_QuitButton.onClick.AddListener( OnClickQuitButton );

            m_LoadingScreen = transform.Find( "PanelMenu/LoadingScreen" ).gameObject;
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

        /// <summary>
        /// 씬을 불러들이는 작업이 완료되면 처리할 작업들
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="loadSceneMode"></param>
        private void LoadedSceneComplete(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.name == SceneController.instance.CurrentScene.ToString())
            {
                SceneManager.sceneLoaded -= LoadedSceneComplete;
            }
        }
    }
}

