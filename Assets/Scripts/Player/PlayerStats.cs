using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DivisionLike
{
    public class PlayerStats : MonoBehaviour
    {
        public int _maxHealth = 1200;                            // The amount of health the player starts the game with.
        public int _currentHealth;                                   // The current health the player has.
        public uint _currentLevel = 1;
        public const uint _maxLevel = 30;
        public ulong _currentXP = 0;
        public ulong[] _xpRequire = new ulong[ 30 ];
        public uint _currentArmor = 0;

        private void Awake()
        {
            _currentHealth = _maxHealth;

            SetDummyxpRequire();
        }

        private void SetDummyxpRequire()
        {
            // FIXME: hard coding temporarily...

            _xpRequire[ 0 ] = 1234;
            _xpRequire[ 1 ] = 12345;
            _xpRequire[ 2 ] = 123456;
            _xpRequire[ 3 ] = 1234567;
            _xpRequire[ 4 ] = 12345678;
            _xpRequire[ 5 ] = 123456789;
            _xpRequire[ 6 ] = 123456789;
            _xpRequire[ 7 ] = 123456789;
            _xpRequire[ 8 ] = 123456789;
            _xpRequire[ 9 ] = 123456789;
            _xpRequire[ 10 ] = 123456789;
            _xpRequire[ 11 ] = 123456789;
            _xpRequire[ 12 ] = 123456789;
            _xpRequire[ 13 ] = 123456789;
            _xpRequire[ 14 ] = 123456789;
            _xpRequire[ 15 ] = 123456789;
            _xpRequire[ 16 ] = 123456789;
            _xpRequire[ 17 ] = 123456789;
            _xpRequire[ 18 ] = 123456789;
            _xpRequire[ 19 ] = 123456789;
            _xpRequire[ 20 ] = 123456789;
            _xpRequire[ 21 ] = 123456789;
            _xpRequire[ 22 ] = 123456789;
            _xpRequire[ 23 ] = 123456789;
            _xpRequire[ 24 ] = 123456789;
            _xpRequire[ 25 ] = 123456789;
            _xpRequire[ 26 ] = 123456789;
            _xpRequire[ 27 ] = 123456789;
            _xpRequire[ 28 ] = 123456789;
            _xpRequire[ 29 ] = 123456789;

        }

        public void CheckLevel()
        {
            if ( _currentXP >= _xpRequire[ _currentLevel - 1 ] )
            {
                if ( _currentLevel < _maxLevel )
                {
                    _currentLevel++;
                    
                    _currentXP = 0;

                    Debug.Log( "[Level Up] Level = " + _currentLevel + " xp = " + _currentXP );

                    ScreenHUD.instance.SetLevelText();
                }
            }
            
        }
    }
}