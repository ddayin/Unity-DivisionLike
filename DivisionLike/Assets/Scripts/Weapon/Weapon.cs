using UnityEngine;
using System.Collections;

namespace DivisionLike
{
    /// <summary>
    /// 무기
    /// </summary>
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class Weapon : MonoBehaviour
    {
        private Collider m_Collider;
        private Rigidbody m_RigidBody;
        private Animator m_Animator;

        public Light m_FaceLight;

        private float
            m_EffectsDisplayTime = 0.2f; // The proportion of the timeBetweenBullets that the effects will display for.

        private float m_Timer = 0f;

        public enum WeaponType
        {
            Primary = 0,
            Secondary,
            Sidearm
        }

        public WeaponType m_WeaponType;

        public class WeaponName
        {
            public string m_Makarov = "Makarov";
            public string m_ModernRussianAR = "Modern Russian AR";
            public string m_M4A1 = "M4A1";
        }

        public string m_WeaponName;

        [System.Serializable]
        public class UserSettings
        {
            public Transform leftHandIKTarget;
            public Vector3 spineRotation;
        }

        [SerializeField] public UserSettings m_UserSettings;

        [System.Serializable]
        public class WeaponSettings
        {
            [Header("-Bullet Options-")] public Transform bulletSpawn;
            public float damage = 40.0f;
            public float bulletSpread = 0.01f;
            public float fireRate = 0.2f;
            public LayerMask bulletLayers;
            public float range = 200.0f;

            [Header("-Effects-")] public GameObject muzzleFlash;
            public GameObject decal;
            public GameObject shell;
            public GameObject clip;
            public GameObject _bullet;
            public LineRenderer _bulletLine;

            [Header("-Other-")] public GameObject crosshairPrefab;
            public float reloadDuration = 2.0f;
            public Transform shellEjectSpot;
            public float shellEjectSpeed = 7.5f;
            public Transform clipEjectPos;
            public GameObject clipGO;
            public float crossHairSize;
            public float _goUpSpeed = 0.1f;

            [Header("-Positioning-")] public Vector3 equipPosition;
            public Vector3 equipRotation;
            public Vector3 unequipPosition;
            public Vector3 unequipRotation;

            [Header("-Animation-")] public bool useAnimation;
            public int fireAnimationLayer = 0;
            public string fireAnimationName = "Fire";
        }

        [SerializeField] public WeaponSettings m_WeaponSettings;

        [System.Serializable]
        public class Ammunition
        {
            public int carryingAmmo; // 모든 총알의 개수
            public int carryingMaxAmmo = 800;
            public int clipAmmo; // 장전되어 있는 총알의 개수 
            public int maxClipAmmo; // 장전될 수 있는 총알의 최대 개수
            public bool clipInfiniteAmmo = false; // 모든 총알의 개수가 무한개인지
        }

        [SerializeField] public Ammunition m_Ammo;

        private WeaponHandler m_Owner;
        private bool m_IsEquipped;
        public bool m_ResettingCartridge;

        [System.Serializable]
        public class SoundSettings
        {
            public AudioClip[] gunshotSounds;
            public AudioClip reloadSound;
            [Range(0, 3)] public float pitchMin = 1;
            [Range(0, 3)] public float pitchMax = 1.2f;
            public AudioSource audioS;
        }

        [SerializeField] public SoundSettings m_SoundSettings;

        private GameObject m_ShellParent;

        // Use this for initialization
        private void Start()
        {
            m_Collider = GetComponent<Collider>();
            m_RigidBody = GetComponent<Rigidbody>();
            m_Animator = GetComponent<Animator>();
            m_ShellParent = GameObject.Find("ShellParent");

            if (m_Ammo.clipInfiniteAmmo == true)
            {
                m_Ammo.carryingAmmo = int.MaxValue;
            }
        }

        // Update is called once per frame
        private void Update()
        {
            m_Timer += Time.deltaTime;

            if (m_Owner != null)
            {
                DisableEnableComponents(false);
                if (m_IsEquipped)
                {
                    if (m_Owner.m_UserSettings.m_RightHand != null)
                    {
                        Equip();
                    }
                }
                else
                {
                    if (m_WeaponSettings.bulletSpawn.childCount > 0)
                    {
                        foreach (Transform t in m_WeaponSettings.bulletSpawn.GetComponentsInChildren<Transform>())
                        {
                            if (t != m_WeaponSettings.bulletSpawn)
                            {
                                Destroy(t.gameObject);
                            }
                        }
                    }

                    Unequip(m_WeaponType);
                }
            }
            else
            {
                // If owner is null
                DisableEnableComponents(true);
                transform.SetParent(null);
            }

            // 총알 빛 효과 일정 시간 지나면 끈다.
            if (m_Timer >= m_EffectsDisplayTime)
            {
                m_FaceLight.enabled = false;
            }
        }

        private BoxCollider m_BoxCollider;

        /// <summary>
        /// MeshRenderer의 크기에 맞게 BoxCollider의 크기도 조정한다.
        /// </summary>
        private void FitBoxColliderSize()
        {
            MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
            m_BoxCollider.center = renderer.bounds.center;
            m_BoxCollider.size = renderer.bounds.size;
        }

        private RaycastHit m_Hit;
        private RaycastHit m_RagdollHit;

        /// <summary>
        /// This fires the weapon
        /// </summary>
        /// <param name="ray"></param>
        public void Fire(Ray ray)
        {
            if (m_Ammo.clipAmmo <= 0 || m_ResettingCartridge || !m_WeaponSettings.bulletSpawn || !m_IsEquipped)
                return;

            // 적 캐릭터 안에 총알 구멍이 들어있을 때 예외 처리
            int layerMask = LayerMask.GetMask("Ragdoll");

            Collider[] colliders =
                Physics.OverlapSphere(m_WeaponSettings.bulletSpawn.transform.position, 0.1f, layerMask);
            if (colliders.Length > 0)
            {
                // Try and find an EnemyHealth script on the gameobject hit.
                EnemyHealth enemyHealth = colliders[0].GetComponent<EnemyHealth>();

                // If the EnemyHealth component exist...
                if (enemyHealth != null)
                {
                    // ... the enemy should take damage.
                    enemyHealth.TakeDamage((int)Player.instance.m_Stats.CalculateDamage(),
                        colliders[0].transform.position);
                }

                // bullet line effect
                m_WeaponSettings._bulletLine.SetPosition(1, colliders[0].transform.position);
            }

            EZCameraShake.CameraShaker.Instance.ShakeOnce(0.5f, 0.5f, 0.1f, 0.1f);

            m_Timer = 0f;

            m_FaceLight.enabled = true;

            Transform bSpawn = m_WeaponSettings.bulletSpawn;
            Vector3 bSpawnPoint = bSpawn.position;

            // 거리를 재어서 dir를 계산해야 한다
            Vector3 dir = Vector3.zero;
            if (Physics.Raycast(ray, out m_Hit) == true)
            {
                dir = m_Hit.point - bSpawnPoint;
            }
            else
            {
                dir = ray.GetPoint(m_WeaponSettings.range) - bSpawnPoint;
            }

            // 총알이 랜덤하게 흩어진다
            dir += (Vector3)Random.insideUnitCircle * m_WeaponSettings.bulletSpread;

            m_WeaponSettings._bulletLine.enabled = true;
            m_WeaponSettings._bulletLine.SetPosition(0, m_WeaponSettings.bulletSpawn.transform.position);

            Invoke("DisableBulletLine", 0.1f);

            // 적 캐릭터를 맞추었을 때
            if (Physics.Raycast(bSpawnPoint, dir, out m_RagdollHit, m_WeaponSettings.range, layerMask))
            {
                OnEnemyHit();
            }

            // 적 캐릭터 제외
            if (Physics.Raycast(bSpawnPoint, dir, out m_Hit, Mathf.Infinity, m_WeaponSettings.bulletLayers) == true)
            {
                m_WeaponSettings._bulletLine.SetPosition(1, m_Hit.point);

                HitEffects(m_Hit);
            }
            // 충돌체가 없고 하늘을 향해 쏘았을 때
            else
            {
                m_WeaponSettings._bulletLine.SetPosition(1, dir);
            }

            // 기름 통
            layerMask = LayerMask.GetMask("Explodable");
            if (Physics.Raycast(bSpawnPoint, dir, out m_Hit, Mathf.Infinity, layerMask))
            {
                m_WeaponSettings._bulletLine.SetPosition(1, m_Hit.point);

                HitEffects(m_Hit);

                OilBarrel barrel = m_Hit.collider.GetComponent<OilBarrel>();
                if (barrel != null)
                {
                    barrel.TakeDamage((int)Player.instance.m_Stats.CalculateDamage());
                }
            }

            GunEffects();

            if (m_WeaponSettings.useAnimation)
                m_Animator.Play(m_WeaponSettings.fireAnimationName, m_WeaponSettings.fireAnimationLayer);

            m_Ammo.clipAmmo--;

            ScreenHUD.instance.SetAmmoSlider();

            m_ResettingCartridge = true;
            StartCoroutine(LoadNextBullet());
        }

        /// <summary>
        /// 적이 맞았을때 처리
        /// </summary>
        private void OnEnemyHit()
        {
            // Try and find an EnemyHealth script on the gameobject hit.
            EnemyHealth enemyHealth = m_RagdollHit.collider.GetComponent<EnemyHealth>();

            // If the EnemyHealth component exist...
            if (enemyHealth != null)
            {
                // ... the enemy should take damage.
                enemyHealth.TakeDamage((int)Player.instance.m_Stats.CalculateDamage(), m_RagdollHit.point);
            }

            // bullet line effect
            m_WeaponSettings._bulletLine.SetPosition(1, m_RagdollHit.point);
        }

        /// <summary>
        /// 
        /// </summary>
        private void DisableBulletLine()
        {
            m_WeaponSettings._bulletLine.enabled = false;
        }

        /// <summary>
        /// Loads the next bullet into the chamber
        /// </summary>
        /// <returns></returns>
        IEnumerator LoadNextBullet()
        {
            yield return new WaitForSeconds(m_WeaponSettings.fireRate);
            m_ResettingCartridge = false;
        }


        /// <summary>
        /// 맞았을 때의 이펙트가 생성되고 일정 시간 후에 삭제된다.
        /// </summary>
        /// <param name="hit"></param>
        void HitEffects(RaycastHit hit)
        {
            if (hit.collider.gameObject.isStatic)
            {
                if (m_WeaponSettings.decal)
                {
                    Vector3 hitPoint = hit.point;
                    Quaternion lookRotation = Quaternion.LookRotation(hit.normal);
                    GameObject decal = Instantiate(m_WeaponSettings.decal, hitPoint, lookRotation) as GameObject;
                    Transform decalT = decal.transform;
                    Transform hitT = hit.transform;
                    decalT.SetParent(hitT);
                    Destroy(decal, Random.Range(15.0f, 20.0f));
                }
            }
        }

        /// <summary>
        /// muzzle flash와 탄피가 튀어나온다.
        /// </summary>
        void GunEffects()
        {
            // muzzle flash
            if (m_WeaponSettings.muzzleFlash)
            {
                Vector3 bulletSpawnPos = m_WeaponSettings.bulletSpawn.position;
                GameObject muzzleFlash =
                    Instantiate(m_WeaponSettings.muzzleFlash, bulletSpawnPos, Quaternion.identity) as GameObject;
                Transform muzzleT = muzzleFlash.transform;
                muzzleT.SetParent(m_WeaponSettings.bulletSpawn);
                Destroy(muzzleFlash, 2.0f);
            }

            // ejecting shell
            if (m_WeaponSettings.shell)
            {
                if (m_WeaponSettings.shellEjectSpot)
                {
                    Vector3 shellEjectPos = m_WeaponSettings.shellEjectSpot.position;
                    Quaternion shellEjectRot = m_WeaponSettings.shellEjectSpot.rotation;

                    GameObject shell =
                        Lean.LeanPool.Spawn(m_WeaponSettings.shell, shellEjectPos, shellEjectRot) as GameObject;
                    shell.transform.SetParent(m_ShellParent.transform);


                    if (shell.GetComponent<Rigidbody>())
                    {
                        Rigidbody rigidB = shell.GetComponent<Rigidbody>();
                        rigidB.AddForce(m_WeaponSettings.shellEjectSpot.forward * m_WeaponSettings.shellEjectSpeed,
                            ForceMode.Impulse);
                    }

                    Lean.LeanPool.Despawn(shell, Random.Range(15.0f, 20.0f));
                }
            }

            PlayGunshotSound();
        }

        /// <summary>
        /// 발사된 탄환 사운드 재생
        /// </summary>
        void PlayGunshotSound()
        {
            if (m_SoundSettings.audioS != null)
            {
                if (m_SoundSettings.gunshotSounds.Length > 0)
                {
                    SoundController.instance.InstantiateClip(
                        m_WeaponSettings.bulletSpawn.position, // Where we want to play the sound from
                        m_SoundSettings.gunshotSounds[
                            Random.Range(0,
                                m_SoundSettings.gunshotSounds.Length)], // What audio clip we will use for this sound
                        2, // How long before we destroy the audio
                        true, // Do we want to randomize the sound?
                        m_SoundSettings.pitchMin, // The minimum pitch that the sound will use.
                        m_SoundSettings.pitchMax); // The maximum pitch that the sound will use.
                }
            }
        }


        /// <summary>
        /// Disables or enables collider and rigidbody
        /// </summary>
        /// <param name="enabled"></param>
        void DisableEnableComponents(bool enabled)
        {
            if (!enabled)
            {
                m_RigidBody.isKinematic = true;
                m_Collider.enabled = false;
            }
            else
            {
                m_RigidBody.isKinematic = false;
                m_Collider.enabled = true;
            }
        }


        /// <summary>
        /// Equips this weapon to the hand
        /// </summary>
        void Equip()
        {
            if (m_Owner == null) return;
            else if (m_Owner.m_UserSettings.m_RightHand == null) return;

            transform.SetParent(m_Owner.m_UserSettings.m_RightHand);

            transform.localPosition = m_WeaponSettings.equipPosition;

            Quaternion equipRot = Quaternion.Euler(m_WeaponSettings.equipRotation);
            transform.localRotation = equipRot;
        }


        /// <summary>
        /// Unequips the weapon and places it to the desired location
        /// </summary>
        /// <param name="wpType"></param>
        void Unequip(WeaponType wpType)
        {
            if (m_Owner == null)
                return;

            switch (wpType)
            {
                case WeaponType.Primary:
                    transform.SetParent(m_Owner.m_UserSettings.m_RifleUnequipSpot);
                    break;
                case WeaponType.Secondary:
                    transform.SetParent(m_Owner.m_UserSettings.m_PistolUnequipSpot);
                    break;
                case WeaponType.Sidearm:
                    break;
                default:
                    break;
            }

            transform.localPosition = m_WeaponSettings.unequipPosition;
            Quaternion unEquipRot = Quaternion.Euler(m_WeaponSettings.unequipRotation);
            transform.localRotation = unEquipRot;
        }


        /// <summary>
        /// Loads the clip and calculates the ammo
        /// </summary>
        public void LoadClip()
        {
            int ammoNeeded = m_Ammo.maxClipAmmo - m_Ammo.clipAmmo;

            if (ammoNeeded >= m_Ammo.carryingAmmo)
            {
                m_Ammo.clipAmmo = m_Ammo.carryingAmmo;
                m_Ammo.carryingAmmo = 0;
            }
            else
            {
                m_Ammo.carryingAmmo -= ammoNeeded;
                m_Ammo.clipAmmo = m_Ammo.maxClipAmmo;
            }
        }


        /// <summary>
        /// Sets the weapons equip state
        /// </summary>
        /// <param name="equip"></param>
        public void SetEquipped(bool equip)
        {
            m_IsEquipped = equip;
        }


        /// <summary>
        /// Sets the owner of this weapon
        /// </summary>
        /// <param name="wp"></param>
        public void SetOwner(WeaponHandler wp)
        {
            m_Owner = wp;
        }
    }
}