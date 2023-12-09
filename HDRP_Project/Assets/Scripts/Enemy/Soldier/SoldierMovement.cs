using UnityEngine;
using System.Collections;
using System;

namespace DivisionLike
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CharacterController))]
    /// <summary>
    /// 적 캐릭터의 움직임
    /// </summary>
    public class SoldierMovement : MonoBehaviour
    {
        private Animator m_Animator;
        private CharacterController m_CharacterController;

        [System.Serializable]
        public class AnimationSettings
        {
            public string m_VerticalVelocityFloat = "Forward";
            public string m_HorizontalVelocityFloat = "Strafe";
        }

        [SerializeField] public AnimationSettings m_AnimationSettings; // 애니메이션 관련 세팅 값

        [System.Serializable]
        public class PhysicsSettings
        {
            public float m_BaseGravity = 50.0f;
            public float m_ResetGravityValue = 1.2f;
        }

        [SerializeField] public PhysicsSettings m_PhysicsSettings; // 물리 관련 세팅 값

        private float m_Forward;
        private float m_Strafe;
        private float m_Gravity;

        #region MonoBehaviour

        void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_CharacterController = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
            ApplyGravity();
        }

        #endregion

        /// <summary>
        /// Applys downard force to the character when we aren't jumping
        /// </summary>
        private void ApplyGravity()
        {
            m_Gravity = m_PhysicsSettings.m_BaseGravity;

            Vector3 gravityVector = new Vector3();

            gravityVector.y -= m_Gravity;

            m_CharacterController.Move(gravityVector * Time.deltaTime);
        }

        /// <summary>
        /// Animates the character and root motion handles the movement
        /// </summary>
        /// <param name="forward"></param>
        /// <param name="strafe"></param>
        public void Animate(float forward, float strafe)
        {
            m_Forward = forward;
            m_Strafe = strafe;

            m_Animator.SetFloat(m_AnimationSettings.m_VerticalVelocityFloat, forward);
            m_Animator.SetFloat(m_AnimationSettings.m_HorizontalVelocityFloat, strafe);
        }
    }
}