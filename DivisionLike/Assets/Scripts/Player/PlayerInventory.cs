using System.Collections.Generic;
using UnityEngine;

namespace DivisionLike
{
    /// <summary>
    /// 플레이어의 인벤토리
    /// </summary>
    public class PlayerInventory : MonoBehaviour
    {
        public Dictionary<string, Weapon> m_DicWeapons = new Dictionary<string, Weapon>();

        public uint m_CurrentMedikit = 5;
        public const uint m_MaxMedikit = 5;
        public uint m_CurrentGrenade = 6;
        public const uint m_MaxGrenade = 6;

        private PlayerHealth m_Health;

        private void Awake()
        {
            m_Health = transform.GetComponent<PlayerHealth>();
        }

        /// <summary>
        /// 모든 아이템들을 줍는다.
        /// </summary>
        /// <returns></returns>
        public bool ObtainAllItems()
        {
            // TODO: v 키를 누르면 바닥에 떨어진 근처 아이템을 모두 줍는다
            return true;
        }

        /// <summary>
        /// 지정된 양의 탄약을 얻는다.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool ObtainAmmo(int amount)
        {
            // TODO: 총알 위를 밟고 지나가면 바닥에 떨어진 총알을 줍는다
            Player.instance.m_WeaponHandler.m_CurrentWeapon.m_Ammo.carryingAmmo += amount;
            return true;
        }

        /// <summary>
        /// 아이템들을 떨어뜨린다.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Weapon> DropItems()
        {
            return m_DicWeapons; // 임시
        }

        /// <summary>
        /// 인벤토리 GUI를 표시한다.
        /// </summary>
        public void ShowInventoryUI()
        {
            // TODO: 인벤토리 GUI 표시
        }

        /// <summary>
        /// 약을 사용하여 한 칸의 체력을 회복한다.
        /// </summary>
        public void UseMedikit()
        {
            if (m_CurrentMedikit <= 0)
            {
                return;
            }

            Debug.Log("use 1 medikit");

            m_CurrentMedikit--;

            //_health.RecoverMax();
            m_Health.RecoverOneCell();
        }
    }
}