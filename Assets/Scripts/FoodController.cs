using UnityEngine;
using System.Collections;

public class FoodController : MonoBehaviour {
	
	public Vector2 initialV;
	public Transform SplatterPrefab;
	public bool FacesMovement;
	public bool CanBounce;

	private bool unfrozen;
	private GameController gc;

	// Use this for initialization
	void Start () {

		GameObject go = GameObject.Find ("GameController");
		gc = go.GetComponent<GameController>();
		gc.NumProjectiles++;
		iTween.Stab (go,gc.throw1Sound,0);
		iTween.Stab (go,gc.throw2Sound,0);
		iTween.Stab (go,gc.throw3Sound,0);
		rigidbody2D.AddForce((initialV * rigidbody2D.mass)/Time.fixedDeltaTime);
//		rigidbody2D.isKinematic = true;  //prevents projectiles from falling before sim runs
	}
	// Update is called once per frame
	void FixedUpdate () {

		if(FacesMovement)
		{
			//updates angle of projectiles
			float aimangle = Mathf.RoundToInt(Mathf.Atan2(rigidbody2D.velocity.x,
			                                              rigidbody2D.velocity.y) * -180/Mathf.PI);		
			transform.eulerAngles = new Vector3(0,0,aimangle + 90);	
		}
	
	}

	public void OnCollisionEnter2D(Collision2D col)
	{
		JumpInputController jic = col.gameObject.GetComponent<JumpInputController>();
		if(jic != null || (!CanBounce && col.gameObject.tag == "Ground"))
		{
			GameObject go = GameObject.Find ("GameController");
			Transform splat = (Transform) GameObject.Instantiate(SplatterPrefab);
			gc.CurrentLevel.Transforms.Add(splat);
			splat.position = transform.position;
			if(jic != null)  // hit player
			{
				gc.NumFails++;
				splat.eulerAngles = transform.eulerAngles;
				splat.parent = col.transform;
				jic.HitByPie();			
			}
			else  // hit ground
			{
				splat.eulerAngles = new Vector3(0,0,-90);
				if(gc.NumProjectiles % 2 != 0)
				{
				iTween.Stab (col.gameObject,gc.splat1Sound,0);
				}
				else
				{
				iTween.Stab (col.gameObject,gc.splat2Sound,0);
				}
			}
			gc.NumProjectiles--;
				
			gc.CurrentLevel.Transforms.Remove(transform);
			GameObject.Destroy(gameObject);
		}
		else if(CanBounce && col.gameObject.tag == "Ground")
		{
			CanBounce = false;
		}
		
	}
}
