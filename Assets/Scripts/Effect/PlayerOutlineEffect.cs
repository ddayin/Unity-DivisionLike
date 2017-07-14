using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

namespace DivisionLike
{
    public class PlayerOutlineEffect : MonoBehaviour
    {
        public Outline[] outlines = new Outline[ 5 ];

        private void Awake()
        {
            SetEnable( false );
        }

        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetEnable( bool enable )
        {
            for ( int i = 0; i < 5; i++ )
            {
                outlines[ i ].enabled = enable;
            }
        }

        public void Enable( float time )
        {
            SetEnable( true );

            Invoke( "Disable", time );
        }

        public void Disable()
        {
            SetEnable( false );
        }
    }
}