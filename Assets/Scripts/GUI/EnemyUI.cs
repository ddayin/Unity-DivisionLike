using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    public class EnemyUI : MonoBehaviour
    {
        private Slider _healthSlider;
        private Canvas _canvas;
        private FloatingTextController _floatingTextController;

        private void Awake()
        {
            _healthSlider = transform.Find( "HealthUI/HealthSlider" ).GetComponent<Slider>();
            _canvas = transform.GetComponent<Canvas>();
            _floatingTextController = transform.GetComponent<FloatingTextController>();
        }

        private void OnEnable()
        {
            _healthSlider.normalizedValue = 1f;
        }

        public void SetHealthSlider( int health )
        {
            _healthSlider.value = health;
        }

        public void CreateDamageText( string amount )
        {
            _floatingTextController.CreateFloatingText( amount, _canvas.transform );
        }
        
    }
}

