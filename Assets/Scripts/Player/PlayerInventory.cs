using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DivisionLike
{
    public class PlayerInventory : MonoBehaviour
    {
        public Dictionary<string, Weapon> dicWeapons = new Dictionary<string, Weapon>();

        public uint medikit;
        public const uint medikitMax = 5;
        public uint grenade;
        public const uint grenadeMax = 4;

        private PlayerHealth health;

        private void Awake()
        {
            health = transform.GetComponent<PlayerHealth>();
        }

        public bool ObtainAllItems()
        {
            // TODO: v 키를 누르면 바닥에 떨어진 근처 아이템을 모두 줍는다
            return true;
        }

        public bool ObtainAmmo( int amount )
        {
            // TODO: 총알 위를 밟고 지나가면 바닥에 떨어진 총알을 줍는다
            Player.instance.weaponHandler.currentWeapon.ammo.carryingAmmo += amount;
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

        public void UseMedikit()
        {
            if ( medikit <= 0 )
            {
                return;
            }

            Debug.Log( "use medikit" );

            medikit--;
            

            health.Recover();
        }

    }
}