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

namespace DivisionLike
{
    /// <summary>
    /// 폭파 범위 이펙트
    /// </summary>
    public class GrenadeCircleEffect : MonoBehaviour
    {
        public int m_Segments;
        public float m_xRadius;
        public float m_yRadius;
        LineRenderer m_Line;

        void Awake()
        {
            m_Line = gameObject.GetComponent<LineRenderer>();

            m_Line.positionCount = m_Segments + 1;
            m_Line.useWorldSpace = false;

            CreatePoints();
        }


        /// <summary>
        /// 슈류탄의 동그란 원을 만들기 위한 점을 위치시킨다.
        /// </summary>
        private void CreatePoints()
        {
            float x;
            float y;
            float z = 0f;

            float angle = 20f;

            for ( int i = 0; i < ( m_Segments + 1 ); i++ )
            {
                x = Mathf.Sin( Mathf.Deg2Rad * angle ) * m_xRadius;
                y = Mathf.Cos( Mathf.Deg2Rad * angle ) * m_yRadius;

                m_Line.SetPosition( i, new Vector3( x, y, z ) );

                angle += ( 360f / m_Segments );
            }
        }
    }
}


