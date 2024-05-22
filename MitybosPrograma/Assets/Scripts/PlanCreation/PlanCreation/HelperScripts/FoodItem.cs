using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FoodItem
{
	public string name;
	public GameObject foodPrefab;

	public FoodItem(string name, GameObject foodPrefab)
	{
		this.name = name;
		this.foodPrefab = foodPrefab;
	}
}