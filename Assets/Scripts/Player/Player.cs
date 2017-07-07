using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DivisionLike
{
    public class Player : MonoBehaviour
    {
        public static Player instance = null;

        public UserInput userInput;
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


            userInput = transform.GetComponent<UserInput>();
            weaponHandler = transform.GetComponent<WeaponHandler>();
        }
    }
}

