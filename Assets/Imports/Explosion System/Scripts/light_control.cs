// script to delete light after explosition
using UnityEngine;
using System.Collections;

public class light_control : MonoBehaviour {
	private float timeout = 0.5f;
	public Light Light;

	void Start () {
	
	}
	
	void Update () {
		if(timeout>0.1f)
		{
			timeout-=Time.deltaTime;
			Light.range=15;
		}
		else
		{
		Light.range=0;
		}
	
	}
}
