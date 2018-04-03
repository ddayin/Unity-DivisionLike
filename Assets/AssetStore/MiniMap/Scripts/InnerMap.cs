/*
 * reference - https://github.com/Area730/MiniMap
 */ 

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[AddComponentMenu("MiniMap/Inner map")]
[RequireComponent(typeof(Image))]
public class InnerMap : MonoBehaviour {

    private RectTransform _innerMapRect;

    public RectTransform InnerMapRect
    {
        get
        {
            if (!_innerMapRect)
            {
                _innerMapRect = GetComponent<RectTransform>();
            }

            return _innerMapRect;
        }
    }

    public float getMapRadius()
    {
        Vector3[] arr = new Vector3[4];
        InnerMapRect.GetLocalCorners(arr);

        float mapRadius;

        if (Mathf.Abs(arr[0].x) < Mathf.Abs(arr[0].y))
        {
            mapRadius = Mathf.Abs(arr[0].x);
        }
        else
        {
            mapRadius = Mathf.Abs(arr[0].y);
        }

        return mapRadius;
    }

}
