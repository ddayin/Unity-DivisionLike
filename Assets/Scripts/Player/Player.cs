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
using WanzyeeStudio;


namespace DivisionLike
{
    /// <summary>
    /// 플레이어의 component들을 가지고 있다.
    /// </summary>
    public class Player : MonoBehaviour
    {
        public static Player instance
        {
            get { return Singleton<Player>.instance; }
        }

        [HideInInspector] public PlayerAnimation m_Animation;
        [HideInInspector] public PlayerStats m_Stats;
        [HideInInspector] public PlayerHealth m_Health;
        [HideInInspector] public PlayerInput m_UserInput;
        [HideInInspector] public PlayerInventory m_Inventory;
        [HideInInspector] public WeaponHandler m_WeaponHandler;
        [HideInInspector] public PlayerOutlineEffect m_OutlineEffect;

        void Awake()
        {
            switch ( SceneController.instance.m_CurrentScene )
            {
                case eSceneName.Intro:
                    {
                        m_Animation = transform.GetComponent<PlayerAnimation>();
                        m_Stats = transform.GetComponent<PlayerStats>();                        
                        m_Inventory = transform.GetComponent<PlayerInventory>();
                    }
                    break;
                case eSceneName.Play:
                    {
                        m_Animation = transform.GetComponent<PlayerAnimation>();
                        m_Stats = transform.GetComponent<PlayerStats>();
                        m_Health = transform.GetComponent<PlayerHealth>();
                        m_UserInput = transform.GetComponent<PlayerInput>();
                        m_Inventory = transform.GetComponent<PlayerInventory>();
                        m_WeaponHandler = transform.GetComponent<WeaponHandler>();
                        m_OutlineEffect = transform.GetComponent<PlayerOutlineEffect>();

                        m_UserInput.enabled = true;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}

