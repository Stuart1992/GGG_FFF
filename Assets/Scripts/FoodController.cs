using UnityEngine;
using System.Collections;

public class FoodController : MonoBehaviour {
	
	public Vector2 initialV;
	public Transform SplatterPrefab;

	private bool unfrozen;
	private GameController gc;
	// Use this for initialization
	void Start () {

		GameObject go = GameObject.Find ("GameController");
		gc = go.GetComponent<GameController>();
		rigidbody2D.isKinematic = true;  //prevents projectiles from falling before sim runs
	}
	// Update is called once per frame
	void FixedUpdate () {
		//upon hitting spacebar the simulation begins for the projectiles
		if (unfrozen == false & !gc.IsTimeFrozen) {
			    rigidbody2D.isKinematic = false;
				rigidbody2D.AddRelativeForce ((initialV * rigidbody2D.mass) / Time.fixedDeltaTime);
			    unfrozen = true;
				}
	}
	
	public void OnCollisionEnter2D(Collision2D col)
	{
		JumpInputController jic = col.gameObject.GetComponent<JumpInputController>();
		if(jic != null)
		{
			Transform splat = (Transform) GameObject.Instantiate(SplatterPrefab);
			gc.CurrentLevel.Transforms.Add(splat);
			splat.position = transform.position;
			splat.parent = col.transform;
			jic.HitByPie();
		}
		
		gc.CurrentLevel.Transforms.Remove(transform);
		GameObject.Destroy(gameObject);
	}
}
