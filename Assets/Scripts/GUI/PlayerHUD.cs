using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    public class PlayerHUD : MonoBehaviour
    {
        public static PlayerHUD instance = null;

        private Text _clipAmmoText;
        private Text _carryingAmmoText;
        private Text _anotherAmmoText;

        private Text _medikitNumberText;

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
            
            _clipAmmoText = transform.Find( "PlayerHUD/Ammo/ClipAmmoText" ).GetComponent<Text>();
            _carryingAmmoText = transform.Find( "PlayerHUD/Ammo/CarryingAmmoText" ).GetComponent<Text>();
            _anotherAmmoText = transform.Find( "PlayerHUD/Ammo/AnotherAmmoText" ).GetComponent<Text>();

            _medikitNumberText = transform.Find( "PlayerHUD/Medikit/NumberText" ).GetComponent<Text>();

            _weaponHandler = Player.instance._weaponHandler;

            SetAnotherWeapon();
            SetAmmoText();
            SetMedikitText();
        }

        // Update is called once per frame
        void Update()
        {
            SetAmmoText();
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
        
    }
}