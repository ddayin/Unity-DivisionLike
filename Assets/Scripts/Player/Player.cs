using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DivisionLike
{
    public class Player : MonoBehaviour
    {
        public static Player instance = null;
        public WeaponHandler weaponHandler;

        void Awake()
        {
            Debug.Log( "Player Awake()" );
            if ( instance == null )
            {
                instance = this;
            }
            else if ( instance != null )
            {
                Destroy( gameObject );
            }

            DontDestroyOnLoad( gameObject );

            weaponHandler = transform.GetComponent<WeaponHandler>();
        }
    }
}

