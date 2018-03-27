using UnityEngine;
using UnityEditor;
using System.Collections;

namespace DivisionLike
{
    [CustomEditor( typeof( CameraControl ) )]
    public class CameraRigEditor : Editor
    {

        CameraControl cameraRig;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            cameraRig = (CameraControl) target;

            EditorGUILayout.LabelField( "Camera Helper" );

            if ( GUILayout.Button( "Save camera's position now." ) )
            {
                Camera cam = Camera.main;

                if ( cam )
                {
                    Transform camT = cam.transform;
                    Vector3 camPos = camT.localPosition;
                    Vector3 camRight = camPos;
                    Vector3 camLeft = camPos;
                    camLeft.x = -camPos.x;
                    cameraRig.m_CameraSettings.m_CamPositionOffsetRight = camRight;
                    cameraRig.m_CameraSettings.m_CamPositionOffsetLeft = camLeft;
                }
            }
        }
    }
}