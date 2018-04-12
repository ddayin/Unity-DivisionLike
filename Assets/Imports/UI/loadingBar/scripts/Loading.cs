using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    private RectTransform m_RectComponent;
    private Image m_ImageComp;
    private bool m_IsUp;

    public float m_RotateSpeed = 200f;
    public float m_OpenSpeed = .005f;
    public float m_CloseSpeed = .01f;

    private void Awake()
    {
        m_RectComponent = GetComponent<RectTransform>();
        m_ImageComp = m_RectComponent.GetComponent<Image>();
        m_IsUp = true;
    }

    //private void Update()
    //{
    //    m_RectComponent.Rotate(0f, 0f, m_RotateSpeed * Time.deltaTime);
    //    changeSize();
    //}

    public void RotateZ( float value )
    {
        //m_RectComponent.Rotate( 0f, 0f, m_RotateSpeed * Time.deltaTime );
        m_RectComponent.Rotate( 0f, 0f, value );
    }

    //private void changeSize()
    //{
    //    float currentSize = m_ImageComp.fillAmount;

    //    if (currentSize < .30f && m_IsUp)
    //    {
    //        m_ImageComp.fillAmount += m_OpenSpeed;
    //    }
    //    else if (currentSize >= .30f && m_IsUp)
    //    {
    //        m_IsUp = false;
    //    }
    //    else if (currentSize >= .02f && !m_IsUp)
    //    {
    //        m_ImageComp.fillAmount -= m_CloseSpeed;
    //    }
    //    else if (currentSize < .02f && !m_IsUp)
    //    {
    //        m_IsUp = true;
    //    }
    //}

}