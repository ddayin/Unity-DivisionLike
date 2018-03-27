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

        public Transform m_Target = null;
        public bool m_IsAutoTargetPlayer = true;
        public LayerMask m_WallLayers;

        public enum Shoulder
        {
            Right, Left
        }
        public Shoulder m_Shoulder;

        [System.Serializable]
        public class CameraSettings
        {
            [Header( "-Positioning-" )]
            public Vector3 m_CamPositionOffsetLeft;
            public Vector3 m_CamPositionOffsetRight;

            [Header( "-Camera Options-" )]
            public Camera m_UICamera;
            public float m_MouseXSensitivity = 5.0f;
            public float m_MouseYSensitivity = 5.0f;
            public float m_MinAngle = -30.0f;
            public float m_MaxAngle = 70.0f;
            public float m_RotationSpeed = 5.0f;
            public float m_MaxCheckDistance = 0.1f;

            [Header( "-Zoom-" )]
            public float m_FieldOfView = 70.0f;
            public float m_ZoomFieldOfView = 30.0f;
            public float m_ZoomMoreFieldOfView = 12.0f;
            public float m_ZoomSpeed = 3.0f;

            [Header( "-Visual Options-" )]
            public float m_HideMeshWhenDistance = 0.5f;
        }
        [SerializeField]
        public CameraSettings m_CameraSettings;

        [System.Serializable]
        public class InputSettings
        {
            public string m_VerticalAxis = "Mouse X";
            public string m_HorizontalAxis = "Mouse Y";
            public string m_AimButton = "Fire2";              // mouse right click
            public string m_ZoomMoreButton = "Tab";              // tab key
            public string m_SwitchShoulderButton = "Sprint";   // left shift button
        }
        [SerializeField]
        public InputSettings m_InputSettings;

        [System.Serializable]
        public class MovementSettings
        {
            public float m_MovementLerpSpeed = 5.0f;
        }
        [SerializeField]
        public MovementSettings m_MovementSettings;

        private float m_NewX = 0.0f;
        private float m_NewY = 0.0f;

        public Camera m_MainCamera { get; protected set; }
        public Transform m_Pivot { get; set; }
        
        private bool m_IsZooming = false;
        private bool m_IsZoomingMore = false;

        // Use this for initialization
        private void Awake()
        {
            instance = this;
            m_MainCamera = Camera.main;
            m_Pivot = transform.GetChild( 0 );
        }

        

        // Update is called once per frame
        private void Update()
        {
            if ( m_Target == null || Application.isPlaying == false )
                return;

            RotateCamera();
            CheckWall();
            CheckMeshRenderer();

            if ( Input.GetButton( m_InputSettings.m_AimButton ) == true )
            {
                m_IsZooming = true;

                if ( Input.GetButtonDown( m_InputSettings.m_ZoomMoreButton ) == true )
                {
                    m_IsZoomingMore = !m_IsZoomingMore;
                }
            }
            else
            {
                m_IsZooming = false;
            }

            Zoom();
            ZoomMore();
            
            if ( Player.instance.m_UserInput.m_IsAiming == true )
            {
                if ( Input.GetButtonDown( m_InputSettings.m_SwitchShoulderButton ) )
                {
                    SwitchShoulders();
                }
            }
            
        }

        private void LateUpdate()
        {
            if ( m_Target == null )
            {
                TargetPlayer();
            }
            else
            {
                Vector3 targetPostion = m_Target.position;
                Quaternion targetRotation = m_Target.rotation;

                FollowTarget( targetPostion, targetRotation );
            }
        }


        /// <summary>
        /// Finds the player gameObject and sets it as target
        /// </summary>
        void TargetPlayer()
        {
            if ( m_IsAutoTargetPlayer == true )
            {
                GameObject player = GameObject.FindGameObjectWithTag( "Player" );

                if ( player != null )
                {
                    Transform playerTransform = player.transform;
                    m_Target = playerTransform;
                }
            }
        }


        /// <summary>
        /// Following the target with Time.deltaTime smoothly
        /// </summary>
        /// <param name="targetPosition"></param>
        /// <param name="targetRotation"></param>
        private void FollowTarget( Vector3 targetPosition, Quaternion targetRotation )
        {
            if ( Application.isPlaying == false )
            {
                transform.position = targetPosition;
                transform.rotation = targetRotation;
            }
            else
            {
                Vector3 newPos = Vector3.Lerp( transform.position, targetPosition, Time.deltaTime * m_MovementSettings.m_MovementLerpSpeed );
                transform.position = newPos;
            }
        }


        /// <summary>
        /// Rotates the camera with input
        /// </summary>
        private void RotateCamera()
        {
            if ( m_Pivot == null )
                return;

            if ( Player.instance.m_WeaponHandler.m_isReloading == true )
            {
                return;
            }
            
            m_NewX += m_CameraSettings.m_MouseXSensitivity * Input.GetAxis( m_InputSettings.m_VerticalAxis );
            m_NewY += m_CameraSettings.m_MouseYSensitivity * Input.GetAxis( m_InputSettings.m_HorizontalAxis );

            if ( Player.instance.m_UserInput.m_IsFiring == true )
            {
                m_NewY = m_NewY + Player.instance.m_WeaponHandler.m_CurrentWeapon.m_WeaponSettings._goUpSpeed * Time.deltaTime;
            }

            Vector3 eulerAngleAxis = new Vector3();
            eulerAngleAxis.x = -m_NewY;
            
            eulerAngleAxis.y = m_NewX;
            

            m_NewX = Mathf.Repeat( m_NewX, 360 );
            m_NewY = Mathf.Clamp( m_NewY, m_CameraSettings.m_MinAngle, m_CameraSettings.m_MaxAngle );

            Quaternion newRotation = Quaternion.Slerp( m_Pivot.localRotation, Quaternion.Euler( eulerAngleAxis ), Time.deltaTime * m_CameraSettings.m_RotationSpeed );
            
            m_Pivot.localRotation = newRotation;
        }


        /// <summary>
        /// Checks the wall and moves the camera up if we hit
        /// </summary>
        private void CheckWall()
        {
            if ( m_Pivot == null || m_MainCamera == null )
                return;

            RaycastHit hit;

            Transform mainCamT = m_MainCamera.transform;
            Vector3 mainCamPos = mainCamT.position;
            Vector3 pivotPos = m_Pivot.position;

            Vector3 start = pivotPos;
            Vector3 dir = mainCamPos - pivotPos;

            float dist = Mathf.Abs( m_Shoulder == Shoulder.Left ? m_CameraSettings.m_CamPositionOffsetLeft.z : m_CameraSettings.m_CamPositionOffsetRight.z );

            if ( Physics.SphereCast( start, m_CameraSettings.m_MaxCheckDistance, dir, out hit, dist, m_WallLayers ) )
            {
                MoveCameraUp( hit, pivotPos, dir, mainCamT );
            }
            else
            {
                switch ( m_Shoulder )
                {
                    case Shoulder.Left:
                        PostionCamera( m_CameraSettings.m_CamPositionOffsetLeft );
                        break;
                    case Shoulder.Right:
                        PostionCamera( m_CameraSettings.m_CamPositionOffsetRight );
                        break;
                }
            }
        }


        /// <summary>
        /// This moves the camera forward when we hit a wall
        /// </summary>
        /// <param name="hit"></param>
        /// <param name="pivotPos"></param>
        /// <param name="dir"></param>
        /// <param name="cameraT"></param>
        private void MoveCameraUp( RaycastHit hit, Vector3 pivotPos, Vector3 dir, Transform cameraT )
        {
            float hitDist = hit.distance;
            Vector3 sphereCastCenter = pivotPos + ( dir.normalized * hitDist );
            cameraT.position = sphereCastCenter;
        }


        /// <summary>
        /// Postions the cameras localPosition to a given location
        /// </summary>
        /// <param name="cameraPos"></param>
        private void PostionCamera( Vector3 cameraPos )
        {
            if ( m_MainCamera == null )
                return;

            Transform mainCamT = m_MainCamera.transform;
            Vector3 mainCamPos = mainCamT.localPosition;
            Vector3 newPos = Vector3.Lerp( mainCamPos, cameraPos, Time.deltaTime * m_MovementSettings.m_MovementLerpSpeed );
            mainCamT.localPosition = newPos;
        }


        /// <summary>
        /// Hides the mesh targets mesh renderers when too close
        /// </summary>
        private void CheckMeshRenderer()
        {
            if ( m_MainCamera == null || m_Target == null )
                return;

            SkinnedMeshRenderer[] meshes = m_Target.GetComponentsInChildren<SkinnedMeshRenderer>();
            Transform mainCamT = m_MainCamera.transform;
            Vector3 mainCamPos = mainCamT.position;
            Vector3 targetPos = m_Target.position;
            float dist = Vector3.Distance( mainCamPos, ( targetPos + m_Target.up ) );

            if ( meshes.Length > 0 )
            {
                for ( int i = 0; i < meshes.Length; i++ )
                {
                    if ( dist <= m_CameraSettings.m_HideMeshWhenDistance )
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


        /// <summary>
        /// Zooms the camera in and out
        /// </summary>
        private void Zoom()
        {
            if ( m_MainCamera == null )
                return;

            if ( m_IsZooming == true )
            {
                float newFieldOfView = Mathf.Lerp( m_MainCamera.fieldOfView, m_CameraSettings.m_ZoomFieldOfView, Time.deltaTime * m_CameraSettings.m_ZoomSpeed );
                m_MainCamera.fieldOfView = newFieldOfView;

                //if ( _cameraSettings._UICamera != null )
                //{
                //    _cameraSettings._UICamera.fieldOfView = newFieldOfView;
                //}
            }
            else
            {
                float originalFieldOfView = Mathf.Lerp( m_MainCamera.fieldOfView, m_CameraSettings.m_FieldOfView, Time.deltaTime * m_CameraSettings.m_ZoomSpeed );
                m_MainCamera.fieldOfView = originalFieldOfView;

                //if ( _cameraSettings._UICamera != null )
                //{
                //    _cameraSettings._UICamera.fieldOfView = originalFieldOfView;
                //}
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ZoomMore()
        {
            if ( m_MainCamera == null )
            {
                return;
            }

            if ( m_IsZooming == false )
            {
                return;
            }
            
            if ( m_IsZoomingMore == true )
            {
                float newFieldOfView = Mathf.Lerp( m_MainCamera.fieldOfView, m_CameraSettings.m_ZoomMoreFieldOfView, Time.deltaTime * m_CameraSettings.m_ZoomSpeed );
                m_MainCamera.fieldOfView = newFieldOfView;
                
            }
            else
            {
                float originalFieldOfView = Mathf.Lerp( m_MainCamera.fieldOfView, m_CameraSettings.m_ZoomFieldOfView, Time.deltaTime * m_CameraSettings.m_ZoomSpeed );
                m_MainCamera.fieldOfView = originalFieldOfView;
                
            }
            
        }


        /// <summary>
        /// Switches the cameras shoulder view
        /// </summary>
        private void SwitchShoulders()
        {
            switch ( m_Shoulder )
            {
                case Shoulder.Left:
                    m_Shoulder = Shoulder.Right;
                    break;
                case Shoulder.Right:
                    m_Shoulder = Shoulder.Left;
                    break;
            }
        }
    }
}