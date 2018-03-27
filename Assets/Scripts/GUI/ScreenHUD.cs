using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    public class ScreenHUD : MonoBehaviour
    {
        public static ScreenHUD instance = null;

        private Text m_LevelText;
        private Slider m_XpSlider;
        private Slider m_AmmoSlider;
        private Image m_AmmoSliderFillImage;
        private Image m_LoadingCircleImage;
        private CircularHit m_CircularHit;
        private Image m_ReloadImage;

        void Awake()
        {
            if ( instance == null )
            {
                instance = this;
            }
            else if ( instance != null )
            {
                Destroy( gameObject );
            }

            m_LevelText = transform.Find( "LevelText" ).GetComponent<Text>();
            m_XpSlider = transform.Find( "ExpSlider" ).GetComponent<Slider>();
            m_AmmoSlider = transform.Find( "AmmoSlider" ).GetComponent<Slider>();
            m_AmmoSliderFillImage = m_AmmoSlider.transform.Find( "Fill Area/Fill" ).GetComponent<Image>();
            m_LoadingCircleImage = transform.Find( "LoadingPanel/CircleImage" ).GetComponent<Image>();
            m_CircularHit = transform.Find( "CircularHit" ).GetComponent<CircularHit>();
            m_ReloadImage = transform.Find( "ReloadImage" ).GetComponent<Image>();
            
            SetLevelText();
            CalculateExpSlider( 0 );
            SetAmmoSlider();
            SetLoadingCircle( 0f );
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
        public void CalculateExpSlider( int xpToAdd )
        {
            Player.instance.m_Stats.m_CurrentXP += (ulong) xpToAdd;
            Player.instance.m_Stats.CheckLevel();
            
            float normalizedXP = (float) (Player.instance.m_Stats.m_CurrentXP) / (float) (Player.instance.m_Stats.m_XpRequire[ Player.instance.m_Stats.m_CurrentLevel - 1 ]);

            m_XpSlider.normalizedValue = normalizedXP;
        }

        private Color _fillColor = new Color( 1f, 0.74f, 0f, 1f );

        /// <summary>
        /// 총알 슬라이더를 설정한다.
        /// </summary>
        public void SetAmmoSlider()
        {
            float normalizedAmmo = (float) (Player.instance.m_WeaponHandler.m_CurrentWeapon.m_Ammo.clipAmmo) / (float) (Player.instance.m_WeaponHandler.m_CurrentWeapon.m_Ammo.maxClipAmmo);
            m_AmmoSlider.normalizedValue = normalizedAmmo;

            if ( normalizedAmmo < 0.2f )
            {
                m_AmmoSliderFillImage.color = Color.red;
            }
            else
            {
                m_AmmoSliderFillImage.color = _fillColor;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fillAmount"></param>
        public void SetLoadingCircle( float fillAmount )
        {
            m_LoadingCircleImage.fillAmount = fillAmount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enable"></param>
        public void SetEnableLoadingCircle( bool enable )
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
        public void RotateCircularHit( Vector3 direction )
        {
            m_CircularHit.RotateHit( direction );
        }

        /// <summary>
        /// 재장전 이미지를 활성화할지 정한다.
        /// </summary>
        /// <param name="enable"></param>
        public void SetEnableReloadImage( bool enable )
        {
            m_ReloadImage.gameObject.SetActive( enable );
        }
    }
}

