using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DivisionLike
{
    public class PlayerInventory : MonoBehaviour
    {
        public Dictionary<string, Weapon> _dicWeapons = new Dictionary<string, Weapon>();

        public uint _currentMedikit = 5;
        public const uint _maxMedikit = 5;
        public uint _currentGrenade = 6;
        public const uint _maxGrenade = 6;

        private PlayerHealth _health;

        private void Awake()
        {
            _health = transform.GetComponent<PlayerHealth>();
        }

        public bool ObtainAllItems()
        {
            // TODO: v 키를 누르면 바닥에 떨어진 근처 아이템을 모두 줍는다
            return true;
        }

        public bool ObtainAmmo( int amount )
        {
            // TODO: 총알 위를 밟고 지나가면 바닥에 떨어진 총알을 줍는다
            Player.instance._weaponHandler.currentWeapon.ammo.carryingAmmo += amount;
            return true;
        }

        public Dictionary<string, Weapon> DropItems()
        {
            return _dicWeapons;  // 임시
        }

        public void ShowInventoryUI()
        {
            // TODO: 인벤토리 GUI 표시
        }

        public void UseMedikit()
        {
            if ( _currentMedikit <= 0 )
            {
                return;
            }

            Debug.Log( "use 1 medikit" );

            _currentMedikit--;
            
            _health.Recover();
        }

    }
}