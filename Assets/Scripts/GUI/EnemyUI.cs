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
    /// 적 캐릭터의 UI
    /// </summary>
    public class EnemyUI : MonoBehaviour
    {
        private Slider m_HealthSlider;
        private Canvas m_Canvas;
        private FloatingTextController m_FloatingTextController;
        private RectTransform m_Transform;
        private Transform m_Parent;

        private void Awake()
        {
            m_HealthSlider = transform.Find( "HealthUI/HealthSlider" ).GetComponent<Slider>();
            m_Canvas = transform.GetComponent<Canvas>();
            m_FloatingTextController = transform.GetComponent<FloatingTextController>();
            m_Transform = transform.GetComponent<RectTransform>();
            m_Parent = transform.parent;
        }

        private void OnEnable()
        {
            m_HealthSlider.normalizedValue = 1f;
        }

        private void Update()
        {
            ScaleIfClosed();
        }

        /// <summary>
        /// HP 바 설정
        /// </summary>
        /// <param name="health"></param>
        public void SetHealthSlider( int health )
        {
            m_HealthSlider.value = health;
        }

        /// <summary>
        /// 일정 숫자의 데미지 텍스트를 생성한다.
        /// </summary>
        /// <param name="amount"></param>
        public void CreateDamageText( string amount )
        {
            m_FloatingTextController.CreateFloatingText( amount, m_Canvas.transform );
        }

        /// <summary>
        /// 플레이어와 적 캐릭터의 거리가 가까우면 scale 조정을 한다.
        /// </summary>
        private void ScaleIfClosed()
        {
            float distance = Vector3.Distance( Player.instance.transform.position, m_Parent.transform.position );
            //Debug.Log( "distance = " + distance );
            if ( distance > 5f )
            {
                m_Transform.localScale = Vector3.one * 0.01f;
            }
            else
            {
                //_transform.localScale = Vector3.one * 0.01f * distance * 0.001f * Time.deltaTime;
                Vector3 newScale = Vector3.one * 0.01f;
                newScale.x = distance * 0.001f;
                newScale.y = distance * 0.001f;
                m_Transform.localScale = newScale;
            }
            
            //Debug.Log( "scale.x = " + m_Transform.localScale.x );
        }
        
    }
}

