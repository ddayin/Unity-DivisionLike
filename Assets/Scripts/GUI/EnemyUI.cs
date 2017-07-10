using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    public class EnemyUI : MonoBehaviour
    {
        private Slider healthSlider;

        private void Awake()
        {
            healthSlider = transform.Find( "HealthUI/HealthSlider" ).GetComponent<Slider>();
        }
        
        public void SetHealthSlider( int health )
        {
            healthSlider.value = health;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

