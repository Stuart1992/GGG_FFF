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
		//creates the list of food items
		Transform food;
		foreach (Transform t in level.foodData) {
			food = (Transform) GameObject.Instantiate (t);
			food.position = new Vector3((float)rand.NextDouble() * 3,(float)rand.NextDouble() * 3,0);
			level.Transforms.Add(food);
		}
	}

}
