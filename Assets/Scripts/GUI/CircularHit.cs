using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    public class CircularHit : MonoBehaviour
    {
        private Image _hitImage;

        private void Awake()
        {
            _hitImage = transform.Find("HitImage").GetComponent<Image>();
            _hitImage.enabled = false;
        }

        public void RotateHit( Vector3 direction )
        {
            _hitImage.enabled = true;

            float angle = Vector3.Angle( Camera.main.transform.forward, direction );
            
            _hitImage.transform.rotation = Quaternion.Euler( 0, 0, -angle );

            Invoke( "DisableImage", 1f );
        }

        private void DisableImage()
        {
            _hitImage.enabled = false;
        }
    }
}


