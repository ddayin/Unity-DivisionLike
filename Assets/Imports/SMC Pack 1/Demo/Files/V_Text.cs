using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class V_Text : MonoBehaviour {

	public Text crosshairNameText;
	V_SMC_Handler handler;

	// Use this for initialization
	void Start () {
		handler = GetComponent<V_SMC_Handler> ();
	}
	
	// Update is called once per frame
	void Update () {
		crosshairNameText.text = handler.crossHairs [handler.curCrossHair].name;
	}
}
