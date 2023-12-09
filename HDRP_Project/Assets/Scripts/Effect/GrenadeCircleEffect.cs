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

            for (int i = 0; i < (m_Segments + 1); i++)
            {
                x = Mathf.Sin(Mathf.Deg2Rad * angle) * m_xRadius;
                y = Mathf.Cos(Mathf.Deg2Rad * angle) * m_yRadius;

                m_Line.SetPosition(i, new Vector3(x, y, z));

                angle += (360f / m_Segments);
            }
        }
    }
}