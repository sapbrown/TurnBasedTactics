using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystemController : MonoBehaviour {
	[SerializeField]private float[] yDegrees;
	[SerializeField]private int startingIndex;
	private int currentIndex;
	private Quaternion[] rotationQuaternions;

	// Use this for initialization
	void Start () {
		rotationQuaternions = new Quaternion[yDegrees.Length];
		for (int i = 0; i < rotationQuaternions.Length; i++) {
			rotationQuaternions [i] = Quaternion.Euler (0f, yDegrees [i], 0f);
		}
		currentIndex = startingIndex;
		transform.rotation = rotationQuaternions [currentIndex];
	}
	
	// Update is called once per frame
	void Update(){
		if (!cameraLerping) {
			MoveCamera ();
			TurnCamera ();
		} else {
			LerpCamera ();
		}
	}

	void FixedUpdate(){
		if (!cameraLerping) {
			MoveCamera ();
		}
	}

	public bool cameraLerping = false;
	private Quaternion startQ;
	private Quaternion endQ;
	private float currentLerpTime;
	void TurnCamera(){
		if (Input.GetKeyDown (KeyCode.Q) || Input.GetAxis("Triggers") < -.2f) {
			currentIndex++;
			if (currentIndex >= rotationQuaternions.Length) {
				currentIndex = 0;
			}
		} else if (Input.GetKeyDown (KeyCode.E) || Input.GetAxis("Triggers") > .2f) {
			currentIndex--;
			if (currentIndex < 0) {
				currentIndex = rotationQuaternions.Length - 1;
			}
		}

		if (transform.rotation.eulerAngles != rotationQuaternions[currentIndex].eulerAngles){
			startQ = transform.rotation;
			endQ = rotationQuaternions [currentIndex];
			isRotating = true;
			cameraLerping = true;
			currentLerpTime = 0f;
		}
	}

	public bool isRotating;
	[SerializeField]private float rotationLerpTime = 1f;
	[SerializeField]private float movementLerpTime = 1f;
	[SerializeField]private Transform playerCursor;
	void LerpCamera(){
		float lerpTime;

		currentLerpTime += Time.deltaTime;

		if (isRotating) {
			lerpTime = rotationLerpTime;
		} else {
			lerpTime = movementLerpTime;
		}
			
		if (currentLerpTime >= lerpTime) {
			currentLerpTime = lerpTime;
			isRotating = false;
			cameraLerping = false;
		}

		float t = currentLerpTime / lerpTime;
		float perc = t * t * t * (t * (6f * t - 15f) + 10f);
		if (isRotating) {
			transform.rotation = Quaternion.Lerp (startQ, endQ, perc);
		} else if (!isRotating && !isMoving) {
			transform.position = Vector3.Lerp (startV, playerCursor.position - 
				new Vector3(0, 8f, 0), perc);
		}
	}

	[SerializeField]private float cameraSpeed = 1f;
	private bool isMoving;
	private Vector3 startV;
	void MoveCamera(){
		float sideMove = Input.GetAxis ("RightJoystick_Horizontal") * cameraSpeed;
		float forwardMove = Input.GetAxis ("RightJoystick_Vertical") * cameraSpeed;

		sideMove *= Time.deltaTime;
		forwardMove *= Time.deltaTime;

		if (forwardMove + sideMove != 0) {
			isMoving = true;
			transform.Translate (sideMove, 0, forwardMove);
			startV = transform.position;
		} else if (isMoving){
			if (startV == transform.position) {
				isMoving = false;
				currentLerpTime = 0f;
				cameraLerping = true;
			}
		}
	}
}
