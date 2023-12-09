using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace DivisionLike
{
    /// <summary>
    /// 점수 관리자
    /// </summary>
    public class ScoreManager : MonoBehaviour
    {
        public static int m_Score; // The player's score.

        private Text m_Text; // Reference to the Text component.


        void Awake()
        {
            // Set up the reference.
            m_Text = GetComponent<Text>();

            // Reset the score.
            m_Score = 0;
        }


        void Update()
        {
            // Set the displayed text to be the word "Score" followed by the score value.
            m_Text.text = "Score: " + m_Score;
        }
    }
}