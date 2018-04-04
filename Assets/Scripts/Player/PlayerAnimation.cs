using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DivisionLike
{
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator m_Animator;

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();

            if ( SceneController.instance.m_CurrentScene == eSceneName.Intro )
            {
                m_Animator.runtimeAnimatorController = Resources.Load( "Player/IntroPlayerAni" ) as RuntimeAnimatorController;
            }
        }

        
    }
}