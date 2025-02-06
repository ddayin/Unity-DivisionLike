using UnityEngine;
using System.Collections;

namespace DivisionLike
{
    /// <summary>
    /// 카메라 조작
    /// </summary>
    [ExecuteInEditMode] // 스크립트가 에디터모드에서 동작하도록 설정
    public class CameraControl : MonoBehaviour
    {
        public static CameraControl instance { get; private set; }

        public Transform m_Target = null; // 카메라가 쳐다볼 타겟
        public bool m_IsAutoTargetPlayer = true; // 카메라가 플레이어를 자동으로 쳐다볼 타겟
        public LayerMask m_WallLayers; // 벽의 레이어 이름

        public enum Shoulder
        {
            Right,
            Left
        }

        public Shoulder m_Shoulder; // 조준 시, 왼쪽 어깨로 볼지, 아니면 오른쪽 어깨로 볼지 설정한다.

        [System.Serializable]
        public class CameraSettings
        {
            [Header("-Positioning-")] public Vector3 m_CamPositionOffsetLeft;
            public Vector3 m_CamPositionOffsetRight;

            [Header("-Camera Options-")] public Camera m_UICamera; // UI 용 카메라
            public float m_MouseXSensitivity = 5.0f; // 마우스 감도
            public float m_MouseYSensitivity = 5.0f; // 마우스 감도
            public float m_MinAngle = -30.0f;
            public float m_MaxAngle = 70.0f;
            public float m_RotationSpeed = 5.0f; // 회전 속도
            public float m_MaxCheckDistance = 0.1f; // 

            [Header("-Zoom-")] public float m_FieldOfView = 70.0f; // field of view
            public float m_ZoomFieldOfView = 30.0f; // 확대했을 때의 field of view
            public float m_ZoomMoreFieldOfView = 12.0f; // 좀 더 확대했을 때의 field of view
            public float m_ZoomSpeed = 3.0f; // 확대 속도

            [Header("-Visual Options-")] public float m_HideMeshWhenDistance = 0.5f; // 타겟의 메쉬를 숨길 때의 거리
        }

        [SerializeField] public CameraSettings m_CameraSettings; // 카메라 세팅 값들

        [System.Serializable]
        public class InputSettings
        {
            public string m_VerticalAxis = "Mouse X"; // 마우스 위치
            public string m_HorizontalAxis = "Mouse Y"; // 마우스 위치
            public string m_AimButton = "Fire2"; // mouse right click
            public string m_ZoomMoreButton = "Tab"; // tab key
            public string m_SwitchShoulderButton = "Sprint"; // left shift button
        }

        [SerializeField] public InputSettings m_InputSettings; // 입력 값들

        [System.Serializable]
        public class MovementSettings
        {
            public float m_MovementLerpSpeed = 5.0f; // 카메라 이동 속도
        }

        [SerializeField] public MovementSettings m_MovementSettings; // 이동 관련 세팅 값들

        private float m_NewX = 0.0f;
        private float m_NewY = 0.0f;

        public Camera m_MainCamera { get; protected set; } // main 카메라
        public Transform m_Pivot { get; set; } // main 카메라 아래에 있는 피봇

        private bool m_IsZooming = false; // 확대를 하고 있는지
        private bool m_IsZoomingMore = false; // 더 확대를 하고 있는지

        #region MonoBehaviour

        private void Awake()
        {
            instance = this;

            m_MainCamera = Camera.main;
            m_Pivot = transform.GetChild(0);
        }

        private void Update()
        {
            if (m_Target == null || Application.isPlaying == false)
                return;

            RotateCamera();
            CheckWall();
            CheckMeshRenderer();

            if (Input.GetButton(m_InputSettings.m_AimButton) == true)
            {
                m_IsZooming = true;

                if (Input.GetButtonDown(m_InputSettings.m_ZoomMoreButton) == true)
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

            if (Player.instance.m_UserInput.m_IsAiming == true)
            {
                if (Input.GetButtonDown(m_InputSettings.m_SwitchShoulderButton))
                {
                    SwitchShoulders();
                }
            }
        }

        private void LateUpdate()
        {
            if (m_Target == null)
            {
                TargetPlayer();
            }
            else
            {
                Vector3 targetPostion = m_Target.position;
                Quaternion targetRotation = m_Target.rotation;

                FollowTarget(targetPostion, targetRotation);
            }
        }

        #endregion

        /// <summary>
        /// Finds the player gameObject and sets it as target
        /// </summary>
        void TargetPlayer()
        {
            if (m_IsAutoTargetPlayer == true)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");

                if (player != null)
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
        private void FollowTarget(Vector3 targetPosition, Quaternion targetRotation)
        {
            if (Application.isPlaying == false)
            {
                transform.position = targetPosition;
                transform.rotation = targetRotation;
            }
            else
            {
                Vector3 newPos = Vector3.Lerp(transform.position, targetPosition,
                    Time.deltaTime * m_MovementSettings.m_MovementLerpSpeed);
                transform.position = newPos;
            }
        }


        /// <summary>
        /// Rotates the camera with input
        /// </summary>
        private void RotateCamera()
        {
            if (m_Pivot == null) return;

            //if ( Player.instance.m_WeaponHandler.m_IsReloading == true ) return;

            m_NewX += m_CameraSettings.m_MouseXSensitivity * Input.GetAxis(m_InputSettings.m_VerticalAxis);
            m_NewY += m_CameraSettings.m_MouseYSensitivity * Input.GetAxis(m_InputSettings.m_HorizontalAxis);

            if (Player.instance.m_UserInput.m_IsFiring == true)
            {
                m_NewY = m_NewY + Player.instance.m_WeaponHandler.m_CurrentWeapon.m_WeaponSettings._goUpSpeed *
                    Time.deltaTime;
            }

            Vector3 eulerAngleAxis = new Vector3();
            eulerAngleAxis.x = -m_NewY;
            eulerAngleAxis.y = m_NewX;

            m_NewX = Mathf.Repeat(m_NewX, 360);
            m_NewY = Mathf.Clamp(m_NewY, m_CameraSettings.m_MinAngle, m_CameraSettings.m_MaxAngle);

            Quaternion newRotation = Quaternion.Slerp(m_Pivot.localRotation, Quaternion.Euler(eulerAngleAxis),
                Time.deltaTime * m_CameraSettings.m_RotationSpeed);

            m_Pivot.localRotation = newRotation;
        }


        /// <summary>
        /// Checks the wall and moves the camera up if we hit
        /// </summary>
        private void CheckWall()
        {
            if (m_Pivot == null || m_MainCamera == null)
                return;

            RaycastHit hit;

            Transform mainCamT = m_MainCamera.transform;
            Vector3 mainCamPos = mainCamT.position;
            Vector3 pivotPos = m_Pivot.position;

            Vector3 start = pivotPos;
            Vector3 dir = mainCamPos - pivotPos;

            float dist = Mathf.Abs(m_Shoulder == Shoulder.Left
                ? m_CameraSettings.m_CamPositionOffsetLeft.z
                : m_CameraSettings.m_CamPositionOffsetRight.z);

            if (Physics.SphereCast(start, m_CameraSettings.m_MaxCheckDistance, dir, out hit, dist, m_WallLayers))
            {
                MoveCameraUp(hit, pivotPos, dir, mainCamT);
            }
            else
            {
                switch (m_Shoulder)
                {
                    case Shoulder.Left:
                        PositionCamera(m_CameraSettings.m_CamPositionOffsetLeft);
                        break;
                    case Shoulder.Right:
                        PositionCamera(m_CameraSettings.m_CamPositionOffsetRight);
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
        private void MoveCameraUp(RaycastHit hit, Vector3 pivotPos, Vector3 dir, Transform cameraT)
        {
            float hitDist = hit.distance;
            Vector3 sphereCastCenter = pivotPos + (dir.normalized * hitDist);
            cameraT.position = sphereCastCenter;
        }


        /// <summary>
        /// Postions the cameras localPosition to a given location
        /// </summary>
        /// <param name="cameraPos"></param>
        private void PositionCamera(Vector3 cameraPos)
        {
            if (m_MainCamera == null)
                return;

            Transform mainCamT = m_MainCamera.transform;
            Vector3 mainCamPos = mainCamT.localPosition;
            Vector3 newPos = Vector3.Lerp(mainCamPos, cameraPos,
                Time.deltaTime * m_MovementSettings.m_MovementLerpSpeed);
            mainCamT.localPosition = newPos;
        }


        /// <summary>
        /// Hides the mesh targets mesh renderers when too close
        /// </summary>
        private void CheckMeshRenderer()
        {
            if (m_MainCamera == null || m_Target == null)
                return;

            SkinnedMeshRenderer[] meshes = m_Target.GetComponentsInChildren<SkinnedMeshRenderer>();
            Transform mainCamT = m_MainCamera.transform;
            Vector3 mainCamPos = mainCamT.position;
            Vector3 targetPos = m_Target.position;
            float dist = Vector3.Distance(mainCamPos, (targetPos + m_Target.up));

            if (meshes.Length > 0)
            {
                for (int i = 0; i < meshes.Length; i++)
                {
                    if (dist <= m_CameraSettings.m_HideMeshWhenDistance)
                    {
                        meshes[i].enabled = false;
                    }
                    else
                    {
                        meshes[i].enabled = true;
                    }
                }
            }
        }


        /// <summary>
        /// Zooms the camera in and out
        /// </summary>
        private void Zoom()
        {
            if (m_MainCamera == null)
                return;

            if (m_IsZooming == true)
            {
                float newFieldOfView = Mathf.Lerp(m_MainCamera.fieldOfView, m_CameraSettings.m_ZoomFieldOfView,
                    Time.deltaTime * m_CameraSettings.m_ZoomSpeed);
                m_MainCamera.fieldOfView = newFieldOfView;

                //if ( _cameraSettings._UICamera != null )
                //{
                //    _cameraSettings._UICamera.fieldOfView = newFieldOfView;
                //}
            }
            else
            {
                float originalFieldOfView = Mathf.Lerp(m_MainCamera.fieldOfView, m_CameraSettings.m_FieldOfView,
                    Time.deltaTime * m_CameraSettings.m_ZoomSpeed);
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
            if (m_MainCamera == null)
            {
                return;
            }

            if (m_IsZooming == false)
            {
                return;
            }

            if (m_IsZoomingMore == true)
            {
                float newFieldOfView = Mathf.Lerp(m_MainCamera.fieldOfView, m_CameraSettings.m_ZoomMoreFieldOfView,
                    Time.deltaTime * m_CameraSettings.m_ZoomSpeed);
                m_MainCamera.fieldOfView = newFieldOfView;
            }
            else
            {
                float originalFieldOfView = Mathf.Lerp(m_MainCamera.fieldOfView, m_CameraSettings.m_ZoomFieldOfView,
                    Time.deltaTime * m_CameraSettings.m_ZoomSpeed);
                m_MainCamera.fieldOfView = originalFieldOfView;
            }
        }


        /// <summary>
        /// Switches the cameras shoulder view
        /// </summary>
        private void SwitchShoulders()
        {
            switch (m_Shoulder)
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