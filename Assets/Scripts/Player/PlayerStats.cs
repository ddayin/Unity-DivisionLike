using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DivisionLike
{
    public class PlayerStats : MonoBehaviour
    {
        public int level = 1;
        public const int maxLevel = 30;
        public ulong xp = 0;
        public ulong[] xpRequire = new ulong[ 30 ];
        public int armor = 0;

        private void Awake()
        {
            SetDummyxpRequire();
        }

        private void SetDummyxpRequire()
        {
            // FIXME: hard coding temporarily...

            xpRequire[ 0 ] = 1234;
            xpRequire[ 1 ] = 12345;
            xpRequire[ 2 ] = 123456;
            xpRequire[ 3 ] = 1234567;
            xpRequire[ 4 ] = 12345678;
            xpRequire[ 5 ] = 123456789;
            xpRequire[ 6 ] = 123456789;
            xpRequire[ 7 ] = 123456789;
            xpRequire[ 8 ] = 123456789;
            xpRequire[ 9 ] = 123456789;
            xpRequire[ 10 ] = 123456789;
            xpRequire[ 11 ] = 123456789;
            xpRequire[ 12 ] = 123456789;
            xpRequire[ 13 ] = 123456789;
            xpRequire[ 14 ] = 123456789;
            xpRequire[ 15 ] = 123456789;
            xpRequire[ 16 ] = 123456789;
            xpRequire[ 17 ] = 123456789;
            xpRequire[ 18 ] = 123456789;
            xpRequire[ 19 ] = 123456789;
            xpRequire[ 20 ] = 123456789;
            xpRequire[ 21 ] = 123456789;
            xpRequire[ 22 ] = 123456789;
            xpRequire[ 23 ] = 123456789;
            xpRequire[ 24 ] = 123456789;
            xpRequire[ 25 ] = 123456789;
            xpRequire[ 26 ] = 123456789;
            xpRequire[ 27 ] = 123456789;
            xpRequire[ 28 ] = 123456789;
            xpRequire[ 29 ] = 123456789;

        }

        public void CheckLevel()
        {
            if ( xp >= xpRequire[ level - 1 ] )
            {
                if ( level < maxLevel )
                {
                    level++;
                    
                    xp = 0;

                    Debug.LogWarning( "level = " + level + " xp = " + xp );

                    ScreenHUD.instance.SetLevelText();
                }
            }
            
        }
    }
}