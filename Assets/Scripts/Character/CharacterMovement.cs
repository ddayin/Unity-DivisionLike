/*
 * reference - https://www.youtube.com/watch?v=gN1BmP4ONSQ&list=PLfxIz_UlKk7IwrcF2zHixNtFmh0lznXow&t=4140s&index=4
 */

using UnityEngine;
using System.Collections;
using System;

namespace DivisionLike
{
    [RequireComponent( typeof( Animator ) )]
    [RequireComponent( typeof( CharacterController ) )]
    public class CharacterMovement : MonoBehaviour
    {
        private Animator _animator;
        private CharacterController _characterController;

        [System.Serializable]
        public class AnimationSettings
        {
            public string _verticalVelocityFloat = "Forward";
            public string _horizontalVelocityFloat = "Strafe";
            public string _groundedBool = "isGrounded";
            public string _jumpBool = "isJumping";
        }
        [SerializeField]
        public AnimationSettings _animationSettings;

        [System.Serializable]
        public class PhysicsSettings
        {
            public float _gravityModifier = 9.81f;
            public float _baseGravity = 50.0f;
            public float _resetGravityValue = 1.2f;
            public LayerMask _groundLayers;
            public float _airSpeed = 2.5f;
        }
        [SerializeField]
        public PhysicsSettings _physicsSettings;

        [System.Serializable]
        public class MovementSettings
        {
            public float _jumpSpeed = 6;
            public float _jumpTime = 0.25f;
        }
        [SerializeField]
        public MovementSettings _movementSettings;

        private Vector3 _airControl;
        private float _forward;
        private float _strafe;
        private bool _isJumping;
        private bool _isResetGravity;
        private float _gravity;


        void Awake()
        {
            _animator = GetComponent<Animator>();
            SetupAnimator();
        }

        // Use this for initialization
        void Start()
        {
            _characterController = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
            AirControl( _forward, _strafe );
            ApplyGravity();
            //isGrounded = characterController.isGrounded;
        }

        private bool isGrounded()
        {
            RaycastHit hit;
            Vector3 start = transform.position + transform.up;
            Vector3 dir = Vector3.down;
            float radius = _characterController.radius;
            if ( Physics.SphereCast( start, radius, dir, out hit, _characterController.height / 2, _physicsSettings._groundLayers ) == true )
            {
                return true;
            }

            return false;
        }

        //Animates the character and root motion handles the movement
        public void Animate( float forward, float strafe )
        {
            this._forward = forward;
            this._strafe = strafe;
            _animator.SetFloat( _animationSettings._verticalVelocityFloat, forward );
            _animator.SetFloat( _animationSettings._horizontalVelocityFloat, strafe );
            _animator.SetBool( _animationSettings._groundedBool, isGrounded() );
            _animator.SetBool( _animationSettings._jumpBool, _isJumping );
        }
        
        void AirControl( float forward, float strafe )
        {
            if ( isGrounded() == false )
            {
                _airControl.x = strafe;
                _airControl.z = forward;
                _airControl = transform.TransformDirection( _airControl );
                _airControl *= _physicsSettings._airSpeed;

                _characterController.Move( _airControl * Time.deltaTime );
            }
        }

        //Makes the character jump
        public void Jump()
        {
            if ( _isJumping == true )
                return;

            if ( isGrounded() == true )
            {
                _isJumping = true;
                StartCoroutine( StopJump() );
            }
        }

        //Stops us from jumping
        IEnumerator StopJump()
        {
            yield return new WaitForSeconds( _movementSettings._jumpTime );
            _isJumping = false;
        }

        //Applys downard force to the character when we aren't jumping
        private void ApplyGravity()
        {
            if ( isGrounded() == false )
            {
                if ( _isResetGravity == false )
                {
                    _gravity = _physicsSettings._resetGravityValue;
                    _isResetGravity = true;
                }
                _gravity += Time.deltaTime * _physicsSettings._gravityModifier;
            }
            else
            {
                _gravity = _physicsSettings._baseGravity;
                _isResetGravity = false;
            }

            Vector3 gravityVector = new Vector3();

            if ( _isJumping == false )
            {
                gravityVector.y -= _gravity;
            }
            else
            {
                gravityVector.y = _movementSettings._jumpSpeed;
            }

            _characterController.Move( gravityVector * Time.deltaTime );
        }

        //Setup the animator with the child avatar
        void SetupAnimator()
        {
            Animator wantedAnim = GetComponentsInChildren<Animator>()[ 1 ];
            Avatar wantedAvater = wantedAnim.avatar;

            _animator.avatar = wantedAvater;
            Destroy( wantedAnim );
        }
    }
}