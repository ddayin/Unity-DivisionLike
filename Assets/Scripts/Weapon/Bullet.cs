using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DivisionLike
{
    public class Bullet : MonoBehaviour
    {
        private void OnCollisionEnter( Collision collision )
        {
            Debug.Log( "destroy bullet" );
            //Destroy( this.gameObject );
        }
    }
}

