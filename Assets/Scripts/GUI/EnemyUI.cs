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
        private RectTransform _transform;
        private Transform _parent;

        private void Awake()
        {
            _healthSlider = transform.Find( "HealthUI/HealthSlider" ).GetComponent<Slider>();
            _canvas = transform.GetComponent<Canvas>();
            _floatingTextController = transform.GetComponent<FloatingTextController>();
            _transform = transform.GetComponent<RectTransform>();
            _parent = transform.parent;
        }

        private void OnEnable()
        {
            _healthSlider.normalizedValue = 1f;
        }

        private void Update()
        {
            ScaleIfClosed();
        }

        public void SetHealthSlider( int health )
        {
            _healthSlider.value = health;
        }

        public void CreateDamageText( string amount )
        {
            _floatingTextController.CreateFloatingText( amount, _canvas.transform );
        }

        private void ScaleIfClosed()
        {
            float distance = Vector3.Distance( Player.instance.transform.position, _parent.transform.position );
            Debug.Log( "distance = " + distance );
            if ( distance > 5f )
            {
                _transform.localScale = Vector3.one * 0.01f;
            }
            else
            {
                //_transform.localScale = Vector3.one * 0.01f * distance * 0.001f * Time.deltaTime;
                Vector3 newScale = Vector3.one * 0.01f;
                newScale.x = distance * 0.001f;
                newScale.y = distance * 0.001f;
                _transform.localScale = newScale;
            }
            
            Debug.Log( "scale.x = " + _transform.localScale.x );
        }
        
    }
}

