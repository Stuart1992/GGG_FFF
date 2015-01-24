using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelData {

	public int Level { get; set; }

	public bool Over { get; set; }
	public bool Victory { get; set; }

	public Vector2 StartPosition { get; set; }

	public List<Transform> Transforms { get; set; }

	public List<Transform> foodData { get; set; }

	public LevelData(int level)
	{
		Level = level;
		Transforms = new List<Transform>();
		foodData = new List<Transform>();
	}
}
