using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DivisionLike
{
    public class PlayerInventory : MonoBehaviour
    {
        public Dictionary<string, Weapon> dicWeapons = new Dictionary<string, Weapon>();

        public bool ObtainAllItems()
        {
            // TODO: v 키를 누르면 바닥에 떨어진 근처 아이템을 모두 줍는다
            return true;
        }

        public bool ObtainAmmo()
        {
            // TODO: 총알 위를 밟고 지나가면 바닥에 떨어진 총알을 줍는다
            return true;
        }

        public Dictionary<string, Weapon> DropItems()
        {
            return dicWeapons;  // 임시
        }

        public void ShowInventoryUI()
        {
            // TODO: 인벤토리 GUI 표시
        }
    }
}