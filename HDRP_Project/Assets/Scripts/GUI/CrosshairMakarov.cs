using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    /// <summary>
    /// Makarov 총의 crosshair
    /// </summary>
    public class CrosshairMakarov : CrosshairHandler
    {
        private Image _image;

        private void Awake()
        {
            _image = transform.GetComponent<Image>();
        }

        public override void ChangeColor(Color _color)
        {
            //Debug.Log( "CrosshairMakarov.ChangeColor() overrided " + _color );

            // FIXME: this is a tricky way....:(
            if (_image == null)
            {
                _image = transform.GetComponent<Image>();
            }

            _image.color = _color;
        }
    }
}