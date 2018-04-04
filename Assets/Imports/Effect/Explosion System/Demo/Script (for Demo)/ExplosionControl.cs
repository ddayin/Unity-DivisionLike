using UnityEngine;
using System.Collections;

public class ExplosionControl : MonoBehaviour {
	public GameObject explosion1;
	public GameObject explosion2;
	public GameObject wall2;
	public GameObject wall1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Q)) {
			explosion1.SetActive (true);
		}
		if (Input.GetKey (KeyCode.W)) {
			wall2.SetActive (true);
			wall1.SetActive (false);
		}
		if (Input.GetKey (KeyCode.E)) {
			explosion2.SetActive (true);
		}
	
	}
}
