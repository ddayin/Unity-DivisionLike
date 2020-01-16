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
            m_IconImage = transform.Find( "Canvas/IconImage" ).GetComponent<Image>();
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
            if ( m_AmmoBox.m_State == AmmoBox.AmmoBoxState.Emtpy )
            {
                m_IconImage.gameObject.SetActive( false );
            }
        }
    }
}