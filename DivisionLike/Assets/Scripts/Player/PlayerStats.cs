using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DivisionLike
{
    /// <summary>
    /// 플레이어 stat
    /// </summary>
    public class PlayerStats : MonoBehaviour
    {
        public int m_MaxHealth = 1200; // The amount of health the player starts the game with.
        public int m_CurrentHealth; // The current health the player has.
        public uint m_CurrentLevel = 1;
        public const uint m_MaxLevel = 30;
        public ulong m_CurrentXP = 0;
        public ulong[] m_XpRequire = new ulong[30];
        public uint m_CurrentArmor = 0;
        public float m_CriticalHitRate = 5f; // 치명타 확률
        public float m_CriticalHitMultiply = 1.8f; // 치명타 데미지 포인트에 곱할 숫자

        private void Awake()
        {
            m_CurrentHealth = m_MaxHealth;

            SetDummyxpRequire();
        }

        /// <summary>
        /// 최대치의 HP로 회복
        /// </summary>
        public void RecoverMaxHealth()
        {
            m_CurrentHealth = m_MaxHealth;
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetDummyxpRequire()
        {
            // FIXME: hard coding temporarily...

            m_XpRequire[0] = 1234;
            m_XpRequire[1] = 12345;
            m_XpRequire[2] = 123456;
            m_XpRequire[3] = 1234567;
            m_XpRequire[4] = 12345678;
            m_XpRequire[5] = 123456789;
            m_XpRequire[6] = 123456789;
            m_XpRequire[7] = 123456789;
            m_XpRequire[8] = 123456789;
            m_XpRequire[9] = 123456789;
            m_XpRequire[10] = 123456789;
            m_XpRequire[11] = 123456789;
            m_XpRequire[12] = 123456789;
            m_XpRequire[13] = 123456789;
            m_XpRequire[14] = 123456789;
            m_XpRequire[15] = 123456789;
            m_XpRequire[16] = 123456789;
            m_XpRequire[17] = 123456789;
            m_XpRequire[18] = 123456789;
            m_XpRequire[19] = 123456789;
            m_XpRequire[20] = 123456789;
            m_XpRequire[21] = 123456789;
            m_XpRequire[22] = 123456789;
            m_XpRequire[23] = 123456789;
            m_XpRequire[24] = 123456789;
            m_XpRequire[25] = 123456789;
            m_XpRequire[26] = 123456789;
            m_XpRequire[27] = 123456789;
            m_XpRequire[28] = 123456789;
            m_XpRequire[29] = 123456789;
        }

        /// <summary>
        /// 
        /// </summary>
        public void CheckLevel()
        {
            if (m_CurrentXP >= m_XpRequire[m_CurrentLevel - 1])
            {
                if (m_CurrentLevel < m_MaxLevel)
                {
                    m_CurrentLevel++;

                    m_CurrentXP = 0;

                    Debug.Log("[Level Up] Level = " + m_CurrentLevel + " xp = " + m_CurrentXP);

                    ScreenHUD.instance.SetLevelText();
                }
            }
        }

        /// <summary>
        /// 데미지 값을 계산한다.
        /// </summary>
        /// <returns></returns>
        public float CalculateDamage()
        {
            float damage = Player.instance.m_WeaponHandler.m_CurrentWeapon.m_WeaponSettings.damage;
            float result = Random.Range(0f, 100f);
            if (result < m_CriticalHitRate)
            {
                damage *= m_CriticalHitMultiply;
            }

            return damage;
        }
    }
}