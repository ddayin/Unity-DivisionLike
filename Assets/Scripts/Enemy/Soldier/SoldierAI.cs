using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DivisionLike
{
    public enum SoldierState
    {
        Idle = 0,   // 대기
        Patrol,     // 순찰
        Follow,     // 타겟을 향해 이동
        Fire,       // 총 발사
        Reload,     // 총알 재장전
        Hit,        // 총에 맞았을 때
        Die         // 죽음
    }

    /// <summary>
    /// 
    /// </summary>
    [RequireComponent( typeof( Animator ) )]
    public class SoldierAI : MonoBehaviour
    {
        private NavMeshAgent m_NavMeshAgent;
        public SoldierState m_State = SoldierState.Idle;

        #region MonoBehaviour
        private void Awake()
        {
            m_NavMeshAgent = GetComponent<NavMeshAgent>();

            m_State = SoldierState.Idle;
            InitNavMeshAgent();
        }

        private void InitNavMeshAgent()
        {
            m_NavMeshAgent.speed = 0;
            m_NavMeshAgent.acceleration = 0;
            m_NavMeshAgent.autoBraking = false;
        }

        private void Start()
        {
            //m_NavMeshAgent.destination = Player.instance.transform.position;
        }

        private void Update()
        {
            switch ( m_State )
            {
                case SoldierState.Idle:
                    break;
                case SoldierState.Patrol:
                    Patrol();
                    break;

                case SoldierState.Follow:
                    Follow();
                    break;

                case SoldierState.Fire:
                    Fire();
                    break;

                case SoldierState.Reload:
                    break;

                case SoldierState.Hit:
                    break;
                case SoldierState.Die:
                    break;
                default:
                    break;
            }
        }
        #endregion

        private void Patrol()
        {

        }

        private void Follow()
        {
            if ( m_NavMeshAgent == null ) return;

            m_NavMeshAgent.destination = Player.instance.transform.position;
        }

        private void Fire()
        {

        }
    }
}


