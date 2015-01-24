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
		//variables for origin position of projectiles
		float offsetX, offsetY, distribVar;
		offsetX = -15;
		offsetY = 8;
		distribVar = 7;

		//variables for input force of projectiles
		float xvelMin, xvelMax, yvelMin, yvelMax;
		xvelMin = 7;
		xvelMax = 13;
		yvelMin = 7;
		yvelMax = 13;

		//creates the list of food items with positions
		Transform food;
		foreach (Transform t in level.foodData) {
			food = (Transform) GameObject.Instantiate (t);
			food.position = new Vector3(
				(float)rand.NextDouble()* distribVar + offsetX,
			    (float)rand.NextDouble()* distribVar + offsetY,0);

			FoodController fc = food.gameObject.GetComponent<FoodController>();
			
			fc.initialV = new Vector2(Random.Range(xvelMin,xvelMax),
			                          Random.Range(yvelMin,yvelMax));


			level.Transforms.Add(food);
		}
	}

}
