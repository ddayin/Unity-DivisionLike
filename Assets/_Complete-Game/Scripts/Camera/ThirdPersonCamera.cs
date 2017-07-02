/*
 * https://youtu.be/Ta7v27yySKs
 */


using UnityEngine;
using System.Collections;
 
public class ThirdPersonCamera : MonoBehaviour
{
    private const float Y_ANGLE_MIN = 0.0f;
    private const float Y_ANGLE_MAX = 50.0f;

    public Transform lookAt;
    public Transform cameraTransform;

    private Camera camera;

    private float distance = 15.0f;     // 카메라와 lookAt 간의 간격
    private float cameraHeight = 4.0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    public float sensitivityMouseX = 5.0f;  // 마우스 민감도
    public float sensitivityMouseY = 5.0f;  // 마우스 민감도
    private Vector3 cameraPosition = new Vector3( 0, 1, -16 );

    private void Start()
    {
        cameraTransform = transform;
        camera = Camera.main;
    }

    private void Update()
    {
        currentX = currentX + Input.GetAxis( "Mouse X" ) * sensitivityMouseX;
        currentY = currentY + Input.GetAxis( "Mouse Y" ) * sensitivityMouseY;

        //Debug.Log( "mouse X = " + currentX );
        //Debug.Log( "mouse Y = " + currentY );

        // mouse Y 값에 제한을 둬서 바닥 넘어서 보이지 않도록 Y 값 조정
        //currentY = Mathf.Clamp( currentY, Y_ANGLE_MIN, Y_ANGLE_MAX );
    }

    private void LateUpdate()
    {
        Quaternion rotation = Quaternion.Euler( currentY, currentX, 0 );
        cameraTransform.position = lookAt.position + rotation * cameraPosition;

        cameraTransform.LookAt( lookAt.position );
    }
}
