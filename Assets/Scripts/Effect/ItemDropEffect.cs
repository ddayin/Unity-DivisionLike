using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DivisionLike
{
    public class ItemDropEffect : MonoBehaviour
    {
        private LineRenderer lineUp;

        private void Awake()
        {
            lineUp = transform.GetComponent<LineRenderer>();

            lineUp.SetPosition( 0, transform.position );

            Vector3 onePosition = transform.position;
            onePosition.y = 100f;
            lineUp.SetPosition( 1, onePosition );
        }
    }
}