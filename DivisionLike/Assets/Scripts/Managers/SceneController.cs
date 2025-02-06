using UnityEngine;
using UnityEngine.SceneManagement;

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
        //None = -1,

        /// <summary>
        /// 처음 인트로 화면
        /// </summary>
        //Intro = 0,

        /// <summary>
        /// 실제 인게임 플레이 화면 (무료 에셋 사용한 배경)
        /// </summary>
        PlayFreeAssets,

        /// <summary>
        /// 실제 인게임 플레이 화면 (유료 에셋 사용한 배경)
        /// </summary>
        //PlayPaidAssets,

        /// <summary>
        /// 사격 훈련장
        /// </summary>
        //Training
    }

    /// <summary>
    /// 씬을 불러들이고 관리한다.
    /// </summary>
    public class SceneController : MonoBehaviour
    {
        public static SceneController instance { get; private set; }

        /// <summary>
        /// 현재 실행 중인 씬의 이름
        /// </summary>
        //[SerializeField] private SceneName m_CurrentSceneName = SceneName.None;
        [SerializeField] private SceneName m_CurrentSceneName = SceneName.PlayFreeAssets;

        public SceneName CurrentScene
        {
            get { return m_CurrentSceneName; }
            set { m_CurrentSceneName = value; }
        }

        private void Awake()
        {
            instance = this;
            DontDestroyOnLoad(this);
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
            SceneManager.LoadScene(m_CurrentSceneName.ToString(), LoadSceneMode.Single);
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

        /// <summary>
        /// 불러온 씬이 이미 있는데 추가적으로 더 불러들이는 씬이 있다.
        /// </summary>
        /// <param name="name"></param>
        public void LoadSceneAddictive(SceneName name)
        {
            SceneManager.LoadScene(name.ToString(), LoadSceneMode.Additive);
        }
    }
}