using UnityEngine;
using System.Collections;

public class JumpInputController : MonoBehaviour {

	public Transform ArrowPrefab;

	private Transform arrow;
	private Vector2 aimPoint;
	private bool isTrackingMouse=false;
	
	
	//docs say this should work but no dice - assume it is a problem with 2D vs 3D box colliders??
	void OnMouseDown()
	{
		isTrackingMouse = true;
		Debug.Log ("YOU CLICKED ON THE GIRL!");
	}
	
	void Start () {
		arrow = (Transform) GameObject.Instantiate(ArrowPrefab);
		arrow.position = new Vector3(999,999,0);
	}	
	
	void Update () {
		if(isTrackingMouse)
		{
			if(Input.GetMouseButtonUp(0))
			{
				isTrackingMouse = false;
			}
			else
			{
				aimPoint =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
				PointArrowToAimPoint();
			}
				
		}			

		
	}
	
	private void PointArrowToAimPoint()
	{
		Vector2 s = transform.position;
		Vector2 e = aimPoint;
		Vector2 diff = (s-e);
		arrow.position = (s+e)/2; // midpoint between start and end		
		arrow.localScale =  new Vector3(arrow.localScale.x, -diff.magnitude/10, arrow.localScale.z);
		
		float aimangle = Mathf.RoundToInt(Mathf.Atan2(diff.x, diff.y) * -180/Mathf.PI);		
		arrow.eulerAngles = new Vector3(0,0,aimangle);
		
	}
	
}
