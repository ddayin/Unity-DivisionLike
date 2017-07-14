using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DivisionLike
{
    public class Player : MonoBehaviour
    {
        public static Player instance = null;

        public PlayerStats stats;
        public PlayerHealth health;
        public UserInput userInput;
        public PlayerInventory inventory;
        public WeaponHandler weaponHandler;
        public PlayerOutlineEffect outlineEffect;

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

            stats = transform.GetComponent<PlayerStats>();
            health = transform.GetComponent<PlayerHealth>();
            userInput = transform.GetComponent<UserInput>();
            inventory = transform.GetComponent<PlayerInventory>();
            weaponHandler = transform.GetComponent<WeaponHandler>();
            outlineEffect = transform.GetComponent<PlayerOutlineEffect>();
        }
    }
}

