using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DivisionLike
{
    /// <summary>
    /// crosshair 처리
    /// </summary>
    public class CrosshairController : MonoBehaviour
    {
        private WeaponHandler m_WeaponHandler;
        private Camera m_TPSCamera;
        private Dictionary<Weapon, GameObject> m_CrosshairPrefabMap = new Dictionary<Weapon, GameObject>();

        private void Awake()
        {
            m_WeaponHandler = GetComponent<WeaponHandler>();
            m_TPSCamera = Camera.main;

            SetupCrosshairs();
        }

        /// <summary>
        /// 무기 별로 다른 crosshair 설정
        /// </summary>
        private void SetupCrosshairs()
        {
            if (m_WeaponHandler.m_WeaponsList.Count > 0)
            {
                foreach (Weapon wep in m_WeaponHandler.m_WeaponsList)
                {
                    GameObject prefab = wep.m_WeaponSettings.crosshairPrefab;
                    if (prefab != null)
                    {
                        GameObject clone = (GameObject)Instantiate(prefab);
                        clone.transform.SetParent(ScreenHUD.instance.transform);
                        clone.transform.localPosition = Vector3.zero;

                        m_CrosshairPrefabMap.Add(wep, clone);
                        ToggleCrosshair(false, wep);
                    }
                }
            }
        }

        private Vector3 m_FireDirection;
        private Vector3 m_FirePoint;

        /// <summary>
        /// crosshair가 캐릭터를 맞추었을 때 색상 처리
        /// </summary>
        private void HitCrosshair()
        {
            RaycastHit hit;
            int range = 1000;
            m_FireDirection = m_TPSCamera.transform.forward * 10;
            m_FirePoint = m_WeaponHandler.m_CurrentWeapon.m_WeaponSettings.bulletSpawn.position;
            // Debug the ray out in the editor:
            Debug.DrawRay(m_FirePoint, m_FireDirection, Color.green);

            CrosshairHandler crosshair = m_WeaponHandler.m_CurrentWeapon.m_WeaponSettings.crosshairPrefab
                .GetComponent<CrosshairHandler>();

            if (Physics.Raycast(m_FirePoint, (m_FireDirection), out hit, range))
            {
                if (hit.transform.gameObject.layer == LayerMask.GetMask("Ragdoll"))
                {
                    Debug.LogWarning("crosshair Red");
                    crosshair.ChangeColor(Color.red);
                }
                else
                {
                    Debug.Log("crosshair white");
                    crosshair.ChangeColor(Color.white);
                }
            }
            else
            {
                crosshair.ChangeColor(Color.white);
            }
        }

        /// <summary>
        /// 모든 crosshair들을 끈다.
        /// </summary>
        private void TurnOffAllCrosshairs()
        {
            foreach (Weapon wep in m_CrosshairPrefabMap.Keys)
            {
                ToggleCrosshair(false, wep);
            }
        }

        /// <summary>
        /// 해당 무기의 crosshair를 생성한다.
        /// </summary>
        /// <param name="wep"></param>
        private void CreateCrosshair(Weapon wep)
        {
            GameObject prefab = wep.m_WeaponSettings.crosshairPrefab;
            if (prefab != null)
            {
                prefab = Instantiate(prefab);
                prefab.transform.SetParent(ScreenHUD.instance.transform);
                ToggleCrosshair(false, wep);
            }
        }

        /// <summary>
        /// 해당 무기의 crosshair를 삭제한다.
        /// </summary>
        /// <param name="wep"></param>
        private void DeleteCrosshair(Weapon wep)
        {
            if (!m_CrosshairPrefabMap.ContainsKey(wep))
                return;

            Destroy(m_CrosshairPrefabMap[wep]);
            m_CrosshairPrefabMap.Remove(wep);
        }

        /// <summary>
        /// Toggle on and off the crosshair prefab
        /// </summary>
        /// <param name="enabled"></param>
        /// <param name="wep"></param>
        private void ToggleCrosshair(bool enabled, Weapon wep)
        {
            if (!m_CrosshairPrefabMap.ContainsKey(wep))
                return;

            m_CrosshairPrefabMap[wep].transform.localPosition = Vector3.zero;
            m_CrosshairPrefabMap[wep].SetActive(enabled);
        }

        /// <summary>
        /// crosshair들을 끈다.
        /// </summary>
        private void UpdateCrosshairs()
        {
            if (m_WeaponHandler.m_WeaponsList.Count == 0)
                return;

            foreach (Weapon wep in m_WeaponHandler.m_WeaponsList)
            {
                if (wep != m_WeaponHandler.m_CurrentWeapon)
                {
                    ToggleCrosshair(false, wep);
                }
            }
        }
    }
}