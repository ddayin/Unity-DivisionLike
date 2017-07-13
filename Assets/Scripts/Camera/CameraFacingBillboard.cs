/*
 * reference - http://wiki.unity3d.com/index.php?title=CameraFacingBillboard
 */

using UnityEngine;
using System.Collections;


namespace DivisionLike
{
    public class CameraFacingBillboard : MonoBehaviour
    {
        public Camera m_Camera;

        private void Awake()
        {
            m_Camera = Camera.main;
        }

        void Update()
        {
            transform.LookAt( transform.position + m_Camera.transform.rotation * Vector3.forward,
                m_Camera.transform.rotation * Vector3.up );
        }
    }
}