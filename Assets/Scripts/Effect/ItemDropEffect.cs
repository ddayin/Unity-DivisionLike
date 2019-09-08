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
    /// 아이템을 떨어뜨린 지점에 라인을 그린다.
    /// </summary>
    public class ItemDropEffect : MonoBehaviour
    {
        private LineRenderer m_LineUp;

        private void Awake()
        {
            m_LineUp = transform.GetComponent<LineRenderer>();
        }

        private void OnEnable()
        {
            m_LineUp.SetPosition( 0, transform.position );

            Vector3 onePosition = transform.position;
            onePosition.y = 100f;
            m_LineUp.SetPosition( 1, onePosition );
        }

        void OnTriggerEnter( Collider other )
        {
            // 플레이어가 총알을 획득한다.
            if ( other.gameObject.tag.Equals( "Player" ) == true )
            {
                int ammo = Random.Range( 1, 30 );
                
                Player.instance.m_Inventory.ObtainAmmo( ammo );
                //Destroy( gameObject );
                Lean.LeanPool.Despawn( gameObject );                
            }
        }

        void OnTriggerExit( Collider other )
        {
            
        }
    }
}