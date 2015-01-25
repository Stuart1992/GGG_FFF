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
		int difficulty;// = (level + 3) / 2;
		if(level<=2)
			difficulty = 2;
		else if(level<=4)
			difficulty = 3;
		else if(level<=6)
			difficulty=4;
		else if((level>10) && (level%10==0))
		{
			difficulty = 10 + rand.Next(0,10);
		}
		else if((level>10) && (level%6==0))
		{
			difficulty = 8;
		}		
		else {
			difficulty = Mathf.Min (6, 1 + Mathf.FloorToInt(level/2)) + rand.Next(-1,2);
		}
		
		for (int i = 0; i < difficulty; i++) 
		{
			data.foodData.Add(foodItemPrefabs[Random.Range(0,foodItemPrefabs.Count)]);
		}

		return data;
	}

}
