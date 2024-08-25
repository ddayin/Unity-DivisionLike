using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DivisionLike
{
    /// <summary>
    /// 슈류탄 처리
    /// </summary>
    public class GrenadeHandler : MonoBehaviour
    {
        public GameObject m_GrenadePrefab;
        private GameObject m_GrenadeParent;

        [System.Serializable]
        public class UserSettings
        {
            public float m_ThrowForce = 100f;
        }

        public UserSettings m_UserSettings;

        void Awake()
        {
            m_GrenadeParent = GameObject.Find("GrenadeParent");
        }

        /// <summary>
        /// 슈류탄을 생성한다.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        public void CreateGrenade(Vector3 position, Quaternion rotation /*, Vector3 force */)
        {
            GameObject grenadeObj =
                (GameObject)Instantiate(m_GrenadePrefab, position, rotation, m_GrenadeParent.transform);

            grenadeObj.GetComponent<Rigidbody>()
                .AddForce(transform.forward * m_UserSettings.m_ThrowForce, ForceMode.Impulse);
        }
    }
}