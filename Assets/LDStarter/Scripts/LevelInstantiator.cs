using UnityEngine;
using System.Collections;

public class LevelInstantiator  {

	private System.Random rand;

	public LevelInstantiator(System.Random r)
	{
		rand = r;
	}
// line for setting force on obj	rigidbody2D.AddRelativeForce ((initialV * rigidbody2D.mass) / Time.fixedDeltaTime);
	public void InstantiateLevel(LevelData level)
	{
		Transform food;
		FoodController fc;
		/*
		// LOW PIE:  This is to prevent the player from just pitching low-left every time
		food = (Transform) GameObject.Instantiate (level.foodData[0]);
		food.position = new Vector3(
			(float)rand.NextDouble()* 3 + -20,
			(float)rand.NextDouble()* 3 + 23,0);
		fc = food.gameObject.GetComponent<FoodController>();		
		fc.initialV = new Vector2(Random.Range(3,5),
		                          Random.Range(-2,0));
		
		
		// HIGH PIE:  This is to prevent the player from just standing still
		food = (Transform) GameObject.Instantiate (level.foodData[0]);
		food.position = new Vector3(
			(float)rand.NextDouble()* 3 + -20,
			(float)rand.NextDouble()* 3 + 23,0);
		fc = food.gameObject.GetComponent<FoodController>();		
		fc.initialV = new Vector2(Random.Range(10,12),
		                          Random.Range(5,7));
		
		*/
	
		// COMMENT THIS SHIT - it's just to test the semi-fixed pies above w/out the randoms below.
	
		//variables for origin position of projectiles
		float offsetX, offsetY, xdistribVar, ydistribVar;
		offsetX = -25;
		offsetY = 13;
		xdistribVar = 10;
		ydistribVar = 10;		
		
		//variables for input force of projectiles
		float xvelMin, xvelMax, yvelMin, yvelMax;
		xvelMin = 10;
		xvelMax = 13;
		yvelMin = -5;
		yvelMax = 15;

		//creates the list of food items with positions and angles		
		foreach (Transform t in level.foodData) {
			food = (Transform) GameObject.Instantiate (t);
			fc = food.gameObject.GetComponent<FoodController>();
			
			float heightFactor = 1;
			if(fc.CanBounce)
				heightFactor = 0.6f;		
			
			food.position = new Vector3(
				(float)rand.NextDouble()* xdistribVar + offsetX,
				((float)rand.NextDouble()* ydistribVar + offsetY) * heightFactor,0);

			
			fc.initialV = new Vector2(Random.Range(xvelMin,xvelMax),
			                          Random.Range(yvelMin,yvelMax*heightFactor) );

			float aimangle = Mathf.RoundToInt(Mathf.Atan2(fc.initialV.x,
			                                              fc.initialV.y) * -180/Mathf.PI);
			Debug.Log ("aim angle = " + aimangle);
			food.eulerAngles = new Vector3(0,0,aimangle + 90);	

			level.Transforms.Add(food);
		}
	}

}
