/*
MIT License

Copyright (c) 2020 ddayin

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
    /// 탄약 상자
    /// </summary>
    public class AmmoBox : MonoBehaviour
    {
        private cakeslice.Outline m_Outline;
        private float m_Timer = 0f;
        public float m_KeyPressTime = 3f;

        public enum AmmoBoxState
        {
            OutOfRange = 0,     // color green
            InRange,            // color blue
            Emtpy               // color red
        }

        public AmmoBoxState m_State = AmmoBoxState.OutOfRange;

        private void Awake()
        {
            m_Outline = transform.GetComponent<cakeslice.Outline>();
            m_Outline.enabled = true;
        }
        
        // Update is called once per frame
        void Update()
        {
            CheckInput();
            SetOutlineColor();
        }

        private void OnTriggerEnter( Collider other )
        {
            if ( m_State == AmmoBoxState.Emtpy)
            {
                return;
            }

            m_State = AmmoBoxState.InRange;
        }

        private void OnTriggerStay( Collider other )
        {

        }

        private void OnTriggerExit( Collider other )
        {
            if ( m_State == AmmoBoxState.Emtpy )
            {
                return;
            }

            m_State = AmmoBoxState.OutOfRange;
        }

        

        /// <summary>
        /// F 키를 눌러 탄약을 가득 채운다.
        /// </summary>
        private void CheckInput()
        {
            if ( m_State == AmmoBoxState.OutOfRange || m_State == AmmoBoxState.Emtpy )
            {
                return;
            }

            if ( Input.GetKey( KeyCode.F ) == true )
            {
                m_Timer += Time.deltaTime;

                ScreenHUD.instance.SetEnableLoadingCircle( true );

                float amount = m_Timer / m_KeyPressTime;
                ScreenHUD.instance.SetLoadingCircle( amount );

                if ( m_Timer > m_KeyPressTime )
                {
                    Debug.Log( "ammo box give you full ammo" );

                    m_Timer = 0f;

                    ScreenHUD.instance.SetEnableLoadingCircle( false );

                    // full ammo
                    for ( int i = 0; i < Player.instance.m_WeaponHandler.m_WeaponsList.Count; i++ )
                    {
                        Player.instance.m_WeaponHandler.m_WeaponsList[ i ].m_Ammo.carryingAmmo = Player.instance.m_WeaponHandler.m_WeaponsList[ i ].m_Ammo.carryingMaxAmmo;
                    }

                    m_State = AmmoBoxState.Emtpy;
                }
            }
            else
            {
                m_Timer = 0f;
                ScreenHUD.instance.InitLoadingCircle();
            }
        }
        
        /// <summary>
        /// 아웃라인의 색상을 설정한다.
        /// </summary>
        private void SetOutlineColor()
        {
            switch( m_State )
            {
                case AmmoBoxState.OutOfRange:
                    m_Outline.color = 1;
                    break;

                case AmmoBoxState.InRange:
                    m_Outline.color = 2;
                    break;

                case AmmoBoxState.Emtpy:
                    m_Outline.color = 0;
                    break;

                default:
                    break;
            }
        }
    }
}


