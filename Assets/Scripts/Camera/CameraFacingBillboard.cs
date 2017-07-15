/*
 * reference - http://wiki.unity3d.com/index.php?title=CameraFacingBillboard
 */

using UnityEngine;
using System.Collections;


namespace DivisionLike
{
    public class CameraFacingBillboard : MonoBehaviour
    {
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        void Update()
        {
            transform.LookAt( transform.position + _camera.transform.rotation * Vector3.forward,
                _camera.transform.rotation * Vector3.up );
        }
    }
}