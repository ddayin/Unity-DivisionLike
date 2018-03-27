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
        private Animator m_Animator;
        private CharacterController m_CharacterController;

        [System.Serializable]
        public class AnimationSettings
        {
            public string m_VerticalVelocityFloat = "Forward";
            public string m_HorizontalVelocityFloat = "Strafe";
            public string m_GroundedBool = "isGrounded";
            public string m_JumpBool = "isJumping";
        }
        [SerializeField]
        public AnimationSettings m_AnimationSettings;

        [System.Serializable]
        public class PhysicsSettings
        {
            public float m_GravityModifier = 9.81f;
            public float m_BaseGravity = 50.0f;
            public float m_ResetGravityValue = 1.2f;
            public LayerMask m_GroundLayers;
            public float m_AirSpeed = 2.5f;
        }
        [SerializeField]
        public PhysicsSettings m_PhysicsSettings;

        [System.Serializable]
        public class MovementSettings
        {
            public float m_JumpSpeed = 6;
            public float m_JumpTime = 0.25f;
        }
        [SerializeField]
        public MovementSettings m_MovementSettings;

        private Vector3 m_AirControl;
        private float m_Forward;
        private float m_Strafe;
        private bool m_IsJumping;
        private bool m_IsResetGravity;
        private float m_Gravity;


        void Awake()
        {
            m_Animator = GetComponent<Animator>();
            SetupAnimator();
        }

        // Use this for initialization
        void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
            AirControl( m_Forward, m_Strafe );
            ApplyGravity();
            //isGrounded = characterController.isGrounded;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool isGrounded()
        {
            RaycastHit hit;
            Vector3 start = transform.position + transform.up;
            Vector3 dir = Vector3.down;
            float radius = m_CharacterController.radius;
            if ( Physics.SphereCast( start, radius, dir, out hit, m_CharacterController.height / 2, m_PhysicsSettings.m_GroundLayers ) == true )
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// Animates the character and root motion handles the movement
        /// </summary>
        /// <param name="forward"></param>
        /// <param name="strafe"></param>
        public void Animate( float forward, float strafe )
        {
            this.m_Forward = forward;
            this.m_Strafe = strafe;
            m_Animator.SetFloat( m_AnimationSettings.m_VerticalVelocityFloat, forward );
            m_Animator.SetFloat( m_AnimationSettings.m_HorizontalVelocityFloat, strafe );
            m_Animator.SetBool( m_AnimationSettings.m_GroundedBool, isGrounded() );
            m_Animator.SetBool( m_AnimationSettings.m_JumpBool, m_IsJumping );
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="forward"></param>
        /// <param name="strafe"></param>
        void AirControl( float forward, float strafe )
        {
            if ( isGrounded() == false )
            {
                m_AirControl.x = strafe;
                m_AirControl.z = forward;
                m_AirControl = transform.TransformDirection( m_AirControl );
                m_AirControl *= m_PhysicsSettings.m_AirSpeed;

                m_CharacterController.Move( m_AirControl * Time.deltaTime );
            }
        }


        /// <summary>
        /// Makes the character jump
        /// </summary>
        public void Jump()
        {
            if ( m_IsJumping == true )
                return;

            if ( isGrounded() == true )
            {
                m_IsJumping = true;
                StartCoroutine( StopJump() );
            }
        }


        /// <summary>
        /// Stops us from jumping
        /// </summary>
        /// <returns></returns>
        IEnumerator StopJump()
        {
            yield return new WaitForSeconds( m_MovementSettings.m_JumpTime );
            m_IsJumping = false;
        }


        /// <summary>
        /// Applys downard force to the character when we aren't jumping
        /// </summary>
        private void ApplyGravity()
        {
            if ( isGrounded() == false )
            {
                if ( m_IsResetGravity == false )
                {
                    m_Gravity = m_PhysicsSettings.m_ResetGravityValue;
                    m_IsResetGravity = true;
                }
                m_Gravity += Time.deltaTime * m_PhysicsSettings.m_GravityModifier;
            }
            else
            {
                m_Gravity = m_PhysicsSettings.m_BaseGravity;
                m_IsResetGravity = false;
            }

            Vector3 gravityVector = new Vector3();

            if ( m_IsJumping == false )
            {
                gravityVector.y -= m_Gravity;
            }
            else
            {
                gravityVector.y = m_MovementSettings.m_JumpSpeed;
            }

            m_CharacterController.Move( gravityVector * Time.deltaTime );
        }


        /// <summary>
        /// Setup the animator with the child avatar
        /// </summary>
        void SetupAnimator()
        {
            Animator wantedAnim = GetComponentsInChildren<Animator>()[ 1 ];
            Avatar wantedAvater = wantedAnim.avatar;

            m_Animator.avatar = wantedAvater;
            Destroy( wantedAnim );
        }
    }
}