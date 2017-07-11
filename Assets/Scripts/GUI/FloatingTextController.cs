/*
 * reference - https://www.youtube.com/watch?v=fbUOG7f3jq8&t=19s
 */

using UnityEngine;
using System.Collections;

public class FloatingTextController : MonoBehaviour
{
    public FloatingText popupText;
    private GameObject canvas;

    private void Awake()
    {
        canvas = this.gameObject;
    }

    public void CreateFloatingText( string text, Transform location )
    {
        FloatingText instance = Instantiate( popupText );
        
        instance.transform.SetParent( canvas.transform, false );

        Vector3 newPos = new Vector3( location.position.x + Random.Range( -0.3f, 0.3f ), location.position.y + Random.Range( -0.3f, 0.3f ), location.position.z  );
        instance.transform.position = newPos;
        instance.SetText( text );
    }
}
