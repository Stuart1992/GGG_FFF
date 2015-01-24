using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour {

	public int InitialMaxHealth;
	public int InitialWalkingSpeed;

	[HideInInspector]
	public int Health;
	[HideInInspector]
	public int MaxHealth;
	
	[HideInInspector]
	public float WalkingSpeed;

	// Use this for initialization
	void Start () {
		ResetForNewGame();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void ResetForNewGame()
	{
		Health = MaxHealth = InitialMaxHealth;
		WalkingSpeed = InitialWalkingSpeed;
	}
	
	public void TakeDamage(int dmg)
	{
		Health -= dmg;
	}
	
	public void ResetForNewLevel()
	{
	}

}
