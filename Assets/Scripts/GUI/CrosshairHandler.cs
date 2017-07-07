using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    public class CrosshairHandler : MonoBehaviour
    {
        private Image image;

        // Use this for initialization
        void Awake()
        {
            image = this.GetComponent<Image>();

            gameObject.SetActive( false );

        }

        public void ChangeColor( Color color )
        {
            image.color = color;
        }


    }
}