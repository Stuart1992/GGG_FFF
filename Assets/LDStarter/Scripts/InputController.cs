using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

	private PlayerMovementController moveCtrl;
	private Camera mainCamera;

	void Start () {
		moveCtrl = gameObject.GetComponent<PlayerMovementController>();
		GameObject camGO = GameObject.Find ("Main Camera");
		if(camGO == null)
			Debug.LogError("Input Contoller could not find 'Main Camera' game object.  Required for getting mouse input.  Fix it.");
		mainCamera = camGO.GetComponent<Camera>();
	}

	void Update () {
		Vector2 mousePos = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);

		moveCtrl.AimPoint = mousePos;
		moveCtrl.Moving = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

	}
}
