using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    public class ScreenHUD : MonoBehaviour
    {
        public static ScreenHUD instance = null;

        private Text _levelText;
        private Slider _xpSlider;
        private Slider _ammoSlider;
        private Image _loadingCircleImage;

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

            _levelText = transform.Find( "LevelText" ).GetComponent<Text>();
            _xpSlider = transform.Find( "ExpSlider" ).GetComponent<Slider>();
            _ammoSlider = transform.Find( "AmmoSlider" ).GetComponent<Slider>();
            _loadingCircleImage = transform.Find( "LoadingPanel/CircleImage" ).GetComponent<Image>();
            
            SetLevelText();
            CalculateExpSlider( 0 );
            SetAmmoSlider();
            SetLoadingCircle( 0f );
        }

        // Update is called once per frame
        void Update()
        {
            SetAmmoSlider();
        }

        public void SetLevelText()
        {
            _levelText.text = Player.instance._stats._currentLevel.ToString();
        }

        public void CalculateExpSlider( int xpToAdd )
        {
            Player.instance._stats._currentXP += (ulong) xpToAdd;
            Player.instance._stats.CheckLevel();
            
            float normalizedXP = (float) (Player.instance._stats._currentXP) / (float) (Player.instance._stats._xpRequire[ Player.instance._stats._currentLevel - 1 ]);

            _xpSlider.normalizedValue = normalizedXP;
        }

        private void SetAmmoSlider()
        {
            float normalizedAmmo = (float) (Player.instance._weaponHandler.currentWeapon.ammo.clipAmmo) / (float) (Player.instance._weaponHandler.currentWeapon.ammo.maxClipAmmo);
            _ammoSlider.normalizedValue = normalizedAmmo;
        }

        public void SetLoadingCircle( float fillAmount )
        {
            _loadingCircleImage.fillAmount = fillAmount;
        }

        public void SetEnableLoadingCircle( bool enable )
        {
            _loadingCircleImage.enabled = enable;
        }

        public void InitLoadingCircle()
        {
            _loadingCircleImage.enabled = true;
            _loadingCircleImage.fillAmount = 0f;
        }
    }
}

