using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DivisionLike
{
    public class CrosshairController : MonoBehaviour
    {
        private WeaponHandler _weaponHandler;
        private Camera _TPSCamera;
        private Dictionary<Weapon, GameObject> _crosshairPrefabMap = new Dictionary<Weapon, GameObject>();

        private void Awake()
        {
            
        }

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
                    Debug.LogWarning( "crosshair Red" );
                    crosshair.ChangeColor( Color.red );
                }
                else
                {
                    Debug.Log( "crosshair white" );
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
    }
}