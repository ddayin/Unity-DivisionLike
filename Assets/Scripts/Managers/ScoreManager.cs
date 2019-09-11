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
 * reference - https://www.assetstore.unity3d.com/kr/#!/content/40756
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace DivisionLike
{
    /// <summary>
    /// 점수 관리자
    /// </summary>
    public class ScoreManager : MonoBehaviour
    {
        public static int m_Score;        // The player's score.
        
        private Text m_Text;                      // Reference to the Text component.


        void Awake()
        {
            // Set up the reference.
            m_Text = GetComponent<Text>();

            // Reset the score.
            m_Score = 0;
        }


        void Update()
        {
            // Set the displayed text to be the word "Score" followed by the score value.
            m_Text.text = "Score: " + m_Score;
        }
    }
}