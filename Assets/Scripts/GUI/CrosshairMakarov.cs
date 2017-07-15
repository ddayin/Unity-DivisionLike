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
            image = GetComponent<Image>();
        }

        public override void ChangeColor( Color _color )
        {
            //Debug.Log( "CrosshairMakarov.ChangeColor() overrided" );

            // FIXME: this is a tricky way....:(
            if ( image == null )
            {
                image = GetComponent<Image>();
            }
            image.CrossFadeColor( _color, 0.1f, false, false );
        }
    }
}