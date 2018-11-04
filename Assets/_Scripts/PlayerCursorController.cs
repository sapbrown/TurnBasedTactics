using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursorController : MonoBehaviour {
	public LayerMask detectLayers;
	[SerializeField]private Transform cursorParticle;
	[SerializeField]private CameraSystemController cameraRotation;
	[SerializeField]private float cursorSpeed = 1f;
	[SerializeField]private Vector3 cameraOffset;
	// Use this for initialization

	void Start () {
	}

	void Update(){
		TurnCursorWithCamera ();
		MoveCursor ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		CastCursorParticle();
	}
		
	void CastCursorParticle(){
		int layer_mask = detectLayers.value;
		RaycastHit hitInfo;

		if (Physics.Raycast (transform.position, -transform.up, out hitInfo, Mathf.Infinity, layer_mask)) {
			if (!isMoving) {
				cursorParticle.position = hitInfo.collider.transform.position + transform.up * .6f;
			}
		}
	}

	private bool isMoving;
	void MoveCursor(){
		float sideMove = Input.GetAxis ("LeftJoystick_Horizontal") * cursorSpeed;
		float forwardMove = Input.GetAxis ("LeftJoystick_Vertical") * cursorSpeed;

		sideMove *= Time.deltaTime;
		forwardMove *= Time.deltaTime;

		if (sideMove + forwardMove != 0) {
			isMoving = true;
		} else {
			isMoving = false;
		}

		transform.Translate (sideMove, 0, forwardMove);
		cameraRotation.transform.Translate (sideMove, 0, forwardMove);
	}

	void TurnCursorWithCamera(){
		transform.rotation = cameraRotation.transform.rotation;
	}
}
