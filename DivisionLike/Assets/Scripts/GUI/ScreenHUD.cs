using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    /// <summary>
    /// 미니맵, 현재 경험치, 현재 레벨, 현재 장전된 총알 등을 포함한 UI 표시
    /// </summary>
    public class ScreenHUD : MonoBehaviour
    {
        public static ScreenHUD instance { get; private set; }

        private Text m_LevelText; // 현재 레벨
        private Slider m_XpSlider; // 현재 경험치
        private Slider m_AmmoSlider; // 현재 장전되어 있는 총알 슬라이더
        private Image m_AmmoSliderFillImage;
        private Image m_LoadingCircleImage;
        private Image m_ReloadImage; // 재장전 아이콘

        private MinimapHit m_MinimapHit;
        private CircularHit m_CircularHit;

        void Awake()
        {
            instance = this;


            m_LevelText = transform.Find("LevelText").GetComponent<Text>();
            m_XpSlider = transform.Find("ExpSlider").GetComponent<Slider>();
            m_AmmoSlider = transform.Find("AmmoSlider").GetComponent<Slider>();
            m_AmmoSliderFillImage = m_AmmoSlider.transform.Find("Fill Area/Fill").GetComponent<Image>();
            m_LoadingCircleImage = transform.Find("LoadingPanel/CircleImage").GetComponent<Image>();
            m_CircularHit = transform.Find("CircularHit").GetComponent<CircularHit>();
            m_MinimapHit = transform.Find("MiniMap/MapInner/Hit").GetComponent<MinimapHit>();
            m_ReloadImage = transform.Find("ReloadImage").GetComponent<Image>();
        }

        private void Start()
        {
            SetLevelText();
            CalculateExpSlider(0);
            SetAmmoSlider();
            SetLoadingCircle(0f);
        }

        /// <summary>
        /// 레벨을 설정한다.
        /// </summary>
        public void SetLevelText()
        {
            m_LevelText.text = Player.instance.m_Stats.m_CurrentLevel.ToString();
        }

        /// <summary>
        /// 경험치를 계산한다.
        /// </summary>
        /// <param name="xpToAdd"></param>
        public void CalculateExpSlider(int xpToAdd)
        {
            Player.instance.m_Stats.m_CurrentXP += (ulong)xpToAdd;
            Player.instance.m_Stats.CheckLevel();

            float normalizedXP = (float)(Player.instance.m_Stats.m_CurrentXP) /
                                 (float)(Player.instance.m_Stats.m_XpRequire
                                     [Player.instance.m_Stats.m_CurrentLevel - 1]);

            m_XpSlider.normalizedValue = normalizedXP;
        }

        private Color m_FillColor = new Color(1f, 0.74f, 0f, 1f);

        /// <summary>
        /// 총알 슬라이더를 설정한다.
        /// </summary>
        public void SetAmmoSlider()
        {
            float normalizedAmmo = (float)(Player.instance.m_WeaponHandler.m_CurrentWeapon.m_Ammo.clipAmmo) /
                                   (float)(Player.instance.m_WeaponHandler.m_CurrentWeapon.m_Ammo.maxClipAmmo);
            m_AmmoSlider.normalizedValue = normalizedAmmo;

            if (normalizedAmmo < 0.2f)
            {
                m_AmmoSliderFillImage.color = Color.red;
            }
            else
            {
                m_AmmoSliderFillImage.color = m_FillColor;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fillAmount"></param>
        public void SetLoadingCircle(float fillAmount)
        {
            m_LoadingCircleImage.fillAmount = fillAmount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enable"></param>
        public void SetEnableLoadingCircle(bool enable)
        {
            m_LoadingCircleImage.enabled = enable;
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitLoadingCircle()
        {
            m_LoadingCircleImage.enabled = true;
            m_LoadingCircleImage.fillAmount = 0f;
        }

        /// <summary>
        /// 해당 방향으로 맞은 부위를 회전시킨다.
        /// </summary>
        /// <param name="direction"></param>
        public void RotateCircularHit(Vector3 direction)
        {
            m_CircularHit.RotateHit(direction);
        }

        /// <summary>
        /// 해당 방향으로 맞은 것을 미니맵에 표시한다.
        /// </summary>
        /// <param name="direction"></param>
        public void RotateMinimapHit(Vector3 direction)
        {
            m_MinimapHit.RotateHit(direction);
        }

        /// <summary>
        /// 재장전 이미지를 활성화할지 정한다.
        /// </summary>
        /// <param name="enable"></param>
        public void SetEnableReloadImage(bool enable)
        {
            m_ReloadImage.gameObject.SetActive(enable);
        }
    }
}