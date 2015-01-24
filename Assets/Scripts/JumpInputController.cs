using UnityEngine;
using System.Collections;

public class JumpInputController : MonoBehaviour {

	//docs say this should work but no dice - assume it is a problem with 2D vs 3D box colliders??
	//void OnMouseDown()
	//{
	//	Debug.Log ("YOU CLICKED ON THE GIRL!");
	//}
	
	private bool isTrackingMouse=false;

	void Start () {
		
	}	
	
	void Update () {
				
	/*	if ( Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, hit, 100.0))
			{
				Destroy(GameObject.Find("targetArea"));
			}
		}
	*/	
	}
}
