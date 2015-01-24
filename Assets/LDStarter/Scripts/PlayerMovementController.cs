using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (PlayerStatus))]
public class PlayerMovementController : MonoBehaviour {

	//public float WalkSpeed=5f;
	private PlayerStatus pstat;
	private Transform gun;

	public Vector2 Moving { get; set; }
	public Vector2 AimPoint { get; set; }

	void Start () {
		pstat = gameObject.GetComponent<PlayerStatus>();
		gun = transform.Find("Gun");
	}
	
	void FixedUpdate () {
		Walk();
		FaceGunToAim();
	}

	private void Walk()
	{
		Vector2 targetspeed = pstat.WalkingSpeed * Moving.normalized;
		Vector2 deltaspeed = targetspeed - rigidbody2D.velocity;
		Vector2 force = rigidbody2D.mass * deltaspeed / Time.deltaTime;
		rigidbody2D.AddForce(force);
	}

	private void FaceGunToAim()
	{
		if(gun == null)
			return;

		float dx = AimPoint.x - gun.position.x;
		float dy = AimPoint.y - gun.position.y;
		float aimangle = Mathf.RoundToInt(Mathf.Atan2(dx, dy) * -180/Mathf.PI);
		
		gun.eulerAngles = new Vector3(0,0,aimangle);
	}
}
