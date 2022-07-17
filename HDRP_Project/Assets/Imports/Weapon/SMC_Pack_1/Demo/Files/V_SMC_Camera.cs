using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V_SMC_Camera : MonoBehaviour {

	public V_SMC_Handler crosshairHandler;

	float rotationX = 0F;
	float rotationY = 0F;

	public float detectedSize = 2f;

	Vector3 fireDirection;
	Vector3 firePoint;

	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
		rotationX += Input.GetAxis("Mouse X") * 2;
		rotationY -= Input.GetAxis("Mouse Y") * 2;

		Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0);
		transform.rotation = rotation;

		Hit ();

	}

	void Hit(){
		// Raycasting:
		RaycastHit hit;
		int range = 1000;
		fireDirection = transform.TransformDirection(Vector3.forward) * 10;
		firePoint = transform.position;
		// Debug the ray out in the editor:
		Debug.DrawRay(firePoint, fireDirection, Color.green);

		if (Physics.Raycast (firePoint, (fireDirection), out hit, range)) {
			// Scale if crosshair is on something:


			if (hit.transform.name == "Friend") {
				crosshairHandler.ChangeColor (Color.green);
			} else if (hit.transform.name == "Enemy") {
				crosshairHandler.ChangeColor (Color.red);
			} else {
				crosshairHandler.ChangeColor (Color.white);
			}
		} else {
			crosshairHandler.ChangeColor (Color.white);
		}
	}
}
