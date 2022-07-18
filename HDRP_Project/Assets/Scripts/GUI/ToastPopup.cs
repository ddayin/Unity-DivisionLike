using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    public class ToastPopup : MonoBehaviour
    {
        private Text m_TextMessage;
        private string m_Message;
        private float m_LifeTime = 1f;

        private void Awake()
        {
            m_TextMessage = transform.Find("Text").GetComponent<Text>();

            Invoke("Close", m_LifeTime);
        }

        public void Setup(string message)
        {
            m_Message = message;
            m_TextMessage.text = m_Message;
        }

        private void Close()
        {
            Destroy(gameObject);
        }
    }
}