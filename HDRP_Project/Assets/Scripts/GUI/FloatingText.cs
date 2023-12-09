using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DivisionLike
{
    /// <summary>
    /// 텍스트
    /// </summary>
    public class FloatingText : MonoBehaviour
    {
        public Animator m_Animator;
        private Text m_DamageText;

        void OnEnable()
        {
            AnimatorClipInfo[] clipInfo = m_Animator.GetCurrentAnimatorClipInfo(0);
            Destroy(gameObject, clipInfo[0].clip.length);
            m_DamageText = m_Animator.GetComponent<Text>();
        }

        /// <summary>
        /// 텍스트를 설정한다.
        /// </summary>
        /// <param name="text"></param>
        public void SetText(string text)
        {
            m_DamageText.text = text;
        }

        /// <summary>
        /// 크리티컬일 경우 빨간색으로 설정한다.
        /// </summary>
        /// <param name="isCritical"></param>
        public void SetColor(bool isCritical)
        {
            if (isCritical == true)
            {
                m_DamageText.color = Color.red;
            }
        }
    }
}