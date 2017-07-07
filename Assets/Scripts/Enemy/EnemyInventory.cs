using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DivisionLike
{
    public class EnemyInventory : MonoBehaviour
    {
        public Dictionary<string, Weapon> dicWeapons = new Dictionary<string, Weapon>();
        
        public Dictionary<string, Weapon> DropItems()
        {
            return dicWeapons;  // 임시
        }
    }
}