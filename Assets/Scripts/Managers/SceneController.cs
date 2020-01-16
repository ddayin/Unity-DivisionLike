using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

using UnityEngine.SceneManagement;
using WanzyeeStudio;

namespace DivisionLike
{
    /// <summary>
    /// 씬의 이름
    /// </summary>
    public enum eSceneName
    {
        Intro = 0,  // 인트로
        Loading,    // 로딩
        Play        // 플레이
    }

    /// <summary>
    /// 씬 전환 매니저
    /// </summary>
    public class SceneController : MonoBehaviour
    {
        public static SceneController instance
        {
            get { return Singleton<SceneController>.instance; }
        }

        public eSceneName m_CurrentScene;        // 현재 씬
        
        private void Awake()
        {
            m_CurrentScene = eSceneName.Play;

            DontDestroyOnLoad( this.gameObject );
        }

        /// <summary>
        /// 해당 씬을 불러들인다.
        /// </summary>
        /// <param name="name"></param>
        public void LoadScene( eSceneName name )
        {
            m_CurrentScene = name;
            SceneManager.LoadScene( m_CurrentScene.ToString() );
        }

    }
}

