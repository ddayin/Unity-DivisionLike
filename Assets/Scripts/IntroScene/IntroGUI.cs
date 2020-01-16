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
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace DivisionLike
{
    /// <summary>
    /// 인트로 화면 GUI
    /// </summary>
    public class IntroGUI : MonoBehaviour
    {
        private Button m_PlayButton;
        private GameObject m_LoadingScreen;

        #region MonoBehaviour
        private void Awake()
        {
            m_PlayButton = transform.Find( "Button_Play" ).GetComponent<Button>();
            m_PlayButton.onClick.AddListener( OnClickPlayButton );

            m_LoadingScreen = transform.Find( "LoadingScreen" ).gameObject;
        }

        #endregion

        private void OnClickPlayButton()
        {
            m_LoadingScreen.SetActive( true );
            StartCoroutine( LoadScene() );
        }

        IEnumerator LoadScene()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync( "PlayNew" );
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
    }
}

