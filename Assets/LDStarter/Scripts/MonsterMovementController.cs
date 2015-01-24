using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (MonsterStatus))]
public class MonsterMovementController : MonoBehaviour {

	public float WalkSpeed;

	//private Collider2D[] cols;
	
	[HideInInspector]
	public Vector2 FacePoint;
	
	[HideInInspector]
	public float ForwardMove;
	
	[HideInInspector]
	public float Sidestep;
	
	private MonsterStatus mstat;
	
	// Use this for initialization
	void Start () {
		//cols = new Collider2D[10];
		mstat = gameObject.GetComponent<MonsterStatus>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {		
		if(!mstat.IsDead())
		{
			RotateForFacing();
			
			if(!mstat.IsStunned())
				Move (WalkSpeed);
		}
	}
	
	private void Move(float speed)
	{
		
		Vector2 faceline = FacePoint - (Vector2)transform.position;  // get the line we look at the mouse along, all move is relative
		
		Vector2 sideline = new Vector2(faceline.y, -faceline.x);// + (Vector2)transform.position;
		
		Vector2 targetfwd = ForwardMove * faceline.normalized;
		Vector2 targetside = Sidestep * sideline.normalized;
		Vector2 targetspeed = speed * (targetfwd + targetside);
		
		//if(WebVulnerable && StandingOnWeb())
		//	targetspeed *= 0.25f;
		
		Vector2 deltaspeed = targetspeed - rigidbody2D.velocity;
		Vector2 force = rigidbody2D.mass * deltaspeed / Time.deltaTime;
		rigidbody2D.AddForce(force);
	}
	
	private void RotateForFacing()
	{
		int stunFlip = (mstat.IsStunned()) ? -1 : 1;
		if (FacePoint.x > transform.position.x) 
			transform.localScale = new Vector3(-1*stunFlip,1,1);
		else
			transform.localScale = new Vector3(1*stunFlip,1,1);
	}
}
