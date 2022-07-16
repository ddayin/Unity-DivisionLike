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

        private Button m_MultiPlayButton;
        private Button m_TrainingButton;
        private Button m_OptionButton;
        private Button m_GithubButton;

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
            m_PlayButton = transform.Find( "PanelMenu/Button_SinglePlay" ).GetComponent<Button>();
            m_MultiPlayButton = transform.Find("PanelMenu/Button_MultiPlay").GetComponent<Button>();
            m_TrainingButton = transform.Find( "PanelMenu/Button_Training" ).GetComponent<Button>();
            m_OptionButton = transform.Find( "PanelMenu/Button_Option" ).GetComponent<Button>();
            m_GithubButton = transform.Find( "PanelMenu/Button_Github" ).GetComponent<Button>();
            m_QuitButton = transform.Find("PanelMenu/Button_Quit").GetComponent<Button>();

            m_PlayButton.onClick.AddListener( OnClickPlayButton );
            m_MultiPlayButton.onClick.AddListener( OnClickMultiButton );
            m_TrainingButton.onClick.AddListener( OnClickTrainingButton );
            m_OptionButton.onClick.AddListener( OnClickOptionButton );
            m_GithubButton.onClick.AddListener( OnClickGithubButton );
            m_QuitButton.onClick.AddListener( OnClickQuitButton );

            m_LoadingScreen = GameObject.Find( "Canvas/LoadingScreen" ).gameObject; 
            m_LoadingScreen.SetActive( false );
        }

        #endregion

        /// <summary>
        /// 게임 시작 버튼을 누르면 게임이 시작된다.
        /// </summary>
        private void OnClickPlayButton()
        {
            LoadPlayScene();
        }

        private void OnClickMultiButton()
        {

        }

        private void OnClickTrainingButton()
        {
            SceneController.instance.LoadScene( SceneName.Training );
        }

        private void OnClickOptionButton()
        {

        }

        private void OnClickGithubButton()
        {
            Application.OpenURL("https://github.com/ddayin/Unity-DivisionLike");
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

