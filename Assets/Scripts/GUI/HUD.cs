using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    public class HUD : MonoBehaviour
    {
        private Text clipAmmoText;
        private Text carryingAmmoText;
        private Text maxClipAmmoText;

        private WeaponHandler weaponHandler;

        void Awake()
        {
            Debug.Log( "HUD Awake()" );

            clipAmmoText = transform.Find( "Ammo/ClipAmmoText" ).GetComponent<Text>();
            carryingAmmoText = transform.Find( "Ammo/CarryingAmmoText" ).GetComponent<Text>();
            maxClipAmmoText = transform.Find( "Ammo/MaxClipAmmoText" ).GetComponent<Text>();

            weaponHandler = Player.instance.weaponHandler;

            SetAmmoText();            
        }

        private void SetAmmoText()
        {
            clipAmmoText.text = weaponHandler.currentWeapon.ammo.clipAmmo.ToString();
            carryingAmmoText.text = weaponHandler.currentWeapon.ammo.carryingAmmo.ToString();
            maxClipAmmoText.text = weaponHandler.currentWeapon.ammo.maxClipAmmo.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            SetAmmoText();
        }
    }
}