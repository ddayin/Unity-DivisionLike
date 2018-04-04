/*
 * reference - https://github.com/Area730/MiniMap
 */

using UnityEngine;
using System.Collections;

[AddComponentMenu("MiniMap/Marker Group")]
[RequireComponent(typeof(RectTransform))]
public class MarkerGroup : MonoBehaviour {

    public RectTransform MarkerGroupRect
    {
        get
        {
            if (!_rectTransform)
            {
                _rectTransform = GetComponent<RectTransform>();
            }

            return _rectTransform;
        }
    }

    private RectTransform _rectTransform;

}
