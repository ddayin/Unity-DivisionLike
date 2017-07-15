/*
 * reference - https://www.assetstore.unity3d.com/kr/#!/content/25468
 */

using UnityEngine;

namespace DivisionLike
{
    public class TiltWindow : MonoBehaviour
    {
        public Vector2 _range = new Vector2( 5f, 3f );

        private Transform _transform;
        private Quaternion _startQuaternion;
        private Vector2 _rot = Vector2.zero;

        void Awake()
        {
            _transform = transform;
            _startQuaternion = _transform.localRotation;
        }

        void Update()
        {
            Vector3 pos = Input.mousePosition;

            float halfWidth = Screen.width * 0.5f;
            float halfHeight = Screen.height * 0.5f;
            float x = Mathf.Clamp( ( pos.x - halfWidth ) / halfWidth, -1f, 1f );
            float y = Mathf.Clamp( ( pos.y - halfHeight ) / halfHeight, -1f, 1f );
            _rot = Vector2.Lerp( _rot, new Vector2( x, y ), Time.deltaTime * 5f );

            _transform.localRotation = _startQuaternion * Quaternion.Euler( -_rot.y * _range.y, _rot.x * _range.x, 0f );
        }
    }
}