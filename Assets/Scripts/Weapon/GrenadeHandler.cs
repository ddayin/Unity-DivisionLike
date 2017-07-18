using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DivisionLike
{
    public class GrenadeHandler : MonoBehaviour
    {
        public GameObject _grenadePrefab;
        private GameObject _grenadeParent;

        [System.Serializable]
        public class UserSettings
        {
            public float _throwForce = 100f;
        }
        public UserSettings _userSettings;
        
        void Awake()
        {
            _grenadeParent = GameObject.Find( "GrenadeParent" );
        }

        // Update is called once per frame
        
        void Update()
        {
            
        }

        public void CreateGrenade( Vector3 position, Quaternion rotation /*, Vector3 force */ )
        {
            GameObject grenadeObj = (GameObject) Instantiate( _grenadePrefab, position, rotation, _grenadeParent.transform );

            grenadeObj.GetComponent<Rigidbody>().AddForce( transform.forward * _userSettings._throwForce, ForceMode.Impulse );

            
        }

        
    }
}