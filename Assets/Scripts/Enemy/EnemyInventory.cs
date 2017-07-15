using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DivisionLike
{
    public class EnemyInventory : MonoBehaviour
    {
        private Dictionary<string, Weapon> _dicWeapons = new Dictionary<string, Weapon>();
        
        public Dictionary<string, Weapon> DropItems()
        {
            return _dicWeapons;  // 임시
        }
    }
}