using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    /// <summary>
    /// 적 캐릭터의 스탯
    /// </summary>
    public class EnemyStats : MonoBehaviour
    {
        public int m_CurrentLevel; // 현재 레벨
        public int m_MaxLevel; // 최고 레벨
        public int m_CurrentArmor; // 현재 방어력
        public int m_XpWhenDie; // 죽었을 때 경험치
    }
}