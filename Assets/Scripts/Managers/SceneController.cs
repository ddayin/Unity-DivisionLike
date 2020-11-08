using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WanzyeeStudio;

namespace DivisionLike
{
    /// <summary>
    /// 씬 이름
    /// </summary>
    public enum SceneName
    {
        /// <summary>
        /// 
        /// </summary>
        None = -1,

        /// <summary>
        /// 처음 인트로 화면
        /// </summary>
        Intro = 0,

        /// <summary>
        /// 실제 인게임 플레이 화면
        /// </summary>
        Play
    }

    /// <summary>
    /// 씬을 불러들이고 관리한다.
    /// </summary>
    public class SceneController : BaseSingleton<SceneController>
    {
        /// <summary>
        /// 현재 실행 중인 씬의 이름
        /// </summary>
        [SerializeField] private SceneName m_CurrentSceneName = SceneName.None;

        public SceneName CurrentScene
        {
            get { return m_CurrentSceneName; }
            set { m_CurrentSceneName = value; }
        }

        protected override void Awake()
        {
            base.Awake();


        }

        private void Start()
        {
            
        }

        /// <summary>
        /// 현재 활성화된 씬의 이름을 반환한다.
        /// </summary>
        /// <returns></returns>
        public string GetActiveScene()
        {
            Scene scene = SceneManager.GetActiveScene();
            return scene.name;
        }

        /// <summary>
        /// 지정된 이름을 가진 씬을 불러들인다.
        /// </summary>
        /// <param name="name"></param>
        public void LoadScene(SceneName name)
        {
            m_CurrentSceneName = name;
            SceneManager.LoadScene(m_CurrentSceneName.ToString());
        }

        /// <summary>
        /// 지정된 이름을 가진 씬을 불러들인다.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public AsyncOperation LoadSceneAsyn(SceneName name)
        {
            m_CurrentSceneName = name;
            return SceneManager.LoadSceneAsync(m_CurrentSceneName.ToString());
        }
    }
}