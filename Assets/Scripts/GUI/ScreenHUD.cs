using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    public class ScreenHUD : MonoBehaviour
    {
        public static ScreenHUD instance = null;

        private Text levelText;
        private Slider expSlider;

        void Awake()
        {
            Debug.Log( "ScreenHUD Awake()" );

            if ( instance == null )
            {
                instance = this;
            }
            else if ( instance != null )
            {
                Destroy( gameObject );
            }

            levelText = transform.Find( "LevelText" ).GetComponent<Text>();
            expSlider = transform.Find( "ExpSlider" ).GetComponent<Slider>();

            SetLevelText();
            CalculateExpSlider( 0 );
        }

        public void SetLevelText()
        {
            levelText.text = Player.instance.stats.level.ToString();
            
        }

        public void CalculateExpSlider( int xpToAdd )
        {
            Player.instance.stats.xp += (ulong) xpToAdd;
            Player.instance.stats.CheckLevel();
            
            float normalizedXP = (float) (Player.instance.stats.xp) / (float) (Player.instance.stats.xpRequire[ Player.instance.stats.level - 1 ]);
            Debug.Log( "player xp = " + Player.instance.stats.xp );
            Debug.Log( "normalized xp = " + normalizedXP );
            expSlider.normalizedValue = normalizedXP;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

