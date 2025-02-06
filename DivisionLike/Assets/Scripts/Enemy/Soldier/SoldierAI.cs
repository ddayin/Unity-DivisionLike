using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DivisionLike
{
    public enum SoldierState
    {
        Idle = 0, // 대기
        Patrol, // 순찰
        Follow, // 타겟을 향해 이동
        Fire, // 총 발사
        Reload, // 총알 재장전
        Hit, // 총에 맞았을 때
        Die // 죽음
    }

    /// <summary>
    /// 적 캐릭터의 인공지능
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class SoldierAI : MonoBehaviour
    {
        private NavMeshAgent m_NavMeshAgent;
        private WeaponHandler m_WeaponHandler;
        private Camera m_TPSCamera;
        private Transform m_TargetToFire;
        private Animator m_Animator;

        public SoldierState m_State = SoldierState.Idle;

        #region MonoBehaviour

        private void Awake()
        {
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
            m_WeaponHandler = GetComponent<WeaponHandler>();
            m_TPSCamera = Camera.main;
            m_Animator = GetComponent<Animator>();

            m_State = SoldierState.Idle;
            //m_State = SoldierState.Fire;
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
            switch (m_State)
            {
                case SoldierState.Idle:
                    Idle();
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

        private void Idle()
        {
        }

        private void Patrol()
        {
        }

        private void Follow()
        {
            if (m_NavMeshAgent == null) return;

            m_NavMeshAgent.destination = Player.instance.transform.position;
        }

        #region 총 발사

        private void Fire()
        {
            if (m_WeaponHandler.m_CurrentWeapon == null) return;

            float fireRate = m_WeaponHandler.m_CurrentWeapon.m_WeaponSettings.fireRate;

            InvokeRepeating("FireWeapon", 0f, fireRate);
        }

        private void FindTargetToFire()
        {
            m_TargetToFire = Player.instance.gameObject.transform;
        }

        private void FireWeapon()
        {
            FindTargetToFire();

            //Ray aimRay = m_TPSCamera.ViewportPointToRay( new Vector3( 0.5f, 0.5f, 0f ) );
            Ray aimRay = new Ray(transform.position, transform.forward);

            Debug.DrawRay(aimRay.origin, aimRay.direction);

            m_WeaponHandler.FireCurrentWeapon(aimRay);
        }

        #endregion
    }
}