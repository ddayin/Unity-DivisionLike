using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DivisionLike
{
    /// <summary>
    /// 적 캐릭터의 인벤토리
    /// </summary>
    public class EnemyInventory : MonoBehaviour
    {
        private Dictionary<string, Weapon> m_DicWeapons = new Dictionary<string, Weapon>();

        public Dictionary<string, Weapon> DropItems()
        {
            return m_DicWeapons; // 임시
        }
    }
}