using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator {

	private System.Random rand;
	private List<Transform> foodItemPrefabs;

	public LevelGenerator(System.Random r,List<Transform> f)
	{
		rand = r;
		foodItemPrefabs = f;
	}

	public LevelData GenerateLevel(int level)
	{
		LevelData data = new LevelData(level);
		data.StartPosition = new Vector2(1f,1f);
		//generates list of incoming projectiles based off of level difficulty
		int difficulty = (level + 3) / 2;
		for (int i = 0; i < difficulty; i++) 
		{
			data.foodData.Add(foodItemPrefabs[0]);
		}

		return data;
	}

}
