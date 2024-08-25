using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DivisionLike
{
    /// <summary>
    /// 무기 처리
    /// </summary>
    public class WeaponHandler : MonoBehaviour
    {
        private Animator m_Animator;

        [System.Serializable]
        public class UserSettings
        {
            public Transform m_RightHand;
            public Transform m_PistolUnequipSpot;
            public Transform m_RifleUnequipSpot;
        }

        [SerializeField] public UserSettings m_UserSettings;

        [System.Serializable]
        public class Animations
        {
            public string m_WeaponTypeInt = "WeaponType";
            public string m_ReloadingBool = "isReloading";
            public string m_AimingBool = "Aiming";
        }

        [SerializeField] public Animations m_Animations;

        public Weapon m_CurrentWeapon; // 현재 사용하는 무기
        public List<Weapon> m_WeaponsList = new List<Weapon>(); // 보유하고 있는 무기들

        public int m_MaxWeapons = 2;
        public bool m_Aim { get; protected set; }
        public bool m_IsReloading = false; // 재장전 중인지
        private int m_WeaponType;
        private bool m_IsSettingWeapon;

        #region MonoBehaviour

        void Awake()
        {
            m_Animator = GetComponent<Animator>();
            SetupWeapons();
        }

        // Update is called once per frame
        void Update()
        {
            Animate();
        }

        #endregion

        /// <summary>
        /// 무기 설정
        /// </summary>
        private void SetupWeapons()
        {
            if (m_CurrentWeapon != null)
            {
                m_CurrentWeapon.SetEquipped(true);
                m_CurrentWeapon.SetOwner(this);
                AddWeaponToList(m_CurrentWeapon);

                if (m_CurrentWeapon.m_Ammo.clipAmmo <= 0)
                    Reload();

                if (m_IsReloading == true)
                    if (m_IsSettingWeapon == true)
                        m_IsReloading = false;
            }

            if (m_WeaponsList.Count > 0)
            {
                for (int i = 0; i < m_WeaponsList.Count; i++)
                {
                    if (m_WeaponsList[i] != m_CurrentWeapon)
                    {
                        m_WeaponsList[i].SetEquipped(false);
                        m_WeaponsList[i].SetOwner(this);
                    }
                }
            }
        }

        /// <summary>
        /// Animates the character
        /// </summary>
        private void Animate()
        {
            if (m_Animator == null) return;


            m_Animator.SetBool(m_Animations.m_AimingBool, m_Aim);
            m_Animator.SetBool(m_Animations.m_ReloadingBool, m_IsReloading);
            m_Animator.SetInteger(m_Animations.m_WeaponTypeInt, m_WeaponType);

            if (m_CurrentWeapon == null)
            {
                m_WeaponType = 0;
                return;
            }

            switch (m_CurrentWeapon.m_WeaponType)
            {
                case Weapon.WeaponType.Secondary:
                    m_WeaponType = 1;
                    break;
                case Weapon.WeaponType.Primary:
                    m_WeaponType = 2;
                    break;
                case Weapon.WeaponType.Sidearm:
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// Adds a weapon to the weaponsList
        /// </summary>
        /// <param name="weapon"></param>
        private void AddWeaponToList(Weapon weapon)
        {
            if (m_WeaponsList.Contains(weapon))
                return;

            m_WeaponsList.Add(weapon);
        }

        /// <summary>
        /// Puts the finger on the trigger and asks if we pulled
        /// </summary>
        /// <param name="aimRay"></param>
        public void FireCurrentWeapon(Ray aimRay)
        {
            if (m_CurrentWeapon.m_Ammo.clipAmmo == 0)
            {
                Reload();
                return;
            }

            m_CurrentWeapon.Fire(aimRay);
        }

        /// <summary>
        /// Reloads the current weapon
        /// </summary>
        public void Reload()
        {
            if (m_IsReloading == true || !m_CurrentWeapon)
                return;

            if (m_CurrentWeapon.m_Ammo.carryingAmmo <= 0 ||
                m_CurrentWeapon.m_Ammo.clipAmmo == m_CurrentWeapon.m_Ammo.maxClipAmmo)
                return;

            if (m_CurrentWeapon.m_SoundSettings.reloadSound != null)
            {
                if (m_CurrentWeapon.m_SoundSettings.audioS != null)
                {
                    SoundController.instance.PlaySound(m_CurrentWeapon.m_SoundSettings.audioS,
                        m_CurrentWeapon.m_SoundSettings.reloadSound, true, m_CurrentWeapon.m_SoundSettings.pitchMin,
                        m_CurrentWeapon.m_SoundSettings.pitchMax);
                }
            }

            m_IsReloading = true;
            ScreenHUD.instance.SetEnableReloadImage(m_IsReloading);

            StartCoroutine(StopReload());
        }

        /// <summary>
        /// Stops the reloading of the weapon
        /// </summary>
        /// <returns></returns>
        IEnumerator StopReload()
        {
            yield return new WaitForSeconds(m_CurrentWeapon.m_WeaponSettings.reloadDuration);

            m_CurrentWeapon.LoadClip();
            m_IsReloading = false;

            ScreenHUD.instance.SetAmmoSlider();
            ScreenHUD.instance.SetEnableReloadImage(m_IsReloading);
        }

        /// <summary>
        /// Sets out aim bool to be what we pass it
        /// </summary>
        /// <param name="aiming"></param>
        public void Aim(bool aiming)
        {
            m_Aim = aiming;
        }

        /// <summary>
        /// Drops the current weapon
        /// </summary>
        public void DropCurWeapon()
        {
            if (!m_CurrentWeapon)
                return;

            m_CurrentWeapon.SetEquipped(false);
            m_CurrentWeapon.SetOwner(null);
            m_WeaponsList.Remove(m_CurrentWeapon);
            m_CurrentWeapon = null;
        }

        /// <summary>
        /// Switches to the next weapon
        /// </summary>
        public void SwitchWeapons()
        {
            if (m_IsSettingWeapon || m_WeaponsList.Count == 0)
                return;

            if (m_CurrentWeapon != null)
            {
                int currentWeaponIndex = m_WeaponsList.IndexOf(m_CurrentWeapon);
                int nextWeaponIndex = (currentWeaponIndex + 1) % m_WeaponsList.Count;

                m_CurrentWeapon = m_WeaponsList[nextWeaponIndex];
            }
            else
            {
                m_CurrentWeapon = m_WeaponsList[0];
            }

            m_IsSettingWeapon = true;
            StartCoroutine(StopSettingWeapon());

            SetupWeapons();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public void SwitchWeapons(Weapon.WeaponType type)
        {
            if (m_IsSettingWeapon == true || m_WeaponsList.Count == 0)
            {
                return;
            }

            m_IsSettingWeapon = true;
            StartCoroutine(StopSettingWeapon());

            SetupWeapons();
        }

        /// <summary>
        /// Stops swapping weapons
        /// </summary>
        /// <returns></returns>
        IEnumerator StopSettingWeapon()
        {
            yield return new WaitForSeconds(0.7f);
            m_IsSettingWeapon = false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnAnimatorIK()
        {
            if (!m_Animator)
                return;

            if (m_CurrentWeapon && m_CurrentWeapon.m_UserSettings.leftHandIKTarget &&
                m_CurrentWeapon.m_WeaponType == Weapon.WeaponType.Primary
                && !m_IsReloading && !m_IsSettingWeapon)
            {
                m_Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                m_Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                Transform target = m_CurrentWeapon.m_UserSettings.leftHandIKTarget;
                Vector3 targetPos = target.position;
                Quaternion targetRot = target.rotation;
                m_Animator.SetIKPosition(AvatarIKGoal.LeftHand, targetPos);
                m_Animator.SetIKRotation(AvatarIKGoal.LeftHand, targetRot);
            }
            else
            {
                m_Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                m_Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            }
        }
    }
}