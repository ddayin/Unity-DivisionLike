using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    public class EnemyUI : MonoBehaviour
    {
        private Slider healthSlider;
        private Canvas canvas;
        private FloatingTextController floatingTextController;

        private void Awake()
        {
            healthSlider = transform.Find( "HealthUI/HealthSlider" ).GetComponent<Slider>();
            canvas = transform.GetComponent<Canvas>();
            floatingTextController = transform.GetComponent<FloatingTextController>();
        }

        private void OnEnable()
        {
            healthSlider.normalizedValue = 1f;
        }

        public void SetHealthSlider( int health )
        {
            healthSlider.value = health;
        }

        public void CreateDamageText( string amount )
        {
            floatingTextController.CreateFloatingText( amount, canvas.transform );
        }
        
    }
}

