using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class V_SMC_Handler : MonoBehaviour {

	public Sprite[] crossHairs;
	[HideInInspector]
	public int curCrossHair = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		// Roll through the cursors list:
		if (Input.GetKeyDown (KeyCode.Mouse1)) {
			if (curCrossHair < crossHairs.Length - 1) {
				curCrossHair += 1;
			} else {
				curCrossHair = 0;
			}
		}

		// Set the cursor to the current selected:
		GetComponent<Image> ().sprite = crossHairs [curCrossHair];
	}

	public void ChangeColor (Color color){
		this.GetComponent<Image> ().color = color;
	}
}
