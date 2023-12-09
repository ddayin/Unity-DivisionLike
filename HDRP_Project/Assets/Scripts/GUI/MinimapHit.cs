using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    /// <summary>
    /// 미니맵에 어느 방향으로 플레이어 캐릭터가 맞았는지 표시
    /// </summary>
    public class MinimapHit : MonoBehaviour
    {
        private Image m_HitImage;

        private void Awake()
        {
            m_HitImage = GetComponent<Image>();

            Color red = Color.red;
            red.a = 0.4f;
            m_HitImage.color = red;

            m_HitImage.gameObject.SetActive(false);
        }

        /// <summary>
        /// 이미지를 회전한다.
        /// </summary>
        /// <param name="direction"></param>
        public void RotateHit(Vector3 direction)
        {
            m_HitImage.gameObject.SetActive(true);

            float angle = Vector3.Angle(Camera.main.transform.forward, direction);

            m_HitImage.transform.rotation = Quaternion.Euler(0, 0, -angle);

            Invoke("DisableImage", 1.5f);
        }

        /// <summary>
        /// 이미지를 비활성화한다.
        /// </summary>
        private void DisableImage()
        {
            m_HitImage.gameObject.SetActive(false);
        }
    }
}