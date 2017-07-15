using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DivisionLike
{
    public class Player : MonoBehaviour
    {
        public static Player instance = null;

        [HideInInspector]
        public PlayerStats _stats;

        [HideInInspector]
        public PlayerHealth _health;

        [HideInInspector]
        public UserInput _userInput;

        [HideInInspector]
        public PlayerInventory _inventory;

        [HideInInspector]
        public WeaponHandler _weaponHandler;

        [HideInInspector]
        public PlayerOutlineEffect _outlineEffect;

        void Awake()
        {
            if ( instance == null )
            {
                instance = this;
            }
            else if ( instance != null )
            {
                Destroy( gameObject );
            }

            DontDestroyOnLoad( gameObject );

            _stats = transform.GetComponent<PlayerStats>();
            _health = transform.GetComponent<PlayerHealth>();
            _userInput = transform.GetComponent<UserInput>();
            _inventory = transform.GetComponent<PlayerInventory>();
            _weaponHandler = transform.GetComponent<WeaponHandler>();
            _outlineEffect = transform.GetComponent<PlayerOutlineEffect>();
        }
    }
}

