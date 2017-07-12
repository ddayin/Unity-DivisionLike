using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    public class CrosshairMakarov : CrosshairHandler
    {
        private Image image;

        private void Awake()
        {
            image = transform.GetComponent<Image>();
        }

        override public void ChangeColor( Color color )
        {
            Debug.Log( "CrosshairMakarov.ChangeColor() overrided" );

            // FIXME: this is a tricky way....:(
            if ( image == null )
            {
                image = transform.GetComponent<Image>();
            }
            image.color = color;
        }
    }
}