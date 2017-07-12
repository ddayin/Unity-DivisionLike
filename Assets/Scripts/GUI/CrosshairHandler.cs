

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DivisionLike
{
    public abstract class CrosshairHandler : MonoBehaviour
    {
        virtual public void ChangeColor( Color color )
        {
            Debug.Log( "CrosshairHandler.ChangeColor() virtual" );
        }
    }
}