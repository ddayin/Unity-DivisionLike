using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DivisionLike
{
    public class IntroScene : MonoBehaviour
    {
        private void Update()
        {
            ProcessKeyInput();
        }

        private void ProcessKeyInput()
        {
            if ( Input.GetKeyDown( KeyCode.Escape ) == true )
            {
#if UNITY_EDITOR

#else
                Application.Quit();
#endif
            }

            if ( Input.GetKeyDown( KeyCode.Space ) == true || Input.GetKeyDown( KeyCode.Return ) == true )
            {
                SceneController.instance.LoadScene( eSceneName.Play );
            }
        }
    }
}