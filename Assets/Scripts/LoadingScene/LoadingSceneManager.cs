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


