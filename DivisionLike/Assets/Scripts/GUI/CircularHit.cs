using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    /// <summary>
    /// 플레이어를 기준으로 상대에게 맞은 방향으로 이미지 회전해서 표시
    /// </summary>
    public class CircularHit : MonoBehaviour
    {
        private Image m_HitImage;

        private void Awake()
        {
            m_HitImage = transform.Find("HitImage").GetComponent<Image>();
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

            Invoke("DisableImage", 1f);
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