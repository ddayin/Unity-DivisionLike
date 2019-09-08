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

/*
 * reference - https://www.youtube.com/watch?v=fbUOG7f3jq8&t=19s
 */

using UnityEngine;
using System.Collections;


namespace DivisionLike
{
    /// <summary>
    /// 
    /// </summary>
    public class FloatingTextController : MonoBehaviour
    {
        public FloatingText m_PopupText;
        private GameObject m_Canvas;

        private void Awake()
        {
            m_Canvas = this.gameObject;
        }

        /// <summary>
        /// 떠다니는 텍스트를 지정한 위치에 생성한다.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="location"></param>
        public void CreateFloatingText( string text, Transform location )
        {
            FloatingText instance = Instantiate( m_PopupText );

            instance.transform.SetParent( m_Canvas.transform, false );

            Vector3 newPos = new Vector3( location.position.x + Random.Range( -0.3f, 0.3f ), location.position.y + Random.Range( -0.3f, 0.3f ), location.position.z );
            instance.transform.position = newPos;
            instance.SetText( text );
            //instance.SetColor();
        }
    }
}