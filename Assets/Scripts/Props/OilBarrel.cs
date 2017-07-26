using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DivisionLike
{
    public class OilBarrel : MonoBehaviour
    {
        public int _health = 100;

        private void Awake()
        {
            
        }
        

        // Update is called once per frame
        void Update()
        {

        }

        private void Explode()
        {
            Destroy( this.gameObject, 2f );
        }
    }
}


