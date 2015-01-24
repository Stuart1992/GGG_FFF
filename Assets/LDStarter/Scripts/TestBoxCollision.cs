using UnityEngine;
using System.Collections;

public class TestBoxCollision : MonoBehaviour {

	public bool WinResult;

	private GameController gc;
	
	
	// Use this for initialization
	void Start () {
		GameObject go = GameObject.Find ("GameController");
		gc = go.GetComponent<GameController>();
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player")
		{
			gc.CurrentLevel.Over = true;
			gc.CurrentLevel.Victory = WinResult;
		}
		
	}
}
