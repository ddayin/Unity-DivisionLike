/*
 * reference - https://www.youtube.com/watch?v=gN1BmP4ONSQ&list=PLfxIz_UlKk7IwrcF2zHixNtFmh0lznXow&t=4140s&index=4
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace DivisionLike
{
    public class UserInput : MonoBehaviour
    {
        private CharacterMovement _characterMove;
        private WeaponHandler _weaponHandler;
        private PlayerInventory _inventory;

        [System.Serializable]
        public class InputSettings
        {
            public string _verticalAxis = "Vertical";
            public string _horizontalAxis = "Horizontal";
            public string _jumpButton = "Jump";
            public string _reloadButton = "Reload";
            public string _aimButton = "Fire2";
            public string _fireButton = "Fire1";
            public string _switchWeaponButton = "Fire3";
            public string _primaryWeaponButton = "1";
            public string _secondaryWeaponButton = "2";
            public string _sidearmButton = "3";
            public string _sprintButton = "Sprint";
            public string _medikitButton = "Medikit";
        }
        [SerializeField]
        public InputSettings _inputSettings;

        [System.Serializable]
        public class OtherSettings
        {
            public float _lookSpeed = 5.0f;
            public float _lookDistance = 30.0f;
            public bool _requireInputForTurn = true;
            public LayerMask _aimDetectionLayers;
        }
        [SerializeField]
        public OtherSettings _otherSettings;

        private Camera _TPSCamera;

        public bool _debugAim = false;
        public Transform _spineTransform;
        public bool _isAiming = false;
        public bool _isSprinting = false;

        private Dictionary<Weapon, GameObject> _crosshairPrefabMap = new Dictionary<Weapon, GameObject>();

        // Use this for initialization
        void Start()
        {
            _characterMove = transform.GetComponent<CharacterMovement>();
            _weaponHandler = transform.GetComponent<WeaponHandler>();
            _inventory = transform.GetComponent<PlayerInventory>();

            _TPSCamera = Camera.main;

            SetupCrosshairs();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        

        // Update is called once per frame
        void Update()
        {
            CharacterLogic();
            CameraLookLogic();
            WeaponLogic();
            InventoryLogic();
        }

        void LateUpdate()
        {
            if ( _weaponHandler )
            {
                if ( _weaponHandler.currentWeapon )
                {
                    if ( _isAiming == true )
                    {
                        PositionSpine();
                    }   
                }
            }
        }

        //Handles character logic
        void CharacterLogic()
        {
            if ( !_characterMove )
                return;

            float v = Input.GetAxis( _inputSettings._verticalAxis );
            float h = Input.GetAxis( _inputSettings._horizontalAxis );

            // always walk when player is aiming
            if ( Player.instance._userInput._isAiming == true )
            {
                _characterMove.Animate( v * 0.5f, h * 0.5f );
            }
            else
            {
                // sprint
                if ( Input.GetButton( _inputSettings._sprintButton ) == true )
                {
                    _isSprinting = true;
                    _characterMove.Animate( v, h );
                }
                // walk
                else
                {
                    _isSprinting = false;
                    _characterMove.Animate( v * 0.5f, h * 0.5f );
                }
            }
            
            //if ( Input.GetButtonDown( inputSettings.jumpButton ) )
            //    characterMove.Jump();
        }

        //Handles camera logic
        void CameraLookLogic()
        {
            if ( !_TPSCamera )
                return;

            _otherSettings._requireInputForTurn = !_isAiming;

            if ( _otherSettings._requireInputForTurn == true )
            {
                if ( Input.GetAxis( _inputSettings._horizontalAxis ) != 0 || Input.GetAxis( _inputSettings._verticalAxis ) != 0 )
                {
                    CharacterLook();
                }
            }
            else
            {
                CharacterLook();
            }
        }

        //Handles all weapon logic
        void WeaponLogic()
        {
            if ( !_weaponHandler )
                return;

            _isAiming = Input.GetButton( _inputSettings._aimButton ) || _debugAim == true;
            _weaponHandler.Aim( _isAiming );

            if ( Input.GetButtonDown( _inputSettings._switchWeaponButton ) )
            {
                _weaponHandler.SwitchWeapons();
                UpdateCrosshairs();
                PlayerHUD.instance.SetAnotherWeapon();
            }

            if ( Input.GetButtonDown( _inputSettings._primaryWeaponButton ) )
            {
                _weaponHandler.SwitchWeapons( Weapon.WeaponType.Primary );
            }
            else if ( Input.GetButtonDown( _inputSettings._secondaryWeaponButton ) )
            {
                _weaponHandler.SwitchWeapons( Weapon.WeaponType.Secondary );
            }
            else if ( Input.GetButtonDown( _inputSettings._sidearmButton ) )
            {
                //weaponHandler.SwitchWeapons( Weapon.WeaponType.Sidearm );
                _weaponHandler.SwitchWeapons( Weapon.WeaponType.Secondary );
            }

            if ( _weaponHandler.currentWeapon )
            {
                Ray aimRay = new Ray( _TPSCamera.transform.position, _TPSCamera.transform.forward );

                //Debug.DrawRay (aimRay.origin, aimRay.direction);
                if ( Input.GetButton( _inputSettings._fireButton ) == true && _isAiming == true )
                {
                    _weaponHandler.FireCurrentWeapon( aimRay );
                }
                if ( Input.GetButtonDown( _inputSettings._reloadButton ) == true )
                {
                    _weaponHandler.Reload();
                }   

                if ( _isAiming == true )
                {
                    ToggleCrosshair( true, _weaponHandler.currentWeapon );
                    //PositionCrosshair( aimRay, weaponHandler.currentWeapon );
                    HitCrosshair();
                }
                else
                {
                    ToggleCrosshair( false, _weaponHandler.currentWeapon );
                }
            }
            else
            {
                TurnOffAllCrosshairs();
            }
        }

        private void InventoryLogic()
        {
            if ( Input.GetButtonDown( _inputSettings._medikitButton ) )
            {
                _inventory.UseMedikit();
            }
        }

#region crosshair

        private void SetupCrosshairs()
        {
            if ( _weaponHandler.weaponsList.Count > 0 )
            {
                foreach ( Weapon wep in _weaponHandler.weaponsList )
                {
                    GameObject prefab = wep.weaponSettings.crosshairPrefab;
                    if ( prefab != null )
                    {
                        GameObject clone = (GameObject) Instantiate( prefab );
                        clone.transform.SetParent( ScreenHUD.instance.transform );
                        clone.transform.localPosition = Vector3.zero;
                        
                        _crosshairPrefabMap.Add( wep, clone );
                        ToggleCrosshair( false, wep );
                    }
                }
            }
        }

        Vector3 fireDirection;
        Vector3 firePoint;

        void HitCrosshair()
        {
            RaycastHit hit;
            int range = 1000;
            fireDirection = _TPSCamera.transform.forward * 10;
            firePoint = _weaponHandler.currentWeapon.weaponSettings.bulletSpawn.position;
            // Debug the ray out in the editor:
            Debug.DrawRay( firePoint, fireDirection, Color.green );

            CrosshairHandler crosshair = _weaponHandler.currentWeapon.weaponSettings.crosshairPrefab.GetComponent<CrosshairHandler>();

            if ( Physics.Raycast( firePoint, ( fireDirection ), out hit, range ) )
            {
                if ( hit.transform.gameObject.layer == LayerMask.GetMask( "Ragdoll" ) )
                {
                    //Debug.LogWarning( "crosshair Red" );
                    crosshair.ChangeColor( Color.red );
                }
                else
                {
                    //Debug.Log( "crosshair white" );
                    crosshair.ChangeColor( Color.white );
                }
            }
            else
            {
                crosshair.ChangeColor( Color.white );
            }
        }

        private void TurnOffAllCrosshairs()
        {
            foreach ( Weapon wep in _crosshairPrefabMap.Keys )
            {
                ToggleCrosshair( false, wep );
            }
        }

        private void CreateCrosshair( Weapon wep )
        {
            GameObject prefab = wep.weaponSettings.crosshairPrefab;
            if ( prefab != null )
            {
                prefab = Instantiate( prefab );
                prefab.transform.SetParent( ScreenHUD.instance.transform );
                ToggleCrosshair( false, wep );
            }
        }

        private void DeleteCrosshair( Weapon wep )
        {
            if ( !_crosshairPrefabMap.ContainsKey( wep ) )
                return;

            Destroy( _crosshairPrefabMap[ wep ] );
            _crosshairPrefabMap.Remove( wep );
        }

        // Position the crosshair to the point that we are aiming
        //private void PositionCrosshair( Ray ray, Weapon wep )
        //{
        //    Weapon curWeapon = weaponHandler.currentWeapon;
        //    if ( curWeapon == null )
        //        return;
        //    if ( !crosshairPrefabMap.ContainsKey( wep ) )
        //        return;

        //    GameObject crosshairPrefab = crosshairPrefabMap[ wep ];
        //    RaycastHit hit;
        //    Transform bSpawn = curWeapon.weaponSettings.bulletSpawn;
        //    Vector3 bSpawnPoint = bSpawn.position;
        //    Vector3 dir = ray.GetPoint( curWeapon.weaponSettings.range ) - bSpawnPoint;

        //    //Debug.DrawRay( bSpawnPoint, dir );

        //    if ( Physics.Raycast( bSpawnPoint, dir, out hit, curWeapon.weaponSettings.range,
        //        curWeapon.weaponSettings.bulletLayers ) )
        //    {
        //        if ( crosshairPrefab != null )
        //        {
        //            ToggleCrosshair( true, curWeapon );
        //            Vector3 newPos = hit.point;
        //            newPos.z = 10.0f;       // maintain certain position of crosshair z
        //            crosshairPrefab.transform.position = newPos;
        //            crosshairPrefab.transform.LookAt( Camera.main.transform );
        //        }
        //    }
        //    else
        //    {
        //        ToggleCrosshair( false, curWeapon );
        //    }
        //}

        // Toggle on and off the crosshair prefab
        private void ToggleCrosshair( bool enabled, Weapon wep )
        {
            if ( !_crosshairPrefabMap.ContainsKey( wep ) )
                return;

            _crosshairPrefabMap[ wep ].SetActive( enabled );
        }

        private void UpdateCrosshairs()
        {
            if ( _weaponHandler.weaponsList.Count == 0 )
                return;

            foreach ( Weapon wep in _weaponHandler.weaponsList )
            {
                if ( wep != _weaponHandler.currentWeapon )
                {
                    ToggleCrosshair( false, wep );
                }
            }
        }
#endregion


        //Postions the spine when aiming
        void PositionSpine()
        {
            if ( !_spineTransform || !_weaponHandler.currentWeapon || !_TPSCamera )
                return;

            Transform mainCamT = _TPSCamera.transform;
            Vector3 mainCamPos = mainCamT.position;
            Vector3 dir = mainCamT.forward;
            Ray ray = new Ray( mainCamPos, dir );

            _spineTransform.LookAt( ray.GetPoint( 50f ) );

            Vector3 eulerAngleOffset = _weaponHandler.currentWeapon.userSettings.spineRotation;
            _spineTransform.Rotate( eulerAngleOffset );
        }

        //Make the character look at a forward point from the camera
        void CharacterLook()
        {
            Transform mainCamT = _TPSCamera.transform;
            Transform pivotT = mainCamT.parent;
            Vector3 pivotPos = pivotT.position;
            Vector3 lookTarget = pivotPos + ( pivotT.forward * _otherSettings._lookDistance );
            Vector3 thisPos = transform.position;
            Vector3 lookDir = lookTarget - thisPos;
            Quaternion lookRot = Quaternion.LookRotation( lookDir );
            lookRot.x = 0;
            lookRot.z = 0;

            Quaternion newRotation = Quaternion.Lerp( transform.rotation, lookRot, Time.deltaTime * _otherSettings._lookSpeed );
            transform.rotation = newRotation;
        }
    }
}
