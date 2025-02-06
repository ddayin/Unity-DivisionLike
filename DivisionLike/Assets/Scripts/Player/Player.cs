using UnityEngine;
using WanzyeeStudio;


namespace DivisionLike
{
    /// <summary>
    /// 플레이어의 component들을 가지고 있다.
    /// </summary>
    public class Player : BaseSingleton<Player>
    {
        [HideInInspector] public PlayerAnimation m_Animation;
        [HideInInspector] public PlayerStats m_Stats;
        [HideInInspector] public PlayerHealth m_Health;
        [HideInInspector] public PlayerInput m_UserInput;
        [HideInInspector] public PlayerInventory m_Inventory;
        [HideInInspector] public WeaponHandler m_WeaponHandler;
        [HideInInspector] public PlayerOutlineEffect m_OutlineEffect;

        protected override void Awake()
        {
            base.Awake();

            m_Animation = transform.GetComponent<PlayerAnimation>();
            m_Stats = transform.GetComponent<PlayerStats>();
            m_Health = transform.GetComponent<PlayerHealth>();
            m_UserInput = transform.GetComponent<PlayerInput>();
            m_Inventory = transform.GetComponent<PlayerInventory>();
            m_WeaponHandler = transform.GetComponent<WeaponHandler>();
            m_OutlineEffect = transform.GetComponent<PlayerOutlineEffect>();

            m_UserInput.enabled = true;
        }
    }
}