using UnityEngine;
using System.Collections;

public class MonsterCollision : MonoBehaviour {

	public int DamageDealt=1;
	
	private GameController gc;	
	
	// Use this for initialization
	void Start () {
		GameObject go = GameObject.Find ("GameController");
		gc = go.GetComponent<GameController>();
	}
	
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player")
		{
			PlayerStatus ps = coll.gameObject.GetComponent<PlayerStatus>();
			ps.TakeDamage(DamageDealt);
		}
		
	}

}
