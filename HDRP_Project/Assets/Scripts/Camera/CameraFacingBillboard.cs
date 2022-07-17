using UnityEngine;
using System.Collections;


namespace DivisionLike
{
    /// <summary>
    /// billboard
    /// </summary>
    public class CameraFacingBillboard : MonoBehaviour
    {
        private Camera m_Camera;
        private Transform m_Transform;

        private void Awake()
        {
            m_Camera = Camera.main;
            m_Transform = transform;
        }

        void Update()
        {
            m_Transform.LookAt( transform.position + m_Camera.transform.rotation * Vector3.forward,
                m_Camera.transform.rotation * Vector3.up );
        }
    }
}