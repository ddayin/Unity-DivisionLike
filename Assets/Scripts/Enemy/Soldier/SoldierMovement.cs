/*
MIT License

Copyright (c) 2020 ddayin

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

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
        [SerializeField]
        public AnimationSettings m_AnimationSettings;   // 애니메이션 관련 세팅 값

        [System.Serializable]
        public class PhysicsSettings
        {
            public float m_BaseGravity = 50.0f;
            public float m_ResetGravityValue = 1.2f;
        }
        [SerializeField]
        public PhysicsSettings m_PhysicsSettings;       // 물리 관련 세팅 값

        private float m_Forward;
        private float m_Strafe;
        private bool m_IsResetGravity;
        private float m_Gravity;

        #region MonoBehaviour
        void Awake()
        {
            if ( SceneController.instance.m_CurrentScene == eSceneName.Intro ) return;

            m_Animator = GetComponent<Animator>();
            m_CharacterController = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
            if ( SceneController.instance.m_CurrentScene == eSceneName.Intro ) return;

            ApplyGravity();
        }
        #endregion

        /// <summary>
        /// Applys downard force to the character when we aren't jumping
        /// </summary>
        private void ApplyGravity()
        {
            m_Gravity = m_PhysicsSettings.m_BaseGravity;
            m_IsResetGravity = false;

            Vector3 gravityVector = new Vector3();

            gravityVector.y -= m_Gravity;

            m_CharacterController.Move( gravityVector * Time.deltaTime );
        }

        /// <summary>
        /// Animates the character and root motion handles the movement
        /// </summary>
        /// <param name="forward"></param>
        /// <param name="strafe"></param>
        public void Animate( float forward, float strafe )
        {
            m_Forward = forward;
            m_Strafe = strafe;

            m_Animator.SetFloat( m_AnimationSettings.m_VerticalVelocityFloat, forward );
            m_Animator.SetFloat( m_AnimationSettings.m_HorizontalVelocityFloat, strafe );
        }

    }
}