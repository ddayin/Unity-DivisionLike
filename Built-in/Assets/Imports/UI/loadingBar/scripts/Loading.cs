using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    private RectTransform m_RectComponent;
    private Image m_ImageComp;
    
    public float m_RotateSpeed = 200f;
    public float m_OpenSpeed = .005f;
    public float m_CloseSpeed = .01f;

    private void Awake()
    {
        m_RectComponent = GetComponent<RectTransform>();
        m_ImageComp = m_RectComponent.GetComponent<Image>();
    }

    public void RotateDegreeZ( float degree )
    {
        m_RectComponent.Rotate( 0f, 0f, degree );
    }

    public void RotateZ( float value = 1f )
    {
        float degree = Mathf.Lerp( 0f, 360f, value );
        m_RectComponent.Rotate( 0f, 0f, degree );
    }
}