using Unity.Collections;
using UnityEngine;

public class CropManager : SingletonMonoBehaviour<CropManager>
{
	public CropDetails_SO CropDetails_SO;

	[SerializeField, ReadOnly]
	private Transform cropParent;

	[SerializeField, ReadOnly]
	private Grid currentGrid;

	private void Start()
	{
	}

	private void OnEnable()
	{
		EventHandler.PlantSeedEvent += OnPlantSeedEvent;
		EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
	}

	private void OnDisable()
	{
		EventHandler.PlantSeedEvent -= OnPlantSeedEvent;
		EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
	}

	// Handle scene load event to initialize grid and parent objects
	private void OnAfterSceneLoadEvent()
	{
		currentGrid = FindAnyObjectByType<Grid>();
		cropParent = GameObject.Find("CropParent").transform;
	}

	// Handle seed planting event
	private void OnPlantSeedEvent(int ID, TileDetails tileDetails)
	{
		CropDetails currentCrop = GetCropDetails(ID);

		if (currentCrop != null && SeasonCanCrop(currentCrop) && tileDetails.seedItemID == -1)
		{
			tileDetails.seedItemID = ID;
			tileDetails.growthDays = 0;
			InstantiateCrop(tileDetails, currentCrop); // Display the crop
		}
		else if (tileDetails.seedItemID != -1) // Refresh map with existing crops
		{
			InstantiateCrop(tileDetails, currentCrop); // Display the crop
		}
	}

	// Get crop details by crop ID
	public CropDetails GetCropDetails(int cropId)
	{
		for (int i = 0; i < CropDetails_SO.cropDetailsList.Count; i++)
		{
			if (CropDetails_SO.cropDetailsList[i].seedItemID == cropId)
			{
				return CropDetails_SO.cropDetailsList[i];
			}
		}
		return null;
	}

	// Check if the current season matches the crop's suitable season
	private bool SeasonCanCrop(CropDetails cropDetails)
	{
		return TimeManager.Instance.GameSeason == cropDetails.seasons;
	}

	// Instantiate a crop based on its growth stage and tile details
	private void InstantiateCrop(TileDetails tileDetails, CropDetails cropDetails)
	{
		int growStages = cropDetails.growthDays.Length;
		int currentStage = 0;
		int dayCount = cropDetails.TotalGrowthDays;

		// Determine the current growth stage based on the number of growth days
		for (int i = growStages - 1; i >= 0; i--)
		{
			if (tileDetails.growthDays >= dayCount)
			{
				currentStage = i;
				break;
			}
			dayCount -= cropDetails.growthDays[i];
		}

		// Instantiate the crop at the appropriate growth stage
		GameObject cropPrefab = cropDetails.growthPrefabs[currentStage];
		Sprite cropSprite = cropDetails.growthSprites[currentStage];
		Vector3 position = new Vector3(tileDetails.gridX + 0.5f, tileDetails.gridY + 0.5f, 0);
		GameObject cropInstantiate = Instantiate(cropPrefab, position, Quaternion.identity, cropParent);
		cropInstantiate.GetComponentInChildren<SpriteRenderer>().sprite = cropSprite;
		cropInstantiate.GetComponent<Crop>().cropDetails = cropDetails;
		cropInstantiate.GetComponent<Crop>().tileDetails = tileDetails;
	}
}
