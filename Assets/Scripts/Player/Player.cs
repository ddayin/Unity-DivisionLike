using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DivisionLike
{
    public class Player : MonoBehaviour
    {
        public static Player instance = null;

        [HideInInspector] public PlayerStats m_Stats;
        [HideInInspector] public PlayerHealth m_Health;
        [HideInInspector] public UserInput m_UserInput;
        [HideInInspector] public PlayerInventory m_Inventory;
        [HideInInspector] public WeaponHandler m_WeaponHandler;
        [HideInInspector] public PlayerOutlineEffect m_OutlineEffect;

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

            m_Stats = transform.GetComponent<PlayerStats>();
            m_Health = transform.GetComponent<PlayerHealth>();
            m_UserInput = transform.GetComponent<UserInput>();
            m_Inventory = transform.GetComponent<PlayerInventory>();
            m_WeaponHandler = transform.GetComponent<WeaponHandler>();
            m_OutlineEffect = transform.GetComponent<PlayerOutlineEffect>();
        }
    }
}

