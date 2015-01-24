using UnityEngine;
using System.Collections;

public class LevelInstantiator  {

	private System.Random rand;
	
	public LevelInstantiator(System.Random r)
	{
		rand = r;
	}

	public void InstantiateLevel(LevelData level)
	{
		//variables for origin position of projectiles
		float offsetX, offsetY, distribVar;
		offsetX = -15;
		offsetY = 8;
		distribVar = 7;


		//creates the list of food items with positions
		Transform food;
		foreach (Transform t in level.foodData) {
			food = (Transform) GameObject.Instantiate (t);
			food.position = new Vector3(
				(float)rand.NextDouble()* distribVar + offsetX,
			    (float)rand.NextDouble()* distribVar + offsetY,0);
			level.Transforms.Add(food);
		}
	}

}
