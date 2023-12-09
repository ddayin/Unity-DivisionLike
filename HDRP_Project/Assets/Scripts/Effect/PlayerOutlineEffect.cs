using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

namespace DivisionLike {
  /// <summary>
  /// 플레이어에 아웃라인을 그리는 이펙트
  /// </summary>
  public class PlayerOutlineEffect : MonoBehaviour {
    public Outline[] m_Outlines = new Outline[5];

    private void Awake() {
      SetEnable(false);
    }

    public void SetEnable(bool enable) {
      for (int i = 0; i < 5; i++) {
        m_Outlines[i].enabled = enable;
      }
    }

    public void Enable(float time) {
      SetEnable(true);

      Invoke("Disable", time);
    }

    public void Disable()
    {
      SetEnable(false);
    }
  }
}