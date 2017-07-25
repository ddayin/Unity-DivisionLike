using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    public class HUDfollowPlayer : MonoBehaviour
    {
        private RectTransform _transform;

        private void Awake()
        {
            _transform = (RectTransform)transform;
        }
        
        // Update is called once per frame
        void Update()
        {
            FollowPlayer();
        }

        private Vector3 newPos = Vector3.zero;

        private void FollowPlayer()
        {
            //newPos = _transform.position;
            newPos.y = -50f + Player.instance.transform.localPosition.y;
            //Debug.Log( "newPos = " + newPos );
            //Debug.Log( "rect transform.localPosition.y = " + _transform.localPosition.y );
            _transform.localPosition = newPos;
            //_transform.position = newPos;
        }
    }
}
