/*
 * reference - https://www.youtube.com/watch?v=gN1BmP4ONSQ&list=PLfxIz_UlKk7IwrcF2zHixNtFmh0lznXow&t=4140s&index=4
 */

using UnityEngine;
using System.Collections;
using System;

[RequireComponent( typeof( Animator ) )]
[RequireComponent( typeof( CharacterController ) )]
public class CharacterMovement : MonoBehaviour
{
    Animator animator;
    CharacterController characterController;

    [System.Serializable]
    public class AnimationSettings
    {
        public string verticalVelocityFloat = "Forward";
        public string horizontalVelocityFloat = "Strafe";
        public string groundedBool = "isGrounded";
        public string jumpBool = "isJumping";
    }
    [SerializeField]
    public AnimationSettings animations;

    [System.Serializable]
    public class PhysicsSettings
    {
        public float gravityModifier = 9.81f;
        public float baseGravity = 50.0f;
        public float resetGravityValue = 1.2f;
        public LayerMask groundLayers;
        public float airSpeed = 2.5f;
    }
    [SerializeField]
    public PhysicsSettings physics;

    [System.Serializable]
    public class MovementSettings
    {
        public float jumpSpeed = 6;
        public float jumpTime = 0.25f;
    }
    [SerializeField]
    public MovementSettings movement;

    Vector3 airControl;
    float forward;
    float strafe;
    bool jumping;
    bool resetGravity;
    float gravity;

    bool isGrounded()
    {
        RaycastHit hit;
        Vector3 start = transform.position + transform.up;
        Vector3 dir = Vector3.down;
        float radius = characterController.radius;
        if ( Physics.SphereCast( start, radius, dir, out hit, characterController.height / 2, physics.groundLayers ) )
        {
            return true;
        }

        return false;
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        SetupAnimator();
    }

    // Use this for initialization
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        AirControl( forward, strafe );
        ApplyGravity();
        //isGrounded = characterController.isGrounded;
    }

    //Animates the character and root motion handles the movement
    public void Animate( float forward, float strafe )
    {
        this.forward = forward;
        this.strafe = strafe;
        animator.SetFloat( animations.verticalVelocityFloat, forward );
        animator.SetFloat( animations.horizontalVelocityFloat, strafe );
        animator.SetBool( animations.groundedBool, isGrounded() );
        animator.SetBool( animations.jumpBool, jumping );
    }

    void AirControl( float forward, float strafe )
    {
        if ( isGrounded() == false )
        {
            airControl.x = strafe;
            airControl.z = forward;
            airControl = transform.TransformDirection( airControl );
            airControl *= physics.airSpeed;

            characterController.Move( airControl * Time.deltaTime );
        }
    }

    //Makes the character jump
    public void Jump()
    {
        if ( jumping )
            return;

        if ( isGrounded() )
        {
            jumping = true;
            StartCoroutine( StopJump() );
        }
    }

    //Stops us from jumping
    IEnumerator StopJump()
    {
        yield return new WaitForSeconds( movement.jumpTime );
        jumping = false;
    }

    //Applys downard force to the character when we aren't jumping
    void ApplyGravity()
    {
        if ( !isGrounded() )
        {
            if ( !resetGravity )
            {
                gravity = physics.resetGravityValue;
                resetGravity = true;
            }
            gravity += Time.deltaTime * physics.gravityModifier;
        }
        else
        {
            gravity = physics.baseGravity;
            resetGravity = false;
        }

        Vector3 gravityVector = new Vector3();

        if ( !jumping )
        {
            gravityVector.y -= gravity;
        }
        else
        {
            gravityVector.y = movement.jumpSpeed;
        }

        characterController.Move( gravityVector * Time.deltaTime );
    }

    //Setup the animator with the child avatar
    void SetupAnimator()
    {
        Animator wantedAnim = GetComponentsInChildren<Animator>()[ 1 ];
        Avatar wantedAvater = wantedAnim.avatar;

        animator.avatar = wantedAvater;
        Destroy( wantedAnim );
    }
}
