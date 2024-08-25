using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    /// <summary>
    /// 탄약 상자 아이콘
    /// </summary>
    public class AmmoBoxIcon : MonoBehaviour
    {
        private AmmoBox m_AmmoBox;
        private Image m_IconImage;

        private void Awake()
        {
            m_AmmoBox = transform.GetComponent<AmmoBox>();
            m_IconImage = transform.Find("Canvas/IconImage").GetComponent<Image>();
        }


        // Update is called once per frame
        void Update()
        {
            CheckIfEmpty();
        }

        /// <summary>
        /// 비어있으면 아이콘 이미지를 비활성화한다.
        /// </summary>
        private void CheckIfEmpty()
        {
            if (m_AmmoBox.m_State == AmmoBox.AmmoBoxState.Emtpy)
            {
                m_IconImage.gameObject.SetActive(false);
            }
        }
    }
}