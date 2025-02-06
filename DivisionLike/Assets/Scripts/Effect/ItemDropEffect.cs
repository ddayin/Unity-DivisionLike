using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DivisionLike
{
    /// <summary>
    /// 아이템을 떨어뜨린 지점에 라인을 그린다.
    /// </summary>
    public class ItemDropEffect : MonoBehaviour
    {
        private LineRenderer m_LineUp;

        private void Awake()
        {
            m_LineUp = transform.GetComponent<LineRenderer>();
        }

        private void OnEnable()
        {
            m_LineUp.SetPosition(0, transform.position);

            Vector3 onePosition = transform.position;
            onePosition.y = 100f;
            m_LineUp.SetPosition(1, onePosition);
        }

        void OnTriggerEnter(Collider other)
        {
            // 플레이어가 총알을 획득한다.
            if (other.gameObject.tag.Equals("Player") == true)
            {
                int ammo = Random.Range(1, 30);

                Player.instance.m_Inventory.ObtainAmmo(ammo);
                //Destroy( gameObject );
                Lean.LeanPool.Despawn(gameObject);
            }
        }

        void OnTriggerExit(Collider other)
        {
        }
    }
}