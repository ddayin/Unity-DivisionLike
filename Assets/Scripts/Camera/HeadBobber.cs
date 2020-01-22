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

using UnityEngine;
using System.Collections;

namespace HeadBobber
{
    public class HeadBobber : MonoBehaviour
    {
        private float timer = 0.0f;
        float bobbingSpeed = 0.18f;
        float bobbingAmount = 0.2f;

        void Update()
        {
            float waveslice = 0.0f;
            float horizontal = Input.GetAxis( "Horizontal" );
            float vertical = Input.GetAxis( "Vertical" );

            Vector3 cSharpConversion = transform.localPosition;

            if ( Mathf.Abs( horizontal ) == 0 && Mathf.Abs( vertical ) == 0 )
            {
                timer = 0.0f;
            }
            else
            {
                waveslice = Mathf.Sin( timer );
                timer = timer + bobbingSpeed;
                if ( timer > Mathf.PI * 2 )
                {
                    timer = timer - ( Mathf.PI * 2 );
                }
            }
            if ( waveslice != 0 )
            {
                float translateChange = waveslice * bobbingAmount;
                float totalAxes = Mathf.Abs( horizontal ) + Mathf.Abs( vertical );
                totalAxes = Mathf.Clamp( totalAxes, 0.0f, 1.0f );
                translateChange = totalAxes * translateChange;
                cSharpConversion.y = cSharpConversion.y + translateChange;
            }
            
            transform.localPosition = cSharpConversion;
        }
    }
}