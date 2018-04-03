using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DivisionLike
{
    public class Enemy : MonoBehaviour
    {
        [HideInInspector] public EnemyAttack m_Attack;
        [HideInInspector] public EnemyDropItem m_DropItem;
        [HideInInspector] public EnemyHealth m_Health;
        [HideInInspector] public EnemyInventory m_Inventory;
        [HideInInspector] public EnemyMovement m_Movement;
        [HideInInspector] public EnemyStats m_Stats;

        private void Awake()
        {
            m_Attack = GetComponent<EnemyAttack>();
            m_DropItem = GetComponent<EnemyDropItem>();
            m_Health = GetComponent<EnemyHealth>();
            m_Inventory = GetComponent<EnemyInventory>();
            m_Movement = GetComponent<EnemyMovement>();
            m_Stats = GetComponent<EnemyStats>();
        }
    }
}