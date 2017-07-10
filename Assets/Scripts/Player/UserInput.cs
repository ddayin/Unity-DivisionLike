/*
 * reference - https://www.youtube.com/watch?v=gN1BmP4ONSQ&list=PLfxIz_UlKk7IwrcF2zHixNtFmh0lznXow&t=4140s&index=4
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DivisionLike
{
    public class UserInput : MonoBehaviour
    {
        public CharacterMovement characterMove { get; protected set; }
        public WeaponHandler weaponHandler { get; protected set; }
        private PlayerInventory inventory;
        public CrosshairHandler crosshairHandler;

        [System.Serializable]
        public class InputSettings
        {
            public string verticalAxis = "Vertical";
            public string horizontalAxis = "Horizontal";
            public string jumpButton = "Jump";
            public string reloadButton = "Reload";
            public string aimButton = "Fire2";
            public string fireButton = "Fire1";
            public string switchWeaponButton = "Fire3";
            public string primaryWeaponButton = "1";
            public string secondaryWeaponButton = "2";
            public string sidearmButton = "3";
            public string sprintButton = "Sprint";
            public string medikitButton = "Medikit";
        }
        [SerializeField]
        public InputSettings inputSettings;

        [System.Serializable]
        public class OtherSettings
        {
            public float lookSpeed = 5.0f;
            public float lookDistance = 30.0f;
            public bool requireInputForTurn = true;
            public LayerMask aimDetectionLayers;
        }
        [SerializeField]
        public OtherSettings otherSettings;

        public Camera TPSCamera;

        public bool debugAim;
        public Transform spine;
        public bool aiming = false;

        private Dictionary<Weapon, GameObject> crosshairPrefabMap = new Dictionary<Weapon, GameObject>();

        // Use this for initialization
        void Start()
        {
            characterMove = GetComponent<CharacterMovement>();
            weaponHandler = GetComponent<WeaponHandler>();
            inventory = GetComponent<PlayerInventory>();

            SetupCrosshairs();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void SetupCrosshairs()
        {
            //if ( weaponHandler.weaponsList.Count > 0 )
            //{
            //    foreach ( Weapon wep in weaponHandler.weaponsList )
            //    {
            //        GameObject prefab = wep.weaponSettings.crosshairPrefab;
            //        if ( prefab != null )
            //        {
            //            GameObject clone = (GameObject) Instantiate( prefab );
            //            crosshairPrefabMap.Add( wep, clone );
            //            ToggleCrosshair( false, wep );
            //        }
            //    }
            //}
            ToggleCrosshair( false, null );
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
            if ( weaponHandler )
            {
                if ( weaponHandler.currentWeapon )
                {
                    if ( aiming )
                        PositionSpine();
                }
            }
        }

        //Handles character logic
        void CharacterLogic()
        {
            if ( !characterMove )
                return;

            float v = Input.GetAxis( inputSettings.verticalAxis );
            float h = Input.GetAxis( inputSettings.horizontalAxis );

            // always walk when player is aiming
            if ( Player.instance.userInput.aiming == true )
            {
                characterMove.Animate( v * 0.5f, h * 0.5f );
            }
            else
            {
                // sprint
                if ( Input.GetButton( inputSettings.sprintButton ) == true )
                {
                    characterMove.Animate( v, h );
                }
                // walk
                else
                {
                    characterMove.Animate( v * 0.5f, h * 0.5f );
                }
            }
            
            //if ( Input.GetButtonDown( inputSettings.jumpButton ) )
            //    characterMove.Jump();
        }

        //Handles camera logic
        void CameraLookLogic()
        {
            if ( !TPSCamera )
                return;

            otherSettings.requireInputForTurn = !aiming;

            if ( otherSettings.requireInputForTurn )
            {
                if ( Input.GetAxis( inputSettings.horizontalAxis ) != 0 || Input.GetAxis( inputSettings.verticalAxis ) != 0 )
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
            if ( !weaponHandler )
                return;

            aiming = Input.GetButton( inputSettings.aimButton ) || debugAim;
            weaponHandler.Aim( aiming );

            if ( Input.GetButtonDown( inputSettings.switchWeaponButton ) )
            {
                weaponHandler.SwitchWeapons();
                UpdateCrosshairs();
                PlayerHUD.instance.SetAnotherWeapon();
            }

            if ( Input.GetButtonDown( inputSettings.primaryWeaponButton ) )
            {
                weaponHandler.SwitchWeapons( Weapon.WeaponType.Primary );
            }
            else if ( Input.GetButtonDown( inputSettings.secondaryWeaponButton ) )
            {
                weaponHandler.SwitchWeapons( Weapon.WeaponType.Secondary );
            }
            else if ( Input.GetButtonDown( inputSettings.sidearmButton ) )
            {
                //weaponHandler.SwitchWeapons( Weapon.WeaponType.Sidearm );
                weaponHandler.SwitchWeapons( Weapon.WeaponType.Secondary );
            }

            if ( weaponHandler.currentWeapon )
            {
                Ray aimRay = new Ray( TPSCamera.transform.position, TPSCamera.transform.forward );

                //Debug.DrawRay (aimRay.origin, aimRay.direction);
                if ( Input.GetButton( inputSettings.fireButton ) && aiming )
                    weaponHandler.FireCurrentWeapon( aimRay );
                if ( Input.GetButtonDown( inputSettings.reloadButton ) )
                    weaponHandler.Reload();

                if ( aiming )
                {
                    ToggleCrosshair( true, weaponHandler.currentWeapon );
                    //PositionCrosshair( aimRay, weaponHandler.currentWeapon );
                    HitCrosshair();
                }
                else
                {
                    ToggleCrosshair( false, weaponHandler.currentWeapon );
                }
            }
            else
            {
                TurnOffAllCrosshairs();
            }

        }

        private void InventoryLogic()
        {
            if ( Input.GetButtonDown( inputSettings.medikitButton ) )
            {
                inventory.UseMedikit();
            }
        }

        Vector3 fireDirection;
        Vector3 firePoint;

        void HitCrosshair()
        {
            RaycastHit hit;
            int range = 1000;
            fireDirection = TPSCamera.transform.forward * 10;
            firePoint = weaponHandler.currentWeapon.weaponSettings.bulletSpawn.position;
            // Debug the ray out in the editor:
            Debug.DrawRay( firePoint, fireDirection, Color.green );

            if ( Physics.Raycast( firePoint, ( fireDirection ), out hit, range ) )
            {
                // Scale if crosshair is on something:


                if ( hit.transform.gameObject.layer == 13 )
                {
                    crosshairHandler.ChangeColor( Color.red );
                }
                else
                {
                    crosshairHandler.ChangeColor( Color.white );
                }
            }
            else
            {
                crosshairHandler.ChangeColor( Color.white );
            }
        }

        void TurnOffAllCrosshairs()
        {
            //foreach ( Weapon wep in crosshairPrefabMap.Keys )
            //{
            //    ToggleCrosshair( false, wep );
            //}
            ToggleCrosshair( false, null );
        }

        //void CreateCrosshair( Weapon wep )
        //{
        //    GameObject prefab = wep.weaponSettings.crosshairPrefab;
        //    if ( prefab != null )
        //    {
        //        prefab = Instantiate( prefab );
        //        ToggleCrosshair( false, wep );
        //    }
        //}

        //void DeleteCrosshair( Weapon wep )
        //{
        //    if ( !crosshairPrefabMap.ContainsKey( wep ) )
        //        return;

        //    Destroy( crosshairPrefabMap[ wep ] );
        //    crosshairPrefabMap.Remove( wep );
        //}

        // Position the crosshair to the point that we are aiming
        //void PositionCrosshair( Ray ray, Weapon wep )
        //{
        //    Weapon curWeapon = weaponHandler.currentWeapon;
        //    if ( curWeapon == null )
        //        return;
        //    if ( !crosshairPrefabMap.ContainsKey( wep ) )
        //        return;

        //    //GameObject crosshairPrefab = crosshairPrefabMap[ wep ];
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
        void ToggleCrosshair( bool enabled, Weapon wep )
        {
            //if ( !crosshairPrefabMap.ContainsKey( wep ) )
            //    return;

            //crosshairPrefabMap[ wep ].SetActive( enabled );

            crosshairHandler.gameObject.SetActive( enabled );
        }

        void UpdateCrosshairs()
        {
            if ( weaponHandler.weaponsList.Count == 0 )
                return;

            //foreach ( Weapon wep in weaponHandler.weaponsList )
            //{
            //    if ( wep != weaponHandler.currentWeapon )
            //    {
            //        ToggleCrosshair( false, wep );
            //    }
            //}

            ToggleCrosshair( false, null );
        }

        //Postions the spine when aiming
        void PositionSpine()
        {
            if ( !spine || !weaponHandler.currentWeapon || !TPSCamera )
                return;

            Transform mainCamT = TPSCamera.transform;
            Vector3 mainCamPos = mainCamT.position;
            Vector3 dir = mainCamT.forward;
            Ray ray = new Ray( mainCamPos, dir );

            spine.LookAt( ray.GetPoint( 50 ) );

            Vector3 eulerAngleOffset = weaponHandler.currentWeapon.userSettings.spineRotation;
            spine.Rotate( eulerAngleOffset );
        }

        //Make the character look at a forward point from the camera
        void CharacterLook()
        {
            Transform mainCamT = TPSCamera.transform;
            Transform pivotT = mainCamT.parent;
            Vector3 pivotPos = pivotT.position;
            Vector3 lookTarget = pivotPos + ( pivotT.forward * otherSettings.lookDistance );
            Vector3 thisPos = transform.position;
            Vector3 lookDir = lookTarget - thisPos;
            Quaternion lookRot = Quaternion.LookRotation( lookDir );
            lookRot.x = 0;
            lookRot.z = 0;

            Quaternion newRotation = Quaternion.Lerp( transform.rotation, lookRot, Time.deltaTime * otherSettings.lookSpeed );
            transform.rotation = newRotation;
        }
    }
}
