using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	[SerializeField]private Transform[] cameraPositions;
	[SerializeField]private string startingLocation;
	private int currentPosition;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < cameraPositions.Length; i++) {
			if (startingLocation == cameraPositions [i].name) {
				transform.position = cameraPositions [i].position;
				transform.rotation = cameraPositions [i].rotation;
				currentPosition = i;
			}
		}
	}
	
	void Update(){
		if (!cameraLerping) {
			TurnCamera ();
		} else {
			LerpCamera ();
		}
	}

	private bool cameraLerping = false;
	private Vector3 startV;
	private Vector3 endV;
	private Quaternion startQ;
	private Quaternion endQ;
	[SerializeField]private float lerpTime = 1f;
	private float currentLerpTime;
	void TurnCamera(){
		if (Input.GetKeyDown (KeyCode.Q)) {
			currentPosition++;
			if (currentPosition >= cameraPositions.Length) {
				currentPosition = 0;
			}
		}

		if (Input.GetKeyDown (KeyCode.E)) {
			currentPosition--;
			if (currentPosition < 0) {
				currentPosition = cameraPositions.Length - 1;
			}
		}

		if (transform.position != cameraPositions[currentPosition].position){
			startV = transform.position;
			endV = cameraPositions [currentPosition].position;
			startQ = transform.rotation;
			endQ = cameraPositions [currentPosition].rotation;
			cameraLerping = true;
			currentLerpTime = 0f;
		}
	}

	void LerpCamera(){
		currentLerpTime += Time.deltaTime;

		if (currentLerpTime >= lerpTime) {
			currentLerpTime = lerpTime;
			cameraLerping = false;
		}

		float t = currentLerpTime / lerpTime;
		float perc = t * t * t * (t * (6f * t - 15f) + 10f);

		transform.position = Vector3.Lerp (startV, endV, perc);
		transform.rotation = Quaternion.Lerp (startQ, endQ, perc);
	}
}
