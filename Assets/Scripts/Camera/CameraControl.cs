/*
 * reference - https://www.youtube.com/watch?v=gN1BmP4ONSQ&list=PLfxIz_UlKk7IwrcF2zHixNtFmh0lznXow&t=4140s&index=4
 */

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CameraControl : MonoBehaviour
{
    public Transform target = null;
    public bool autoTargetPlayer = true;
    public LayerMask wallLayers;

    public enum Shoulder
    {
        Right, Left
    }
    public Shoulder shoulder;

    [System.Serializable]
    public class CameraSettings
    {
        [Header( "-Positioning-" )]
        public Vector3 camPositionOffsetLeft;
        public Vector3 camPositionOffsetRight;

        [Header( "-Camera Options-" )]
        public Camera UICamera;
        public float mouseXSensitivity = 5.0f;
        public float mouseYSensitivity = 5.0f;
        public float minAngle = -30.0f;
        public float maxAngle = 70.0f;
        public float rotationSpeed = 5.0f;
        public float maxCheckDistance = 0.1f;

        [Header( "-Zoom-" )]
        public float fieldOfView = 70.0f;
        public float zoomFieldOfView = 30.0f;
        public float zoomSpeed = 3.0f;

        [Header( "-Visual Options-" )]
        public float hideMeshWhenDistance = 0.5f;
    }
    [SerializeField]
    public CameraSettings cameraSettings;

    [System.Serializable]
    public class InputSettings
    {
        public string verticalAxis = "Mouse X";
        public string horizontalAxis = "Mouse Y";
        public string aimButton = "Fire2";              // mouse right click
        public string switchShoulderButton = "Fire4";   // left shift button
    }
    [SerializeField]
    public InputSettings inputSettings;

    [System.Serializable]
    public class MovementSettings
    {
        public float movementLerpSpeed = 5.0f;
    }
    [SerializeField]
    public MovementSettings movementSettings;

    float newX = 0.0f;
    float newY = 0.0f;

    public Camera mainCamera { get; protected set; }
    public Transform pivot { get; set; }

    // Use this for initialization
    private void Start()
    {
        mainCamera = Camera.main;
        pivot = transform.GetChild( 0 );

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if ( target == null || Application.isPlaying == false )
            return;

        RotateCamera();
        CheckWall();
        CheckMeshRenderer();
        Zoom( Input.GetButton( inputSettings.aimButton ) );

        if ( Input.GetButtonDown( inputSettings.switchShoulderButton ) )
        {
            SwitchShoulders();
        }
    }

    private void LateUpdate()
    {
        if ( target == null )
        {
            TargetPlayer();
        }
        else
        {
            Vector3 targetPostion = target.position;
            Quaternion targetRotation = target.rotation;

            FollowTarget( targetPostion, targetRotation );
        }
    }

    // Finds the player gameObject and sets it as target
    void TargetPlayer()
    {
        if ( autoTargetPlayer == true )
        {
            GameObject player = GameObject.FindGameObjectWithTag( "Player" );

            if ( player != null )
            {
                Transform playerTransform = player.transform;
                target = playerTransform;
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
            Vector3 newPos = Vector3.Lerp( transform.position, targetPosition, Time.deltaTime * movementSettings.movementLerpSpeed );
            transform.position = newPos;
        }
    }

    // Rotates the camera with input
    private void RotateCamera()
    {
        if ( pivot == null )
            return;

        newX += cameraSettings.mouseXSensitivity * Input.GetAxis( inputSettings.verticalAxis );
        newY += cameraSettings.mouseYSensitivity * Input.GetAxis( inputSettings.horizontalAxis );

        Vector3 eulerAngleAxis = new Vector3();
        eulerAngleAxis.x = -newY;
        eulerAngleAxis.y = newX;

        newX = Mathf.Repeat( newX, 360 );
        newY = Mathf.Clamp( newY, cameraSettings.minAngle, cameraSettings.maxAngle );

        Quaternion newRotation = Quaternion.Slerp( pivot.localRotation, Quaternion.Euler( eulerAngleAxis ), Time.deltaTime * cameraSettings.rotationSpeed );

        pivot.localRotation = newRotation;
    }

    // Checks the wall and moves the camera up if we hit
    private void CheckWall()
    {
        if ( pivot == null || mainCamera == null )
            return;

        RaycastHit hit;

        Transform mainCamT = mainCamera.transform;
        Vector3 mainCamPos = mainCamT.position;
        Vector3 pivotPos = pivot.position;

        Vector3 start = pivotPos;
        Vector3 dir = mainCamPos - pivotPos;

        float dist = Mathf.Abs( shoulder == Shoulder.Left ? cameraSettings.camPositionOffsetLeft.z : cameraSettings.camPositionOffsetRight.z );

        if ( Physics.SphereCast( start, cameraSettings.maxCheckDistance, dir, out hit, dist, wallLayers ) )
        {
            MoveCamUp( hit, pivotPos, dir, mainCamT );
        }
        else
        {
            switch ( shoulder )
            {
                case Shoulder.Left:
                    PostionCamera( cameraSettings.camPositionOffsetLeft );
                    break;
                case Shoulder.Right:
                    PostionCamera( cameraSettings.camPositionOffsetRight );
                    break;
            }
        }
    }

    // This moves the camera forward when we hit a wall
    private void MoveCamUp( RaycastHit hit, Vector3 pivotPos, Vector3 dir, Transform cameraT )
    {
        float hitDist = hit.distance;
        Vector3 sphereCastCenter = pivotPos + ( dir.normalized * hitDist );
        cameraT.position = sphereCastCenter;
    }

    // Postions the cameras localPosition to a given location
    private void PostionCamera( Vector3 cameraPos )
    {
        if ( mainCamera == null )
            return;

        Transform mainCamT = mainCamera.transform;
        Vector3 mainCamPos = mainCamT.localPosition;
        Vector3 newPos = Vector3.Lerp( mainCamPos, cameraPos, Time.deltaTime * movementSettings.movementLerpSpeed );
        mainCamT.localPosition = newPos;
    }

    // Hides the mesh targets mesh renderers when too close
    private void CheckMeshRenderer()
    {
        if ( mainCamera == null || target == null )
            return;

        SkinnedMeshRenderer[] meshes = target.GetComponentsInChildren<SkinnedMeshRenderer>();
        Transform mainCamT = mainCamera.transform;
        Vector3 mainCamPos = mainCamT.position;
        Vector3 targetPos = target.position;
        float dist = Vector3.Distance( mainCamPos, ( targetPos + target.up ) );

        if ( meshes.Length > 0 )
        {
            for ( int i = 0; i < meshes.Length; i++ )
            {
                if ( dist <= cameraSettings.hideMeshWhenDistance )
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
    private void Zoom( bool isZooming )
    {
        if ( mainCamera == null )
            return;

        if ( isZooming == true )
        {
            float newFieldOfView = Mathf.Lerp( mainCamera.fieldOfView, cameraSettings.zoomFieldOfView, Time.deltaTime * cameraSettings.zoomSpeed );
            mainCamera.fieldOfView = newFieldOfView;

            if ( cameraSettings.UICamera != null )
            {
                cameraSettings.UICamera.fieldOfView = newFieldOfView;
            }
        }
        else
        {
            float originalFieldOfView = Mathf.Lerp( mainCamera.fieldOfView, cameraSettings.fieldOfView, Time.deltaTime * cameraSettings.zoomSpeed );
            mainCamera.fieldOfView = originalFieldOfView;

            if ( cameraSettings.UICamera != null )
            {
                cameraSettings.UICamera.fieldOfView = originalFieldOfView;
            }
        }
    }

    //Switches the cameras shoulder view
    private void SwitchShoulders()
    {
        switch ( shoulder )
        {
            case Shoulder.Left:
                shoulder = Shoulder.Right;
                break;
            case Shoulder.Right:
                shoulder = Shoulder.Left;
                break;
        }
    }
}
