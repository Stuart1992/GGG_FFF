using UnityEngine;
using System.Collections;

public class FoodController : MonoBehaviour {
	
	public Vector2 initialV;

	private bool unfrozen;
	private GameController gc;
	// Use this for initialization
	void Start () {

		GameObject go = GameObject.Find ("GameController");
		gc = go.GetComponent<GameController>();
		rigidbody2D.isKinematic = true;
	}
	// Update is called once per frame
	void FixedUpdate () {
		if (unfrozen == false & !gc.IsTimeFrozen) {
			    rigidbody2D.isKinematic = false;
				rigidbody2D.AddRelativeForce ((initialV * rigidbody2D.mass) / Time.fixedDeltaTime);
			    unfrozen = true;
				}
	}
}
