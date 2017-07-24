using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    public class CircularHit : MonoBehaviour
    {
        private Image _hitImage;
        private Transform _centerTransform;

        private void Awake()
        {
            _hitImage = transform.Find("HitImage").GetComponent<Image>();
            _centerTransform = transform.Find( "Center" ).transform;
        }
        
        // Update is called once per frame
        void Update()
        {
            

        }

        public void RotateHit( Vector3 direction )
        {
            Vector3 newDir = Vector3.zero;
            newDir.x = direction.x;
            newDir.y = direction.z;
            newDir.z = direction.y;

            float angle = Vector3.Angle( transform.forward, newDir );
            Debug.Log( "RotateHit() angle = " + angle );

            _hitImage.transform.RotateAround( _centerTransform.position, new Vector3( 0, 0, 1 ), angle );
        }
    }
}


