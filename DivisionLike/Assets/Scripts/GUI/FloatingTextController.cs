using UnityEngine;
using System.Collections;


namespace DivisionLike
{
    /// <summary>
    /// 텍스트 처리
    /// </summary>
    public class FloatingTextController : MonoBehaviour
    {
        public FloatingText m_PopupText;
        private GameObject m_Canvas;

        private void Awake()
        {
            m_Canvas = this.gameObject;
        }

        /// <summary>
        /// 떠다니는 텍스트를 지정한 위치에 생성한다.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="location"></param>
        public void CreateFloatingText(string text, Transform location)
        {
            FloatingText instance = Instantiate(m_PopupText);

            instance.transform.SetParent(m_Canvas.transform, false);

            Vector3 newPos = new Vector3(location.position.x + Random.Range(-0.3f, 0.3f),
                location.position.y + Random.Range(-0.3f, 0.3f), location.position.z);
            instance.transform.position = newPos;
            instance.SetText(text);
            //instance.SetColor();
        }
    }
}