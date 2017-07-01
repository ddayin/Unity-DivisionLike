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

    private void Start()
    {
        cameraTransform = transform;
        camera = Camera.main;
    }

    private void Update()
    {
        currentX += Input.GetAxis( "Mouse X" );
        currentY += Input.GetAxis( "Mouse Y" );

        // mouse Y 값에 제한을 둬서 바닥 넘어서 보이지 않도록 Y 값 조정
        currentY = Mathf.Clamp( currentY, Y_ANGLE_MIN, Y_ANGLE_MAX );
    }

    private void LateUpdate()
    {
        Vector3 dir = new Vector3( 0, cameraHeight, -distance );
        Quaternion rotation = Quaternion.Euler( currentY, currentX, 0 );
        cameraTransform.position = lookAt.position + rotation * dir;

        cameraTransform.LookAt( lookAt.position );
    }
}
