using UnityEngine;
using System.Collections;

public class LevelGenerator {

	private System.Random rand;

	public LevelGenerator(System.Random r)
	{
		rand = r;
	}

	public LevelData GenerateLevel(int level)
	{
		LevelData data = new LevelData(level);
		data.StartPosition = new Vector2(1f,1f);

		// ADD YOUR LEVEL GENERATION CODE HERE
		// you should generate conceptual lists here
		// and then instantiate them as transforms in LevelInstantiator

		return data;
	}

}
