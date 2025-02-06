using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using WanzyeeStudio;

namespace DivisionLike
{
    public class PopupManager : BaseSingleton<PopupManager>
    {
        [SerializeField] private Transform m_ParentCanvas;
        [SerializeField] private Transform m_ParentCanvasUpper;
        [SerializeField] private GameObject m_CommonPopupPrefab;
        [SerializeField] private GameObject m_ToastPopupPrefab;

        private CommonPopup m_CommonPopup;
        private ToastPopup m_ToastPopup;

        public CommonPopup ShowCommonPopup(string title, string content, string yes, string no,
            CommonPopup.ClickYes clickYes, CommonPopup.ClickNo clickNo)
        {
            GameObject newObj = Instantiate(m_CommonPopupPrefab) as GameObject;
            newObj.transform.SetParent(m_ParentCanvas);
            RectTransform rt = newObj.transform as RectTransform;
            rt.anchoredPosition = Vector3.zero;

            m_CommonPopup = newObj.GetComponent<CommonPopup>();
            m_CommonPopup.Setup(title, content, yes, no);
            m_CommonPopup.SetCallback(clickYes, clickNo);

            return m_CommonPopup;
        }

        public void CloseCommonPopup()
        {
            Destroy(m_CommonPopup.gameObject);
        }

        public ToastPopup ShowToastPopup(string message)
        {
            GameObject newObj = Instantiate(m_ToastPopupPrefab) as GameObject;
            newObj.transform.SetParent(m_ParentCanvasUpper);
            RectTransform rt = newObj.transform as RectTransform;
            rt.anchoredPosition = Vector3.zero;

            m_ToastPopup = newObj.GetComponent<ToastPopup>();
            m_ToastPopup.Setup(message);

            return m_ToastPopup;
        }
    }
}