using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairHandler : MonoBehaviour
{
    private Image image;

    // Use this for initialization
    void Start()
    {
        image = this.GetComponent<Image>();

        gameObject.SetActive( false );
        
    }

    public void ChangeColor( Color color )
    {
        image.color = color;
    }


}
