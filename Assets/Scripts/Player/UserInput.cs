/*
 * reference - https://www.youtube.com/watch?v=gN1BmP4ONSQ&list=PLfxIz_UlKk7IwrcF2zHixNtFmh0lznXow&t=4140s&index=4
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserInput : MonoBehaviour
{
    public CharacterMovement characterMove { get; protected set; }
    public WeaponHandler weaponHandler { get; protected set; }

    [System.Serializable]
    public class InputSettings
    {
        public string verticalAxis = "Vertical";
        public string horizontalAxis = "Horizontal";
        public string jumpButton = "Jump";
        public string reloadButton = "Reload";
        public string aimButton = "Fire2";
        public string fireButton = "Fire1";
        public string dropWeaponButton = "DropWeapon";
        public string switchWeaponButton = "SwitchWeapon";
    }
    [SerializeField]
    public InputSettings input;

    [System.Serializable]
    public class OtherSettings
    {
        public float lookSpeed = 5.0f;
        public float lookDistance = 30.0f;
        public bool requireInputForTurn = true;
        public LayerMask aimDetectionLayers;
    }
    [SerializeField]
    public OtherSettings other;

    public Camera TPSCamera;

    public bool debugAim;
    public Transform spine;
    bool aiming;

    Dictionary<Weapon, GameObject> crosshairPrefabMap = new Dictionary<Weapon, GameObject>();

    // Use this for initialization
    void Start()
    {
        characterMove = GetComponent<CharacterMovement>();
        weaponHandler = GetComponent<WeaponHandler>();
        SetupCrosshairs();
    }

    void SetupCrosshairs()
    {
        if ( weaponHandler.weaponsList.Count > 0 )
        {
            foreach ( Weapon wep in weaponHandler.weaponsList )
            {
                GameObject prefab = wep.weaponSettings.crosshairPrefab;
                if ( prefab != null )
                {
                    GameObject clone = (GameObject) Instantiate( prefab );
                    crosshairPrefabMap.Add( wep, clone );
                    ToggleCrosshair( false, wep );
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CharacterLogic();
        CameraLookLogic();
        WeaponLogic();
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

        characterMove.Animate( Input.GetAxis( input.verticalAxis ), Input.GetAxis( input.horizontalAxis ) );

        if ( Input.GetButtonDown( input.jumpButton ) )
            characterMove.Jump();
    }

    //Handles camera logic
    void CameraLookLogic()
    {
        if ( !TPSCamera )
            return;

        other.requireInputForTurn = !aiming;

        if ( other.requireInputForTurn )
        {
            if ( Input.GetAxis( input.horizontalAxis ) != 0 || Input.GetAxis( input.verticalAxis ) != 0 )
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

        aiming = Input.GetButton( input.aimButton ) || debugAim;
        weaponHandler.Aim( aiming );

        if ( Input.GetButtonDown( input.switchWeaponButton ) )
        {
            weaponHandler.SwitchWeapons();
            UpdateCrosshairs();
        }

        if ( weaponHandler.currentWeapon )
        {

            Ray aimRay = new Ray( TPSCamera.transform.position, TPSCamera.transform.forward );

            //Debug.DrawRay (aimRay.origin, aimRay.direction);
            if ( Input.GetButton( input.fireButton ) && aiming )
                weaponHandler.FireCurrentWeapon( aimRay );
            if ( Input.GetButtonDown( input.reloadButton ) )
                weaponHandler.Reload();
            if ( Input.GetButtonDown( input.dropWeaponButton ) )
            {
                DeleteCrosshair( weaponHandler.currentWeapon );
                weaponHandler.DropCurWeapon();
            }

            if ( aiming )
            {
                ToggleCrosshair( true, weaponHandler.currentWeapon );
                PositionCrosshair( aimRay, weaponHandler.currentWeapon );
            }
            else
                ToggleCrosshair( false, weaponHandler.currentWeapon );
        }
        else
            TurnOffAllCrosshairs();
    }

    void TurnOffAllCrosshairs()
    {
        foreach ( Weapon wep in crosshairPrefabMap.Keys )
        {
            ToggleCrosshair( false, wep );
        }
    }

    void CreateCrosshair( Weapon wep )
    {
        GameObject prefab = wep.weaponSettings.crosshairPrefab;
        if ( prefab != null )
        {
            prefab = Instantiate( prefab );
            ToggleCrosshair( false, wep );
        }
    }

    void DeleteCrosshair( Weapon wep )
    {
        if ( !crosshairPrefabMap.ContainsKey( wep ) )
            return;

        Destroy( crosshairPrefabMap[ wep ] );
        crosshairPrefabMap.Remove( wep );
    }

    // Position the crosshair to the point that we are aiming
    void PositionCrosshair( Ray ray, Weapon wep )
    {
        Weapon curWeapon = weaponHandler.currentWeapon;
        if ( curWeapon == null )
            return;
        if ( !crosshairPrefabMap.ContainsKey( wep ) )
            return;

        GameObject crosshairPrefab = crosshairPrefabMap[ wep ];
        RaycastHit hit;
        Transform bSpawn = curWeapon.weaponSettings.bulletSpawn;
        Vector3 bSpawnPoint = bSpawn.position;
        Vector3 dir = ray.GetPoint( curWeapon.weaponSettings.range ) - bSpawnPoint;

        if ( Physics.Raycast( bSpawnPoint, dir, out hit, curWeapon.weaponSettings.range,
            curWeapon.weaponSettings.bulletLayers ) )
        {
            if ( crosshairPrefab != null )
            {
                ToggleCrosshair( true, curWeapon );
                crosshairPrefab.transform.position = hit.point;
                crosshairPrefab.transform.LookAt( Camera.main.transform );
            }
        }
        else
        {
            ToggleCrosshair( false, curWeapon );
        }
    }

    // Toggle on and off the crosshair prefab
    void ToggleCrosshair( bool enabled, Weapon wep )
    {
        if ( !crosshairPrefabMap.ContainsKey( wep ) )
            return;

        crosshairPrefabMap[ wep ].SetActive( enabled );
    }

    void UpdateCrosshairs()
    {
        if ( weaponHandler.weaponsList.Count == 0 )
            return;

        foreach ( Weapon wep in weaponHandler.weaponsList )
        {
            if ( wep != weaponHandler.currentWeapon )
            {
                ToggleCrosshair( false, wep );
            }
        }
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
        Vector3 lookTarget = pivotPos + ( pivotT.forward * other.lookDistance );
        Vector3 thisPos = transform.position;
        Vector3 lookDir = lookTarget - thisPos;
        Quaternion lookRot = Quaternion.LookRotation( lookDir );
        lookRot.x = 0;
        lookRot.z = 0;

        Quaternion newRotation = Quaternion.Lerp( transform.rotation, lookRot, Time.deltaTime * other.lookSpeed );
        transform.rotation = newRotation;
    }
}
