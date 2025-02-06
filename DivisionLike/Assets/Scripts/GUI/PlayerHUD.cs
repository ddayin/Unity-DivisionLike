using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    /// <summary>
    /// 플레이어의 HUD
    /// </summary>
    public class PlayerHUD : MonoBehaviour
    {
        public static PlayerHUD instance { get; private set; }

        private Slider[] m_HealthSlider = new Slider[3];

        private Text m_ClipAmmoText;
        private Text m_CarryingAmmoText;
        private Text m_AnotherAmmoText;

        private Text m_MedikitNumberText;
        private Text m_GrenadeNumberText;

        private WeaponHandler m_WeaponHandler;
        private Weapon.WeaponType anotherType = Weapon.WeaponType.Primary;

        #region MonoBehaviour

        void Awake()
        {
            instance = this;

            m_HealthSlider[0] = transform.Find("PlayerHUD/HealthUI/HealthSlider_0").GetComponent<Slider>();
            m_HealthSlider[1] = transform.Find("PlayerHUD/HealthUI/HealthSlider_1").GetComponent<Slider>();
            m_HealthSlider[2] = transform.Find("PlayerHUD/HealthUI/HealthSlider_2").GetComponent<Slider>();

            m_ClipAmmoText = transform.Find("PlayerHUD/Ammo/ClipAmmoText").GetComponent<Text>();
            m_CarryingAmmoText = transform.Find("PlayerHUD/Ammo/CarryingAmmoText").GetComponent<Text>();
            m_AnotherAmmoText = transform.Find("PlayerHUD/Ammo/AnotherAmmoText").GetComponent<Text>();

            m_MedikitNumberText = transform.Find("PlayerHUD/Medikit/NumberText").GetComponent<Text>();
            m_GrenadeNumberText = transform.Find("PlayerHUD/Grenade/NumberText").GetComponent<Text>();
        }

        private void Start()
        {
            m_WeaponHandler = Player.instance.m_WeaponHandler;

            InitHealthSlider();
            SetAnotherWeapon();
            SetAmmoText();
            SetMedikitText();
            SetGrenadeText();
        }

        // Update is called once per frame
        void Update()
        {
            SetAmmoText();
        }

        #endregion

        /// <summary>
        /// HP 바 초기화
        /// </summary>
        private void InitHealthSlider()
        {
            float toDivide = (float)Player.instance.m_Stats.m_MaxHealth / 3f;

            for (int i = 0; i < 3; i++)
            {
                m_HealthSlider[i].maxValue = toDivide;
                m_HealthSlider[i].value = toDivide;
            }
        }

        /// <summary>
        /// 지정한 HP로 HP 바 설정
        /// </summary>
        /// <param name="health"></param>
        public void SetHealthSlider(int health)
        {
            if (health == Player.instance.m_Stats.m_MaxHealth)
            {
                SetMaxHealthSlider();
                return;
            }

            float toDivide = (float)Player.instance.m_Stats.m_MaxHealth / 3f;
            float fDivided = (float)health / toDivide;
            int iDivided = (int)fDivided;

            //Debug.Log( "iDivided = " + iDivided + " fDivided = " + fDivided );

            if (iDivided == 0)
            {
                m_HealthSlider[0].normalizedValue = fDivided;
                m_HealthSlider[1].normalizedValue = 0f;
                m_HealthSlider[2].normalizedValue = 0f;
            }
            else if (iDivided == 1)
            {
                m_HealthSlider[0].normalizedValue = 1f;
                m_HealthSlider[1].normalizedValue = fDivided - 1f;
                m_HealthSlider[2].normalizedValue = 0f;
            }
            else if (iDivided == 2)
            {
                m_HealthSlider[0].normalizedValue = 1f;
                m_HealthSlider[1].normalizedValue = 1f;
                m_HealthSlider[2].normalizedValue = fDivided - 2f;
            }
        }

        /// <summary>
        /// HP 바를 꽈 채운다.
        /// </summary>
        public void SetMaxHealthSlider()
        {
            for (int i = 0; i < 3; i++)
            {
                m_HealthSlider[i].normalizedValue = 1f;
            }
        }

        /// <summary>
        /// HP 바 셀 하나를 회복한다.
        /// </summary>
        public void RecoverOneCellHealth()
        {
            for (int i = 0; i < 3; i++)
            {
                if (m_HealthSlider[i].normalizedValue != 1f)
                {
                    m_HealthSlider[i].normalizedValue = 1f;
                    return;
                }
            }
        }

        /// <summary>
        /// 장정된 탄약의 숫자를 설정한다.
        /// </summary>
        private void SetAmmoText()
        {
            m_ClipAmmoText.text = m_WeaponHandler.m_CurrentWeapon.m_Ammo.clipAmmo.ToString();
            m_CarryingAmmoText.text = m_WeaponHandler.m_CurrentWeapon.m_Ammo.carryingAmmo.ToString();

            //m_AnotherAmmoText.text = m_AnotherWeapon.m_Ammo.clipAmmo.ToString();
        }

        /// <summary>
        /// 다른 무기를 설정한다.
        /// </summary>
        public void SetAnotherWeapon()
        {
            if (m_WeaponHandler.m_CurrentWeapon.m_WeaponType == Weapon.WeaponType.Primary)
            {
                anotherType = Weapon.WeaponType.Secondary;
            }
            else if (m_WeaponHandler.m_CurrentWeapon.m_WeaponType == Weapon.WeaponType.Secondary)
            {
                anotherType = Weapon.WeaponType.Primary;
            }
            else if (m_WeaponHandler.m_CurrentWeapon.m_WeaponType == Weapon.WeaponType.Sidearm)
            {
                anotherType = Weapon.WeaponType.Primary;
            }
        }

        /// <summary>
        /// 약품의 개수를 설정한다.
        /// </summary>
        public void SetMedikitText()
        {
            m_MedikitNumberText.text = Player.instance.m_Inventory.m_CurrentMedikit.ToString();
        }

        /// <summary>
        /// 슈류탄의 개수를 설정한다.
        /// </summary>
        public void SetGrenadeText()
        {
            m_GrenadeNumberText.text = Player.instance.m_Inventory.m_CurrentGrenade.ToString();
        }
    }
}