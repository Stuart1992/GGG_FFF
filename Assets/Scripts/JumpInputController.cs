using UnityEngine;
using System.Collections;

public class JumpInputController : MonoBehaviour {

	public Transform ArrowPrefab;
	public float MaxArrowSize;
	public float MaxJumpSpeed;

	private Transform arrow;
	private Vector2 aimPoint;
	private bool isTrackingMouse;
	private bool hasJumped;
	
	private GameController gc;
	private Vector2 startPos;
	
	void Start () {
		GameObject go = GameObject.Find ("GameController");
		gc = go.GetComponent<GameController>();
		
		arrow = (Transform) GameObject.Instantiate(ArrowPrefab);
		startPos = transform.position;
		ResetForNewLevel();
	}
	
	public void ResetForNewLevel()
	{
		transform.position = startPos;
		isTrackingMouse = false;
		hasJumped = false;
		arrow.position = new Vector3(999,999,0);
		aimPoint = transform.position;		
	}
	
	void OnMouseDown()
	{
		isTrackingMouse = true;
	}
	
	void Update () {
	
		if(!gc.IsTimeFrozen)  // game on!  no more user input, and hide the arrow.  jump will commence in FixedUpdate since it's physics
		{
			isTrackingMouse = false;
			arrow.position = new Vector3(999,999,0);
		}
	
		if(isTrackingMouse)
		{
			if(Input.GetMouseButtonUp(0))
			{
				isTrackingMouse = false;
			}
			else
			{
				aimPoint =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Debug.Log ("  aimPoint set to " + aimPoint);
				PointArrowToAimPoint();
			}			
		}		
	}
	
	void FixedUpdate()
	{
		if(!gc.IsTimeFrozen && !hasJumped)
		{
			DoJump();
		}		
	}
	
	private void DoJump()
	{
		Debug.Log ("Girl DoJump start!!");
		hasJumped = true;
		Debug.Log ("               aimPoint = " + aimPoint + " (Vector2)transform.position = " + (Vector2)transform.position);
		
		
		Vector2 jumpVec = (aimPoint - (Vector2)transform.position);
		float aimSize = jumpVec.magnitude;		
		float jumpFactor = aimSize/MaxArrowSize;
		Debug.Log ("               jumpVec = " + jumpVec + " aimSize = " + aimSize + "  jumpFactor = " + jumpFactor);
		
		Vector2 targetVel = jumpFactor * MaxJumpSpeed * jumpVec.normalized;		
		Vector2 deltaV = targetVel - rigidbody2D.velocity;
		Vector2 force = rigidbody2D.mass * deltaV / Time.fixedDeltaTime;
		Debug.Log ("               targetVel = " + targetVel + " deltaV = " + deltaV + "  force = " + force);
		
		Debug.Log ("Girl jumping: force is "+ force);
		rigidbody2D.AddForce(force);
	}
	
	private void PointArrowToAimPoint()
	{
		Vector2 s = transform.position;
		Vector2 e = aimPoint;
		Vector2 diff = (s-e);
		arrow.position = (s+e)/2; // midpoint between start and end		
		arrow.localScale =  new Vector3(arrow.localScale.x, -diff.magnitude/10, arrow.localScale.z);
		
		float aimangle = Mathf.RoundToInt(Mathf.Atan2(diff.x, diff.y) * -180/Mathf.PI);		
		arrow.eulerAngles = new Vector3(0,0,aimangle);		
	}
}
