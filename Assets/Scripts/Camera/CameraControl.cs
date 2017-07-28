/*
 * reference - https://www.youtube.com/watch?v=gN1BmP4ONSQ&list=PLfxIz_UlKk7IwrcF2zHixNtFmh0lznXow&t=4140s&index=4
 */

using UnityEngine;
using System.Collections;

namespace DivisionLike
{
    [ExecuteInEditMode]

    public class CameraControl : MonoBehaviour
    {
        public static CameraControl instance = null;

        public Transform _target = null;
        public bool _autoTargetPlayer = true;
        public LayerMask _wallLayers;

        public enum Shoulder
        {
            Right, Left
        }
        public Shoulder _shoulder;

        [System.Serializable]
        public class CameraSettings
        {
            [Header( "-Positioning-" )]
            public Vector3 _camPositionOffsetLeft;
            public Vector3 _camPositionOffsetRight;

            [Header( "-Camera Options-" )]
            public Camera _UICamera;
            public float _mouseXSensitivity = 5.0f;
            public float _mouseYSensitivity = 5.0f;
            public float _minAngle = -30.0f;
            public float _maxAngle = 70.0f;
            public float _rotationSpeed = 5.0f;
            public float _maxCheckDistance = 0.1f;

            [Header( "-Zoom-" )]
            public float _fieldOfView = 70.0f;
            public float _zoomFieldOfView = 30.0f;
            public float _zoomMoreFieldOfView = 12.0f;
            public float _zoomSpeed = 3.0f;

            [Header( "-Visual Options-" )]
            public float _hideMeshWhenDistance = 0.5f;
        }
        [SerializeField]
        public CameraSettings _cameraSettings;

        [System.Serializable]
        public class InputSettings
        {
            public string _verticalAxis = "Mouse X";
            public string _horizontalAxis = "Mouse Y";
            public string _aimButton = "Fire2";              // mouse right click
            public string _zoomMoreButton = "Tab";              // tab key
            public string _switchShoulderButton = "Sprint";   // left shift button
        }
        [SerializeField]
        public InputSettings _inputSettings;

        [System.Serializable]
        public class MovementSettings
        {
            public float _movementLerpSpeed = 5.0f;
        }
        [SerializeField]
        public MovementSettings _movementSettings;

        private float _newX = 0.0f;
        private float _newY = 0.0f;

        public Camera _mainCamera { get; protected set; }
        public Transform _pivot { get; set; }
        
        private bool _isZooming = false;
        private bool _isZoomingMore = false;

        // Use this for initialization
        private void Awake()
        {
            instance = this;
            _mainCamera = Camera.main;
            _pivot = transform.GetChild( 0 );
        }

        

        // Update is called once per frame
        private void Update()
        {
            if ( _target == null || Application.isPlaying == false )
                return;

            RotateCamera();
            CheckWall();
            CheckMeshRenderer();

            if ( Input.GetButton( _inputSettings._aimButton ) == true )
            {
                _isZooming = true;

                if ( Input.GetButtonDown( _inputSettings._zoomMoreButton ) == true )
                {
                    _isZoomingMore = !_isZoomingMore;
                }
            }
            else
            {
                _isZooming = false;
            }

            Zoom();
            ZoomMore();
            
            if ( Player.instance._userInput._isAiming == true )
            {
                if ( Input.GetButtonDown( _inputSettings._switchShoulderButton ) )
                {
                    SwitchShoulders();
                }
            }
            
        }

        private void LateUpdate()
        {
            if ( _target == null )
            {
                TargetPlayer();
            }
            else
            {
                Vector3 targetPostion = _target.position;
                Quaternion targetRotation = _target.rotation;

                FollowTarget( targetPostion, targetRotation );
            }
        }
        
        // Finds the player gameObject and sets it as target
        void TargetPlayer()
        {
            if ( _autoTargetPlayer == true )
            {
                GameObject player = GameObject.FindGameObjectWithTag( "Player" );

                if ( player != null )
                {
                    Transform playerTransform = player.transform;
                    _target = playerTransform;
                }
            }
        }

        // Following the target with Time.deltaTime smoothly
        private void FollowTarget( Vector3 targetPosition, Quaternion targetRotation )
        {
            if ( Application.isPlaying == false )
            {
                transform.position = targetPosition;
                transform.rotation = targetRotation;
            }
            else
            {
                Vector3 newPos = Vector3.Lerp( transform.position, targetPosition, Time.deltaTime * _movementSettings._movementLerpSpeed );
                transform.position = newPos;
            }
        }

        // Rotates the camera with input
        private void RotateCamera()
        {
            if ( _pivot == null )
                return;

            if ( Player.instance._weaponHandler._isReloading == true )
            {
                return;
            }
            
            _newX += _cameraSettings._mouseXSensitivity * Input.GetAxis( _inputSettings._verticalAxis );
            _newY += _cameraSettings._mouseYSensitivity * Input.GetAxis( _inputSettings._horizontalAxis );

            if ( Player.instance._userInput._isFiring == true )
            {
                _newY = _newY + Player.instance._weaponHandler.currentWeapon.weaponSettings._goUpSpeed * Time.deltaTime;
            }

            Vector3 eulerAngleAxis = new Vector3();
            eulerAngleAxis.x = -_newY;
            
            eulerAngleAxis.y = _newX;
            

            _newX = Mathf.Repeat( _newX, 360 );
            _newY = Mathf.Clamp( _newY, _cameraSettings._minAngle, _cameraSettings._maxAngle );

            Quaternion newRotation = Quaternion.Slerp( _pivot.localRotation, Quaternion.Euler( eulerAngleAxis ), Time.deltaTime * _cameraSettings._rotationSpeed );
            
            _pivot.localRotation = newRotation;
        }

        // Checks the wall and moves the camera up if we hit
        private void CheckWall()
        {
            if ( _pivot == null || _mainCamera == null )
                return;

            RaycastHit hit;

            Transform mainCamT = _mainCamera.transform;
            Vector3 mainCamPos = mainCamT.position;
            Vector3 pivotPos = _pivot.position;

            Vector3 start = pivotPos;
            Vector3 dir = mainCamPos - pivotPos;

            float dist = Mathf.Abs( _shoulder == Shoulder.Left ? _cameraSettings._camPositionOffsetLeft.z : _cameraSettings._camPositionOffsetRight.z );

            if ( Physics.SphereCast( start, _cameraSettings._maxCheckDistance, dir, out hit, dist, _wallLayers ) )
            {
                MoveCameraUp( hit, pivotPos, dir, mainCamT );
            }
            else
            {
                switch ( _shoulder )
                {
                    case Shoulder.Left:
                        PostionCamera( _cameraSettings._camPositionOffsetLeft );
                        break;
                    case Shoulder.Right:
                        PostionCamera( _cameraSettings._camPositionOffsetRight );
                        break;
                }
            }
        }

        // This moves the camera forward when we hit a wall
        private void MoveCameraUp( RaycastHit hit, Vector3 pivotPos, Vector3 dir, Transform cameraT )
        {
            float hitDist = hit.distance;
            Vector3 sphereCastCenter = pivotPos + ( dir.normalized * hitDist );
            cameraT.position = sphereCastCenter;
        }

        // Postions the cameras localPosition to a given location
        private void PostionCamera( Vector3 cameraPos )
        {
            if ( _mainCamera == null )
                return;

            Transform mainCamT = _mainCamera.transform;
            Vector3 mainCamPos = mainCamT.localPosition;
            Vector3 newPos = Vector3.Lerp( mainCamPos, cameraPos, Time.deltaTime * _movementSettings._movementLerpSpeed );
            mainCamT.localPosition = newPos;
        }

        // Hides the mesh targets mesh renderers when too close
        private void CheckMeshRenderer()
        {
            if ( _mainCamera == null || _target == null )
                return;

            SkinnedMeshRenderer[] meshes = _target.GetComponentsInChildren<SkinnedMeshRenderer>();
            Transform mainCamT = _mainCamera.transform;
            Vector3 mainCamPos = mainCamT.position;
            Vector3 targetPos = _target.position;
            float dist = Vector3.Distance( mainCamPos, ( targetPos + _target.up ) );

            if ( meshes.Length > 0 )
            {
                for ( int i = 0; i < meshes.Length; i++ )
                {
                    if ( dist <= _cameraSettings._hideMeshWhenDistance )
                    {
                        meshes[ i ].enabled = false;
                    }
                    else
                    {
                        meshes[ i ].enabled = true;
                    }
                }
            }
        }

        //Zooms the camera in and out
        private void Zoom()
        {
            if ( _mainCamera == null )
                return;

            if ( _isZooming == true )
            {
                float newFieldOfView = Mathf.Lerp( _mainCamera.fieldOfView, _cameraSettings._zoomFieldOfView, Time.deltaTime * _cameraSettings._zoomSpeed );
                _mainCamera.fieldOfView = newFieldOfView;

                //if ( _cameraSettings._UICamera != null )
                //{
                //    _cameraSettings._UICamera.fieldOfView = newFieldOfView;
                //}
            }
            else
            {
                float originalFieldOfView = Mathf.Lerp( _mainCamera.fieldOfView, _cameraSettings._fieldOfView, Time.deltaTime * _cameraSettings._zoomSpeed );
                _mainCamera.fieldOfView = originalFieldOfView;

                //if ( _cameraSettings._UICamera != null )
                //{
                //    _cameraSettings._UICamera.fieldOfView = originalFieldOfView;
                //}
            }
        }

        
        private void ZoomMore()
        {
            if ( _mainCamera == null )
            {
                return;
            }

            if ( _isZooming == false )
            {
                return;
            }
            
            if ( _isZoomingMore == true )
            {
                float newFieldOfView = Mathf.Lerp( _mainCamera.fieldOfView, _cameraSettings._zoomMoreFieldOfView, Time.deltaTime * _cameraSettings._zoomSpeed );
                _mainCamera.fieldOfView = newFieldOfView;
                
            }
            else
            {
                float originalFieldOfView = Mathf.Lerp( _mainCamera.fieldOfView, _cameraSettings._zoomFieldOfView, Time.deltaTime * _cameraSettings._zoomSpeed );
                _mainCamera.fieldOfView = originalFieldOfView;
                
            }
            
        }

        //Switches the cameras shoulder view
        private void SwitchShoulders()
        {
            switch ( _shoulder )
            {
                case Shoulder.Left:
                    _shoulder = Shoulder.Right;
                    break;
                case Shoulder.Right:
                    _shoulder = Shoulder.Left;
                    break;
            }
        }
    }
}