/*
 * reference - https://www.assetstore.unity3d.com/kr/#!/content/40756
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace DivisionLike
{
    public class ScoreManager : MonoBehaviour
    {
        public static int _score;        // The player's score.
        
        private Text text;                      // Reference to the Text component.


        void Awake()
        {
            // Set up the reference.
            text = GetComponent<Text>();

            // Reset the score.
            _score = 0;
        }


        void Update()
        {
            // Set the displayed text to be the word "Score" followed by the score value.
            text.text = "Score: " + _score;
        }
    }
}