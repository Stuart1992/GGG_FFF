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
	private bool hitByPie;
	
	private GameController gc;
	private Vector2 startPos;
	private float startMass;
	private float startGravScale;
	private Animator anim;

	private bool outtaHere;
	
	void Start () {
		GameObject go = GameObject.Find ("GameController");
		gc = go.GetComponent<GameController>();		
		anim = gameObject.GetComponent<Animator>();
		
		arrow = (Transform) GameObject.Instantiate(ArrowPrefab);
		startPos = transform.position;
		startMass = rigidbody2D.mass;
		startGravScale = rigidbody2D.gravityScale;
		ResetForNewLevel();
		outtaHere = false;
	}
	
	public void ResetForNewLevel()
	{
		rigidbody2D.gravityScale = startGravScale;
		transform.position = startPos;
		transform.eulerAngles = new Vector3(0,0,0);
		rigidbody2D.velocity = new Vector2(0,0);
		rigidbody2D.mass = startMass;
		isTrackingMouse = false;
		hasJumped = false;
		hitByPie = false;
		arrow.position = new Vector3(999,999,0);
		aimPoint = transform.position;
		anim.SetBool("Jumping", false);
		anim.SetBool("Hit", false);
	}
	
	public void HitByPie()
	{
		hitByPie = true;
		rigidbody2D.gravityScale = 0.1f;
		anim.SetBool("Hit", true);
		StopMotion();
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
				//aimPoint =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
				//Debug.Log ("  aimPoint set to " + aimPoint);
				PointArrowAndSetAimPoint();
			}			
		}		
	}
	
	void FixedUpdate()
	{
		if(!gc.IsTimeFrozen && !hasJumped)
		{
			DoJump();
		}	
		if ((rigidbody2D.position.x < -35 || rigidbody2D.position.x > 35)
		     && !outtaHere) {
			outtaHere = true;
			gc.NumFails++;
				}
	}
	
	private void DoJump()
	{
		Debug.Log ("Girl DoJump start!!");
		hasJumped = true;
		Debug.Log ("               aimPoint = " + aimPoint + " (Vector2)transform.position = " + (Vector2)transform.position);
		
		
		Vector2 jumpVec = (aimPoint - (Vector2)transform.position);
		float aimSize = jumpVec.magnitude;	
		if(aimSize > 0.1f)
			anim.SetBool("Jumping", true);
			
		float jumpFactor = Mathf.Pow(aimSize/MaxArrowSize, 0.5f);
		
		Debug.Log ("               jumpVec = " + jumpVec + " aimSize = " + aimSize + "  jumpFactor = " + jumpFactor);
		
		Vector2 targetVel = jumpFactor * MaxJumpSpeed * jumpVec.normalized;		
		Vector2 deltaV = targetVel - rigidbody2D.velocity;
		Vector2 force = rigidbody2D.mass * deltaV / Time.fixedDeltaTime;
		Debug.Log ("               targetVel = " + targetVel + " deltaV = " + deltaV + "  force = " + force);
		
		Debug.Log ("Girl jumping: force is "+ force);
		rigidbody2D.AddForce(force);
	}
	
	private void PointArrowAndSetAimPoint()
	{
		Vector2 s = transform.position;
		Vector2 e = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 diff = (s-e);
		
		Vector2 clippedEnd = s + (diff.normalized * -Mathf.Min (diff.magnitude, MaxArrowSize));
		aimPoint = clippedEnd;
		Vector2 clippedDiff = (s-clippedEnd);
		
		arrow.position = (s+clippedEnd)/2; // midpoint between start and clippedEnd		
		arrow.localScale =  new Vector3(arrow.localScale.x, -clippedDiff.magnitude/10, arrow.localScale.z);
		
		float aimangle = Mathf.RoundToInt(Mathf.Atan2(clippedDiff.x, clippedDiff.y) * -180/Mathf.PI);		
		arrow.eulerAngles = new Vector3(0,0,aimangle);
	}
	
	private void StopMotion()
	{
		rigidbody2D.mass = 1000;
		Vector2 deltaV = new Vector2(1-rigidbody2D.velocity.x, -rigidbody2D.velocity.y);
		Vector2 force = rigidbody2D.mass * deltaV / Time.fixedDeltaTime;
		rigidbody2D.AddForce(force);
	}
}
