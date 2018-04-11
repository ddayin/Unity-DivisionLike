using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DivisionLike
{
    public class PlayScene : MonoBehaviour
    {
        private void Awake()
        {
            SceneController.instance.m_CurrentScene = eSceneName.Play;
        }
    }
}