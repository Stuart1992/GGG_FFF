using UnityEngine;
using System.Collections;

public class JelloBounceWiggle : MonoBehaviour {

	private Animator anim;
	
	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<Animator>();		
	}
	
	public void OnCollisionEnter2D(Collision2D col)
	{
		JumpInputController jic = col.gameObject.GetComponent<JumpInputController>();
		if(col.gameObject.tag == "Ground")
		{
			anim.SetTrigger("Jiggle");
		}
	}
}
