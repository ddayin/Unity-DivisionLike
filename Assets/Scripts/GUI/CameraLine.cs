using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DivisionLike
{
    public class CameraLine : MonoBehaviour
    {
        private LineRenderer _line;

        private void Awake()
        {
            _line = transform.GetComponent<LineRenderer>();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            _line.SetPosition( 0, Camera.main.transform.position );

            Player.instance._userInput.
        }
    }
}


