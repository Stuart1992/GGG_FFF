using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JumpInputController : MonoBehaviour {

	public Transform ArrowPrefab;
	public float MaxArrowSize;
	public float MaxJumpSpeed;
	public List<AudioClip> HitSoundList;
	public bool PlayJumpSound;
	public float GroundedCastSize;

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
	private System.Random rand;
	private float lastJumpTime;
	
	void Start () {
		rand = new System.Random();
	
		GameObject go = GameObject.Find ("GameController");
		gc = go.GetComponent<GameController>();		
		anim = gameObject.GetComponent<Animator>();
		anim.updateMode = AnimatorUpdateMode.UnscaledTime;
		
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
		if(HitSoundList.Count>0)
		{
			int idx = rand.Next(0,HitSoundList.Count);
			iTween.Stab(gameObject, HitSoundList[idx], 0);
		}
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
		
		if(Time.time - lastJumpTime > 0.1f)	
			CheckForLanding();
			
		if(!hitByPie)
			transform.eulerAngles = new Vector3(0,0,0);
	}
	
	private void CheckForLanding()
	{
		if(!hitByPie && rigidbody2D.velocity.y <=0 && IsGrounded())
		{
			anim.SetBool("Jumping", false);
		}
	}
	
	private bool IsGrounded() {	
		Debug.Log("Checking for IsGrounded");
		LayerMask layerMask = 1 << LayerMask.NameToLayer("Ground");
		RaycastHit2D res = Physics2D.Raycast(transform.position, -Vector2.up, GroundedCastSize, layerMask);
		if(res.collider != null)
			Debug.Log ("   isGrounded True, ray hit " + res.transform.name);
		return (res.collider != null);
	}
	
	
	private void DoJump()
	{
		hasJumped = true;
		lastJumpTime = Time.time;
	
		Vector2 jumpVec = (aimPoint - (Vector2)transform.position);
		float aimSize = jumpVec.magnitude;	
		if(aimSize > 0.5f)
			anim.SetBool("Jumping", true);
			
		float jumpFactor = Mathf.Pow(aimSize/MaxArrowSize, 0.5f);
		
		Vector2 targetVel = jumpFactor * MaxJumpSpeed * jumpVec.normalized;		
		Vector2 deltaV = targetVel - rigidbody2D.velocity;
		Vector2 force = rigidbody2D.mass * deltaV / Time.fixedDeltaTime;

		rigidbody2D.AddForce(force);
		if(PlayJumpSound)
			iTween.Stab (gc.gameObject,gc.jumpSound,0);
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
