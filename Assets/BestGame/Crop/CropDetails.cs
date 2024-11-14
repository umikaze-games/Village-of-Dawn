using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class CropDetails
{
	public int seedItemID;

	[Header("Days required for each growth stage")]
	public int[] growthDays;
	public int TotalGrowthDays 
	{
		get
		{
			int totalGrowthDays = 0;
			foreach (int growthDay in growthDays)
			{
			
				totalGrowthDays += growthDay;
			}
		
			return totalGrowthDays;
		}
	}

	[Header("Prefab for each growth stage")]
	public GameObject[] growthPrefabs;

	[Header("Sprites for each growth stage")]
	public Sprite[] growthSprites;

	[Header("Seasons when the crop can be planted")]
	public int seasons;

	[Header("Harvest Tools")]
	public int harvestToolItemID;

	[Header("Number of uses for each tool")]
	public int[] requireActionCount;

	[Header("Item to be transformed")]
	public int transferItemID;

	[Header("Harvest Information")]
	public int[] producedItemID;
	public int[] producedMinAmount;
	public int[] producedMaxAmount;
	public Vector2 spawnRadius;

	[Header("Options")]
	public bool generateAtPlayerPosition;
	public bool hasAnimation;
	public bool hasParticleEffect;


}
