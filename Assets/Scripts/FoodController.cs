using UnityEngine;
using System.Collections;

public class FoodController : MonoBehaviour {

	public Vector2 initialV;

	// Use this for initialization
	void Start () {
		rigidbody2D.AddRelativeForce ((initialV * rigidbody2D.mass)/Time.fixedDeltaTime);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
