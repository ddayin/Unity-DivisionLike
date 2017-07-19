using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    public class PlayerHUD : MonoBehaviour
    {
        public static PlayerHUD instance = null;

        private Slider[] _healthSlider = new Slider[ 3 ];

        private Text _clipAmmoText;
        private Text _carryingAmmoText;
        private Text _anotherAmmoText;

        private Text _medikitNumberText;
        private Text _grenadeNumberText;

        private WeaponHandler _weaponHandler;
        private Weapon _anotherWeapon = null;

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

            _healthSlider[ 0 ] = transform.Find( "PlayerHUD/HealthUI/HealthSlider_0" ).GetComponent<Slider>();
            _healthSlider[ 1 ] = transform.Find( "PlayerHUD/HealthUI/HealthSlider_1" ).GetComponent<Slider>();
            _healthSlider[ 2 ] = transform.Find( "PlayerHUD/HealthUI/HealthSlider_2" ).GetComponent<Slider>();

            _clipAmmoText = transform.Find( "PlayerHUD/Ammo/ClipAmmoText" ).GetComponent<Text>();
            _carryingAmmoText = transform.Find( "PlayerHUD/Ammo/CarryingAmmoText" ).GetComponent<Text>();
            _anotherAmmoText = transform.Find( "PlayerHUD/Ammo/AnotherAmmoText" ).GetComponent<Text>();

            _medikitNumberText = transform.Find( "PlayerHUD/Medikit/NumberText" ).GetComponent<Text>();
            _grenadeNumberText = transform.Find( "PlayerHUD/Grenade/NumberText" ).GetComponent<Text>();


            _weaponHandler = Player.instance._weaponHandler;

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
        
        private void InitHealthSlider()
        {
            float toDivide = (float) Player.instance._stats._maxHealth / 3f;

            for ( int i = 0; i < 3; i++ )
            {
                _healthSlider[ i ].maxValue = toDivide;
                _healthSlider[ i ].value = toDivide;
            }
        }

        public void SetHealthSlider( int health )
        {
            float toDivide = (float) Player.instance._stats._maxHealth / 3f;
            float fDivided = (float) health / toDivide;
            int iDivided = (int) fDivided;
            
            //Debug.Log( "iDivided = " + iDivided + " fDivided = " + fDivided );
            
            if ( iDivided == 0 )
            {
                _healthSlider[ 0 ].normalizedValue = fDivided;
                _healthSlider[ 1 ].normalizedValue = 0f;
                _healthSlider[ 2 ].normalizedValue = 0f;
            }
            else if ( iDivided == 1 )
            {
                _healthSlider[ 0 ].normalizedValue = 1f;
                _healthSlider[ 1 ].normalizedValue = fDivided - 1f;
                _healthSlider[ 2 ].normalizedValue = 0f;
            }
            else if ( iDivided == 2 )
            {
                _healthSlider[ 0 ].normalizedValue = 1f;
                _healthSlider[ 1 ].normalizedValue = 1f;
                _healthSlider[ 2 ].normalizedValue = fDivided - 2f;
            }
        }
        
        public void SetMaxHealthSlider()
        {
            for ( int i = 0; i < 3; i++ )
            {
                _healthSlider[ i ].normalizedValue = 1f;
            }
        }

        private void SetAmmoText()
        {
            _clipAmmoText.text = _weaponHandler.currentWeapon.ammo.clipAmmo.ToString();
            _carryingAmmoText.text = _weaponHandler.currentWeapon.ammo.carryingAmmo.ToString();
            
            _anotherAmmoText.text = _anotherWeapon.ammo.clipAmmo.ToString();
        }

        public void SetAnotherWeapon()
        {
            Weapon.WeaponType anotherType = Weapon.WeaponType.Primary;
            if ( _weaponHandler.currentWeapon._weaponType == Weapon.WeaponType.Primary )
            {
                anotherType = Weapon.WeaponType.Secondary;
            }
            else if ( _weaponHandler.currentWeapon._weaponType == Weapon.WeaponType.Secondary )
            {
                anotherType = Weapon.WeaponType.Primary;
            }
            else if ( _weaponHandler.currentWeapon._weaponType == Weapon.WeaponType.Sidearm )
            {
                anotherType = Weapon.WeaponType.Primary;
            }

            _anotherWeapon = _weaponHandler.dicWeapons[ anotherType ];
        }

        public void SetMedikitText()
        {
            _medikitNumberText.text = Player.instance._inventory._currentMedikit.ToString();
        }

        public void SetGrenadeText()
        {
            _grenadeNumberText.text = Player.instance._inventory._currentGrenade.ToString();
        }
        
    }
}