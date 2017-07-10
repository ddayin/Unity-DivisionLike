using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    public class EnemyStats : MonoBehaviour
    {
        public int level;
        public int armor;
        public int xpWhenDie;

        private Text xpText;   // show expWhenDie
        private Text damagedText;

        private void Awake()
        {
        
        
        }
    }
}

