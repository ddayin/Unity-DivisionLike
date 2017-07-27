/*
 * reference - https://www.youtube.com/watch?v=gN1BmP4ONSQ&list=PLfxIz_UlKk7IwrcF2zHixNtFmh0lznXow&t=4140s&index=4
 */

using UnityEngine;
using System.Collections;

namespace DivisionLike
{
    [RequireComponent( typeof( Collider ) )]
    [RequireComponent( typeof( Rigidbody ) )]
    public class Weapon : MonoBehaviour
    {
        private Collider _collider;
        private Rigidbody _rigidBody;
        private Animator _animator;

        public Light faceLight;
        private float _effectsDisplayTime = 0.2f;                // The proportion of the timeBetweenBullets that the effects will display for.
        private float _timer = 0f;

        public enum WeaponType
        {
            Primary = 0,
            Secondary,
            Sidearm
        }
        public WeaponType _weaponType;

        public class WeaponName
        {
            public string _Makarov = "Makarov";
            public string _ModernRussianAR = "Modern Russian AR";
            public string _M4A1 = "M4A1";
        }
        public string _weaponName;

        [System.Serializable]
        public class UserSettings
        {
            public Transform leftHandIKTarget;
            public Vector3 spineRotation;
        }
        [SerializeField]
        public UserSettings userSettings;

        [System.Serializable]
        public class WeaponSettings
        {
            [Header( "-Bullet Options-" )]
            public Transform bulletSpawn;
            public float damage = 40.0f;
            public float bulletSpread = 5.0f;
            public float fireRate = 0.2f;
            public LayerMask bulletLayers;
            public float range = 200.0f;

            [Header( "-Effects-" )]
            public GameObject muzzleFlash;
            public GameObject decal;
            public GameObject shell;
            public GameObject clip;
            public GameObject _bullet;
            public LineRenderer _bulletLine;

            [Header( "-Other-" )]
            public GameObject crosshairPrefab;
            public float reloadDuration = 2.0f;
            public Transform shellEjectSpot;
            public float shellEjectSpeed = 7.5f;
            public Transform clipEjectPos;
            public GameObject clipGO;
            public float crossHairSize;
            public float _goUpSpeed = 0.1f;
            
            [Header( "-Positioning-" )]
            public Vector3 equipPosition;
            public Vector3 equipRotation;
            public Vector3 unequipPosition;
            public Vector3 unequipRotation;

            [Header( "-Animation-" )]
            public bool useAnimation;
            public int fireAnimationLayer = 0;
            public string fireAnimationName = "Fire";
        }
        [SerializeField]
        public WeaponSettings weaponSettings;

        [System.Serializable]
        public class Ammunition
        {
            public int carryingAmmo;    // 모든 총알의 개수
            public int carryingMaxAmmo = 800;
            public int clipAmmo;        // 장전되어 있는 총알의 개수 
            public int maxClipAmmo;     // 장전될 수 있는 총알의 최대 개수
            public bool clipInfiniteAmmo = false;    // 모든 총알의 개수가 무한개인지
        }
        [SerializeField]
        public Ammunition ammo;

        private WeaponHandler _owner;
        private bool _isEquipped;
        public bool _resettingCartridge;

        [System.Serializable]
        public class SoundSettings
        {
            public AudioClip[] gunshotSounds;
            public AudioClip reloadSound;
            [Range( 0, 3 )] public float pitchMin = 1;
            [Range( 0, 3 )] public float pitchMax = 1.2f;
            public AudioSource audioS;
        }
        [SerializeField]
        public SoundSettings soundSettings;

        private GameObject _shellParent;
       
        // Use this for initialization
        private void Start()
        {
            _collider = GetComponent<Collider>();
            _rigidBody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _shellParent = GameObject.Find( "ShellParent" );
            
            if ( ammo.clipInfiniteAmmo == true )
            {
                ammo.carryingAmmo = int.MaxValue;
            }
            
        }

        // Update is called once per frame
        private void Update()
        {
            _timer += Time.deltaTime;

            if ( _owner )
            {
                DisableEnableComponents( false );
                if ( _isEquipped )
                {
                    if ( _owner.userSettings.rightHand )
                    {
                        Equip();
                    }
                }
                else
                {
                    if ( weaponSettings.bulletSpawn.childCount > 0 )
                    {
                        foreach ( Transform t in weaponSettings.bulletSpawn.GetComponentsInChildren<Transform>() )
                        {
                            if ( t != weaponSettings.bulletSpawn )
                            {
                                Destroy( t.gameObject );
                            }
                        }
                    }
                    Unequip( _weaponType );
                }
            }
            else
            { // If owner is null
                DisableEnableComponents( true );
                transform.SetParent( null );
            }

            // 총알 빛 효과 일정 시간 지나면 끈다.
            if ( _timer >= _effectsDisplayTime )
            {
                faceLight.enabled = false;
            }
        }
        
        private BoxCollider boxCollider;

        // MeshRenderer의 크기에 맞게 BoxCollider의 크기도 조정한다.
        private void FitBoxColliderSize()
        {
            MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
            boxCollider.center = renderer.bounds.center;
            boxCollider.size = renderer.bounds.size;
        }

        //This fires the weapon
        private RaycastHit _hit;
        private RaycastHit _ragdollHit;

        public void Fire( Ray ray )
        {
            if ( ammo.clipAmmo <= 0 || _resettingCartridge || !weaponSettings.bulletSpawn || !_isEquipped )
                return;

            EZCameraShake.CameraShaker.Instance.ShakeOnce( 0.5f, 0.5f, 0.1f, 0.1f );

            _timer = 0f;

            faceLight.enabled = true;
            
            Transform bSpawn = weaponSettings.bulletSpawn;
            Vector3 bSpawnPoint = bSpawn.position;
            
            // 거리를 재어서 dir를 계산해야 한다
            Vector3 dir = Vector3.zero;
            if ( Physics.Raycast( ray, out _hit ) == true )
            {
                dir = _hit.point - bSpawnPoint;
            }
            else
            {
                dir = ray.GetPoint( weaponSettings.range ) - bSpawnPoint;
            }
            
            // 총알이 랜덤하게 흩어진다
            dir += (Vector3) Random.insideUnitCircle * weaponSettings.bulletSpread;

            weaponSettings._bulletLine.enabled = true;
            weaponSettings._bulletLine.SetPosition( 0, weaponSettings.bulletSpawn.transform.position );

            Invoke( "DisableBulletLine", 0.1f );

            // 적 캐릭터를 맞추었을 때
            int layerMask = LayerMask.GetMask( "Ragdoll" );
            if ( Physics.Raycast( bSpawnPoint, dir, out _ragdollHit, weaponSettings.range, layerMask ) )
            {
                // Try and find an EnemyHealth script on the gameobject hit.
                EnemyHealth enemyHealth = _ragdollHit.collider.GetComponent<EnemyHealth>();

                // If the EnemyHealth component exist...
                if ( enemyHealth != null )
                {
                    // ... the enemy should take damage.
                    enemyHealth.TakeDamage( (int) Player.instance._stats.CalculateDamage(), _ragdollHit.point );
                }
                
                // bullet line effect
                weaponSettings._bulletLine.SetPosition( 1, _ragdollHit.point );
            }

            // 적 캐릭터 제외
            if ( Physics.Raycast( bSpawnPoint, dir, out _hit, Mathf.Infinity, weaponSettings.bulletLayers ) )
            {
                weaponSettings._bulletLine.SetPosition( 1, _hit.point );

                HitEffects( _hit );
            }

            // 기름 통
            layerMask = LayerMask.GetMask( "Explodable" );
            if ( Physics.Raycast( bSpawnPoint, dir, out _hit, Mathf.Infinity, layerMask ) )
            {
                weaponSettings._bulletLine.SetPosition( 1, _hit.point );

                HitEffects( _hit );

                OilBarrel barrel = _hit.collider.GetComponent<OilBarrel>();
                if ( barrel != null )
                {
                    barrel.TakeDamage( (int) Player.instance._stats.CalculateDamage() );
                }
            }

            GunEffects();

            if ( weaponSettings.useAnimation )
                _animator.Play( weaponSettings.fireAnimationName, weaponSettings.fireAnimationLayer );

            ammo.clipAmmo--;

            ScreenHUD.instance.SetAmmoSlider();

            _resettingCartridge = true;
            StartCoroutine( LoadNextBullet() );
            
        }
        
        private void DisableBulletLine()
        {
            weaponSettings._bulletLine.enabled = false;
        }

        //Loads the next bullet into the chamber
        IEnumerator LoadNextBullet()
        {
            yield return new WaitForSeconds( weaponSettings.fireRate );
            _resettingCartridge = false;
        }

        

        void HitEffects( RaycastHit hit )
        {
            if ( hit.collider.gameObject.isStatic )
            {
                if ( weaponSettings.decal )
                {
                    Vector3 hitPoint = hit.point;
                    Quaternion lookRotation = Quaternion.LookRotation( hit.normal );
                    GameObject decal = Instantiate( weaponSettings.decal, hitPoint, lookRotation ) as GameObject;
                    Transform decalT = decal.transform;
                    Transform hitT = hit.transform;
                    decalT.SetParent( hitT );
                    Destroy( decal, Random.Range( 15.0f, 20.0f ) );
                }
            }
        }

        void GunEffects()
        {
            // muzzle flash
            if ( weaponSettings.muzzleFlash )
            {
                Vector3 bulletSpawnPos = weaponSettings.bulletSpawn.position;
                GameObject muzzleFlash = Instantiate( weaponSettings.muzzleFlash, bulletSpawnPos, Quaternion.identity ) as GameObject;
                Transform muzzleT = muzzleFlash.transform;
                muzzleT.SetParent( weaponSettings.bulletSpawn );
                Destroy( muzzleFlash, 2.0f );
            }

            // ejecting shell
            if ( weaponSettings.shell )
            {
                if ( weaponSettings.shellEjectSpot )
                {
                    Vector3 shellEjectPos = weaponSettings.shellEjectSpot.position;
                    Quaternion shellEjectRot = weaponSettings.shellEjectSpot.rotation;
                    //GameObject shell = Instantiate( weaponSettings.shell, shellEjectPos, shellEjectRot ) as GameObject;
                    GameObject shell = Lean.LeanPool.Spawn( weaponSettings.shell, shellEjectPos, shellEjectRot ) as GameObject;
                    shell.transform.SetParent( _shellParent.transform );
                    //shell.transform.localScale = this.transform.localScale;

                    if ( shell.GetComponent<Rigidbody>() )
                    {
                        Rigidbody rigidB = shell.GetComponent<Rigidbody>();
                        rigidB.AddForce( weaponSettings.shellEjectSpot.forward * weaponSettings.shellEjectSpeed, ForceMode.Impulse );
                    }

                    //Destroy( shell, Random.Range( 15.0f, 20.0f ) );
                    Lean.LeanPool.Despawn( shell, Random.Range( 15.0f, 20.0f ) );
                }
            }

            // bullet
            //if ( weaponSettings._bullet != null )
            //{
            //    GameObject bulletObj = (GameObject) Instantiate( weaponSettings._bullet, weaponSettings.bulletSpawn.transform.position, weaponSettings.bulletSpawn.rotation );

            //    //bulletObj.transform.SetParent( weaponSettings.bulletSpawn );

            //    //Vector3 firePos = CameraControl.instance._mainCamera.ScreenToWorldPoint( Input.mousePosition );

            //    bulletObj.GetComponent<Rigidbody>().velocity = bulletObj.transform.forward * 10f;
            //    //bulletObj.GetComponent<Rigidbody>().AddForce();

            //}
            
            PlayGunshotSound();
        }
        

        void PlayGunshotSound()
        {

            if ( soundSettings.audioS != null )
            {
                if ( soundSettings.gunshotSounds.Length > 0 )
                {
                    SoundController.instance.InstantiateClip(
                        weaponSettings.bulletSpawn.position, // Where we want to play the sound from
                        soundSettings.gunshotSounds[ Random.Range( 0, soundSettings.gunshotSounds.Length ) ],  // What audio clip we will use for this sound
                        2, // How long before we destroy the audio
                        true, // Do we want to randomize the sound?
                        soundSettings.pitchMin, // The minimum pitch that the sound will use.
                        soundSettings.pitchMax ); // The maximum pitch that the sound will use.
                }
            }
        }

        //Disables or enables collider and rigidbody
        void DisableEnableComponents( bool enabled )
        {
            if ( !enabled )
            {
                _rigidBody.isKinematic = true;
                _collider.enabled = false;
            }
            else
            {
                _rigidBody.isKinematic = false;
                _collider.enabled = true;
            }
        }

        //Equips this weapon to the hand
        void Equip()
        {
            if ( _owner == null )
                return;
            else if ( _owner.userSettings.rightHand == false )
                return;
            
            transform.SetParent( _owner.userSettings.rightHand );
            
            transform.localPosition = weaponSettings.equipPosition;
            
            Quaternion equipRot = Quaternion.Euler( weaponSettings.equipRotation );
            transform.localRotation = equipRot;
        }

        //Unequips the weapon and places it to the desired location
        void Unequip( WeaponType wpType )
        {
            if ( _owner == null )
                return;

            switch ( wpType )
            {
                case WeaponType.Primary:
                    transform.SetParent( _owner.userSettings.rifleUnequipSpot );
                    break;
                case WeaponType.Secondary:
                    transform.SetParent( _owner.userSettings.pistolUnequipSpot );
                    break;
                case WeaponType.Sidearm:
                    break;
                default:
                    break;
            }
            transform.localPosition = weaponSettings.unequipPosition;
            Quaternion unEquipRot = Quaternion.Euler( weaponSettings.unequipRotation );
            transform.localRotation = unEquipRot;
        }

        //Loads the clip and calculates the ammo
        public void LoadClip()
        {
            int ammoNeeded = ammo.maxClipAmmo - ammo.clipAmmo;

            if ( ammoNeeded >= ammo.carryingAmmo )
            {
                ammo.clipAmmo = ammo.carryingAmmo;
                ammo.carryingAmmo = 0;
            }
            else
            {
                ammo.carryingAmmo -= ammoNeeded;
                ammo.clipAmmo = ammo.maxClipAmmo;
            }
        }

        //Sets the weapons equip state
        public void SetEquipped( bool equip )
        {
            _isEquipped = equip;
        }

        //Sets the owner of this weapon
        public void SetOwner( WeaponHandler wp )
        {
            _owner = wp;
        }
        
    }
}