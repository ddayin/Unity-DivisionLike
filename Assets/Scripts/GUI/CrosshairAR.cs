/*
MIT License

Copyright (c) 2019 ddayin

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
    /// AR 총의 crosshair
    /// </summary>
    public class CrosshairAR : CrosshairHandler
    {
        public Image[] m_CrosshairImages;
        private float m_WalkSize;

        private void Awake()
        {
            gameObject.SetActive( false );

            m_WalkSize = m_CrosshairImages[ 0 ].rectTransform.localPosition.y;
        }

        private void OnEnable()
        {
            m_WalkSize = 10f;

        }

        /// <summary>
        /// 색상을 변경한다.
        /// </summary>
        /// <param name="color"></param>
        public override void ChangeColor( Color color )
        {
            //Debug.Log( "CrosshairAR.ChangeColor() overrided" );
            for ( int i = 0; i < 4; i++ )
            {
                m_CrosshairImages[ i ].color = color;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateCrosshair()
        {
            // y+ x+ x- y- 
            float crossHairSize = calculateCrossHair();

            m_CrosshairImages[ 0 ].rectTransform.localPosition = Vector3.Slerp( m_CrosshairImages[ 0 ].rectTransform.localPosition, new Vector3( 0f, crossHairSize, 0f ), Time.deltaTime * 8f );
            m_CrosshairImages[ 1 ].rectTransform.localPosition = Vector3.Slerp( m_CrosshairImages[ 1 ].rectTransform.localPosition, new Vector3( crossHairSize, 0f, 0f ), Time.deltaTime * 8f );
            m_CrosshairImages[ 2 ].rectTransform.localPosition = Vector3.Slerp( m_CrosshairImages[ 2 ].rectTransform.localPosition, new Vector3( -crossHairSize, 0f, 0f ), Time.deltaTime * 8f );
            m_CrosshairImages[ 3 ].rectTransform.localPosition = Vector3.Slerp( m_CrosshairImages[ 3 ].rectTransform.localPosition, new Vector3( 0f, -crossHairSize, 0f ), Time.deltaTime * 8f );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public float calculateCrossHair()
        {
            float size = m_WalkSize * Player.instance.m_WeaponHandler.m_CurrentWeapon.m_WeaponSettings.crossHairSize;

            if ( Player.instance.m_UserInput.m_IsSprinting == true )
            {
                size *= 2;
            }
            else
            {
                size /= 2;
            }

            return size;
        }
        
    }
}