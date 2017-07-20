
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    public class AmmoBoxIcon : MonoBehaviour
    {
        private AmmoBox _ammoBox;
        private Image _iconImage;

        private void Awake()
        {
            _ammoBox = transform.GetComponent<AmmoBox>();
            _iconImage = transform.Find( "Canvas/IconImage" ).GetComponent<Image>();
        }


        // Update is called once per frame
        void Update()
        {
            CheckIfEmpty();
        }

        private void CheckIfEmpty()
        {
            if ( _ammoBox._state == AmmoBox.AmmoBoxState.Emtpy )
            {
                _iconImage.gameObject.SetActive( false );
            }
        }
    }
}