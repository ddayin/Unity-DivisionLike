using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DivisionLike
{
    public class GrenadeCircleEffect : MonoBehaviour
    {
        public int _segments;
        public float _xRadius;
        public float _yRadius;
        LineRenderer _line;

        void Awake()
        {
            _line = gameObject.GetComponent<LineRenderer>();

            _line.SetVertexCount( _segments + 1 );
            _line.useWorldSpace = false;

            CreatePoints();
        }


        private void CreatePoints()
        {
            float x;
            float y;
            float z = 0f;

            float angle = 20f;

            for ( int i = 0; i < ( _segments + 1 ); i++ )
            {
                x = Mathf.Sin( Mathf.Deg2Rad * angle ) * _xRadius;
                y = Mathf.Cos( Mathf.Deg2Rad * angle ) * _yRadius;

                _line.SetPosition( i, new Vector3( x, y, z ) );

                angle += ( 360f / _segments );
            }
        }
    }
}


