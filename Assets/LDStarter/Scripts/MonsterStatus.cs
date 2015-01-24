using UnityEngine;
using System.Collections;

public class MonsterStatus : MonoBehaviour {

	private bool dead = false;
	private bool stunned = false;
	private float stunUntil;
	
	private GameController gc;
	//private MonsterDeathScene deathScene;
	
	// Use this for initialization
	void Start () {
		
		GameObject cgo = GameObject.Find ("GameController");
		gc = cgo.GetComponent<GameController>();
		
		//deathScene = gameObject.GetComponent<MonsterDeathScene>();
		
	}
	
	public bool IsDead()
	{
		return dead;
	}
	
	public bool IsStunned()
	{
		return Time.time < stunUntil;
	}
	
	public void Die()
	{
		dead = true;
		//deathScene.MarkForDeath();

		// TODO delete game object
	}
	
	public void Stun(float t)
	{
		stunUntil = Time.time + t;		
	}

}
