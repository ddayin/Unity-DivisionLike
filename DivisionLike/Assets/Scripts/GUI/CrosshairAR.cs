using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    /// <summary>
    /// AR 총의 crosshair
    /// </summary>
    public class CrosshairAR : CrosshairHandler
    {
        public Image[] m_CrosshairImages;
        private float m_WalkSize;

        private void Awake()
        {
            gameObject.SetActive(false);

            m_WalkSize = m_CrosshairImages[0].rectTransform.localPosition.y;
        }

        private void OnEnable()
        {
            m_WalkSize = 10f;
        }

        /// <summary>
        /// 색상을 변경한다.
        /// </summary>
        /// <param name="color"></param>
        public override void ChangeColor(Color color)
        {
            //Debug.Log( "CrosshairAR.ChangeColor() overrided" );
            for (int i = 0; i < 4; i++)
            {
                m_CrosshairImages[i].color = color;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateCrosshair()
        {
            // y+ x+ x- y- 
            float crossHairSize = calculateCrossHair();

            m_CrosshairImages[0].rectTransform.localPosition = Vector3.Slerp(
                m_CrosshairImages[0].rectTransform.localPosition,
                new Vector3(0f, crossHairSize, 0f), Time.deltaTime * 8f);
            m_CrosshairImages[1].rectTransform.localPosition = Vector3.Slerp(
                m_CrosshairImages[1].rectTransform.localPosition,
                new Vector3(crossHairSize, 0f, 0f), Time.deltaTime * 8f);
            m_CrosshairImages[2].rectTransform.localPosition = Vector3.Slerp(
                m_CrosshairImages[2].rectTransform.localPosition,
                new Vector3(-crossHairSize, 0f, 0f), Time.deltaTime * 8f);
            m_CrosshairImages[3].rectTransform.localPosition = Vector3.Slerp(
                m_CrosshairImages[3].rectTransform.localPosition,
                new Vector3(0f, -crossHairSize, 0f), Time.deltaTime * 8f);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public float calculateCrossHair()
        {
            float size = m_WalkSize * Player.instance.m_WeaponHandler.m_CurrentWeapon.m_WeaponSettings.crossHairSize;

            if (Player.instance.m_UserInput.m_IsSprinting == true)
            {
                size *= 2;
            }
            else
            {
                size /= 2;
            }

            return size;
        }
    }
}