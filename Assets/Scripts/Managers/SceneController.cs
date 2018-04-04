using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WanzyeeStudio;

namespace DivisionLike
{
    /// <summary>
    /// 씬의 이름
    /// </summary>
    public enum eSceneName
    {
        Intro = 0,
        Play
    }

    /// <summary>
    /// 씬 전환 매니저 매니저
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
            m_CurrentScene = eSceneName.Intro;

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

