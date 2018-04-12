// reference - http://wergia.tistory.com/59


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DivisionLike
{
    public class LoadingSceneManager : MonoBehaviour
    {
        public Loading m_LoadingAni;
        private AsyncOperation m_AO;
        private float m_Progress = 0f;

        private void Awake()
        {
            if ( m_LoadingAni == null ) return;

            StartCoroutine( LoadScene() );

            Invoke( "Test", 5f );
        }

        //private void Test()
        //{
        //    m_AO.allowSceneActivation = true;
        //}



        IEnumerator LoadScene()
        {
            yield return null;

            m_AO = SceneManager.LoadSceneAsync( "Play" );
            m_AO.allowSceneActivation = false;


            while ( m_AO.isDone == false )
            {
                yield return null;

                if ( m_AO.progress < 0.9f )
                {
                    m_Progress = Mathf.Lerp( 0f, 360f, m_AO.progress );

                    m_LoadingAni.RotateDegreeZ( m_Progress );
                }
                else
                {
                    m_AO.allowSceneActivation = true;
                    SceneController.instance.m_CurrentScene = eSceneName.Play;
                }
            }
        }

        private void OnGUI()
        {
            if ( m_AO == null ) return;

            GUI.Label( new Rect( 0, 0, 100, 100 ), m_AO.progress.ToString() );
            GUI.Label( new Rect( 0, 100, 100, 100 ), m_Progress.ToString() );
        }
    }
}


