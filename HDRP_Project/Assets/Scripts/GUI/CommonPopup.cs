using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    public class CommonPopup : MonoBehaviour
    {
        private Text m_TextTitle;
        private Text m_TextContent;
        private Button m_ButtonYes;
        private Text m_TextYes;
        private Button m_ButtonNo;
        private Text m_TextNo;

        private string m_Title;
        private string m_Content;
        private string m_Yes;
        private string m_No;

        public delegate void ClickYes();

        public delegate void ClickNo();

        private ClickYes m_ClickYes = null;
        private ClickNo m_ClickNo = null;

        private void Awake()
        {
            m_TextTitle = transform.Find("Text_Title").GetComponent<Text>();
            m_TextContent = transform.Find("Text_Content").GetComponent<Text>();
            m_ButtonYes = transform.Find("Button_Yes").GetComponent<Button>();
            m_TextYes = m_ButtonYes.transform.Find("Text").GetComponent<Text>();
            m_ButtonNo = transform.Find("Button_No").GetComponent<Button>();
            m_TextNo = m_ButtonNo.transform.Find("Text").GetComponent<Text>();

            m_ButtonYes.onClick.AddListener(OnClickYes);
            m_ButtonNo.onClick.AddListener(OnClickNo);
        }

        public void Setup(string title, string content, string yes, string no)
        {
            m_Title = title;
            m_Content = content;
            m_Yes = yes;
            m_No = no;

            m_TextTitle.text = m_Title;
            m_TextContent.text = m_Content;
            m_TextYes.text = m_Yes;
            m_TextNo.text = m_No;
        }

        public void SetCallback(ClickYes yesCallback, ClickNo noCallback)
        {
            m_ClickYes = yesCallback;
            m_ClickNo = noCallback;
        }

        private void OnClickYes()
        {
            m_ClickYes();
        }

        private void OnClickNo()
        {
            m_ClickNo();
        }
    }
}