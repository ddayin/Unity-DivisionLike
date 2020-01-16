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

/*
 * reference - https://www.assetstore.unity3d.com/kr/#!/content/40756
 */

using UnityEngine;

namespace DivisionLike
{
    /// <summary>
    /// 게임 오버 관리자
    /// </summary>
    public class GameOverManager : MonoBehaviour
    {
        public bool m_IsImortalMode = false;
        private Animator m_Animator;                          // Reference to the animator component.

        void Awake()
        {
            // Set up the reference.
            m_Animator = GetComponent<Animator>();
        }

        void Update()
        {
            if ( m_IsImortalMode == true )
                return;

            // If the player has run out of health...
            if ( Player.instance.m_Stats.m_CurrentHealth <= 0 )
            {
                // ... tell the animator the game is over.
                m_Animator.SetTrigger( "GameOver" );
            }
        }
    }
}