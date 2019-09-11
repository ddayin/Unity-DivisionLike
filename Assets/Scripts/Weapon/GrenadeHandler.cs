/*
MIT License

Copyright (c) 2019 ddayin

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

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
            m_GrenadeParent = GameObject.Find( "GrenadeParent" );
        }
        
        /// <summary>
        /// 슈류탄을 생성한다.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        public void CreateGrenade( Vector3 position, Quaternion rotation /*, Vector3 force */ )
        {
            GameObject grenadeObj = (GameObject) Instantiate( m_GrenadePrefab, position, rotation, m_GrenadeParent.transform );

            grenadeObj.GetComponent<Rigidbody>().AddForce( transform.forward * m_UserSettings.m_ThrowForce, ForceMode.Impulse );

            
        }

        
    }
}