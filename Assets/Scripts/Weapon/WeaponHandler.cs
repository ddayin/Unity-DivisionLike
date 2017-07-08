/*
 * reference - https://www.youtube.com/watch?v=gN1BmP4ONSQ&list=PLfxIz_UlKk7IwrcF2zHixNtFmh0lznXow&t=4140s&index=4
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DivisionLike
{
    public class WeaponHandler : MonoBehaviour
    {
        private Animator animator;
        private SoundController sc;

        [System.Serializable]
        public class UserSettings
        {
            public Transform rightHand;
            public Transform pistolUnequipSpot;
            public Transform rifleUnequipSpot;
        }
        [SerializeField]
        public UserSettings userSettings;

        [System.Serializable]
        public class Animations
        {
            public string weaponTypeInt = "WeaponType";
            public string reloadingBool = "isReloading";
            public string aimingBool = "Aiming";
        }
        [SerializeField]
        public Animations animations;

        public Weapon currentWeapon;
        public List<Weapon> weaponsList = new List<Weapon>();
        public Dictionary<Weapon.WeaponType, Weapon> dicWeapons = new Dictionary<Weapon.WeaponType, Weapon>();
        public int maxWeapons = 2;
        public bool aim { get; protected set; }
        private bool reload;
        private int weaponType;
        private bool settingWeapon;

        // Use this for initialization
        void OnEnable()
        {
            GameObject check = GameObject.FindGameObjectWithTag( "Sound Controller" );
            if ( check != null )
                sc = check.GetComponent<SoundController>();
            animator = GetComponent<Animator>();
            SetupWeapons();

            dicWeapons.Add( Weapon.WeaponType.Primary, weaponsList[ 0 ] );
            dicWeapons.Add( Weapon.WeaponType.Secondary, weaponsList[ 1 ] );
        }

        void SetupWeapons()
        {
            if ( currentWeapon )
            {
                currentWeapon.SetEquipped( true );
                currentWeapon.SetOwner( this );
                AddWeaponToList( currentWeapon );
                
                if ( currentWeapon.ammo.clipAmmo <= 0 )
                    Reload();

                if ( reload )
                    if ( settingWeapon )
                        reload = false;
            }

            if ( weaponsList.Count > 0 )
            {
                for ( int i = 0; i < weaponsList.Count; i++ )
                {
                    if ( weaponsList[ i ] != currentWeapon )
                    {
                        weaponsList[ i ].SetEquipped( false );
                        weaponsList[ i ].SetOwner( this );
                    }
                }
            }
            
        }

        // Update is called once per frame
        void Update()
        {
            Animate();
        }

        //Animates the character
        void Animate()
        {
            if ( animator == null )
                return;

            animator.SetBool( animations.aimingBool, aim );
            animator.SetBool( animations.reloadingBool, reload );
            animator.SetInteger( animations.weaponTypeInt, weaponType );

            if ( currentWeapon == null )
            {
                weaponType = 0;
                return;
            }

            switch ( currentWeapon.weaponType )
            {
                case Weapon.WeaponType.Secondary:
                    weaponType = 1;
                    break;
                case Weapon.WeaponType.Primary:
                    weaponType = 2;
                    break;
                case Weapon.WeaponType.Sidearm:
                    break;
                default:
                    break;
            }
        }

        //Adds a weapon to the weaponsList
        private void AddWeaponToList( Weapon weapon )
        {
            if ( weaponsList.Contains( weapon ) )
                return;

            weaponsList.Add( weapon );
        }

        private void AddWeaponToDic( Weapon.WeaponType type, Weapon weapon )
        {
            if ( dicWeapons.ContainsValue( weapon ) == true )
            {
                return;
            }

            dicWeapons.Add( type, weapon );
        }

        //Puts the finger on the trigger and asks if we pulled
        public void FireCurrentWeapon( Ray aimRay )
        {
            if ( currentWeapon.ammo.clipAmmo == 0 )
            {
                Reload();
                return;
            }

            currentWeapon.Fire( aimRay );
        }

        //Reloads the current weapon
        public void Reload()
        {
            if ( reload || !currentWeapon )
                return;

            if ( currentWeapon.ammo.carryingAmmo <= 0 || currentWeapon.ammo.clipAmmo == currentWeapon.ammo.maxClipAmmo )
                return;

            if ( sc != null )
            {
                if ( currentWeapon.soundSettings.reloadSound != null )
                {
                    if ( currentWeapon.soundSettings.audioS != null )
                    {
                        sc.PlaySound( currentWeapon.soundSettings.audioS, currentWeapon.soundSettings.reloadSound, true, currentWeapon.soundSettings.pitchMin, currentWeapon.soundSettings.pitchMax );
                    }
                }
            }

            reload = true;
            StartCoroutine( StopReload() );
        }

        //Stops the reloading of the weapon
        IEnumerator StopReload()
        {
            yield return new WaitForSeconds( currentWeapon.weaponSettings.reloadDuration );
            currentWeapon.LoadClip();
            reload = false;
        }

        //Sets out aim bool to be what we pass it
        public void Aim( bool aiming )
        {
            aim = aiming;
        }

        //Drops the current weapon
        public void DropCurWeapon()
        {
            if ( !currentWeapon )
                return;

            currentWeapon.SetEquipped( false );
            currentWeapon.SetOwner( null );
            weaponsList.Remove( currentWeapon );
            dicWeapons.Remove( currentWeapon.weaponType );
            currentWeapon = null;
        }

        //Switches to the next weapon
        public void SwitchWeapons()
        {
            if ( settingWeapon || weaponsList.Count == 0 || dicWeapons.Count == 0 )
                return;

            if ( currentWeapon != null )
            {
                int currentWeaponIndex = weaponsList.IndexOf( currentWeapon );
                int nextWeaponIndex = ( currentWeaponIndex + 1 ) % weaponsList.Count;

                currentWeapon = weaponsList[ nextWeaponIndex ];

                currentWeapon = dicWeapons[ (Weapon.WeaponType) nextWeaponIndex ];
            }
            else
            {
                currentWeapon = weaponsList[ 0 ];

                currentWeapon = dicWeapons[ Weapon.WeaponType.Primary ];
            }
            settingWeapon = true;
            StartCoroutine( StopSettingWeapon() );

            SetupWeapons();
        }

        public void SwitchWeapons( Weapon.WeaponType type )
        {
            if ( settingWeapon == true || weaponsList.Count == 0 || dicWeapons.Count == 0 )
            {
                return;
            }

            if ( currentWeapon != null )
            {
                currentWeapon = dicWeapons[ type ];
            }
            else
            {
                currentWeapon = dicWeapons[ Weapon.WeaponType.Primary ];
            }
            settingWeapon = true;
            StartCoroutine( StopSettingWeapon() );

            SetupWeapons();
        }

        //Stops swapping weapons
        IEnumerator StopSettingWeapon()
        {
            yield return new WaitForSeconds( 0.7f );
            settingWeapon = false;
        }

        void OnAnimatorIK()
        {
            if ( !animator )
                return;

            if ( currentWeapon && currentWeapon.userSettings.leftHandIKTarget && currentWeapon.weaponType == Weapon.WeaponType.Primary
                && !reload && !settingWeapon )
            {
                animator.SetIKPositionWeight( AvatarIKGoal.LeftHand, 1 );
                animator.SetIKRotationWeight( AvatarIKGoal.LeftHand, 1 );
                Transform target = currentWeapon.userSettings.leftHandIKTarget;
                Vector3 targetPos = target.position;
                Quaternion targetRot = target.rotation;
                animator.SetIKPosition( AvatarIKGoal.LeftHand, targetPos );
                animator.SetIKRotation( AvatarIKGoal.LeftHand, targetRot );
            }
            else
            {
                animator.SetIKPositionWeight( AvatarIKGoal.LeftHand, 0 );
                animator.SetIKRotationWeight( AvatarIKGoal.LeftHand, 0 );
            }
        }
    }
}