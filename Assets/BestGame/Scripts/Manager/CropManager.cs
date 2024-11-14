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
		currentGrid = FindAnyObjectByType<Grid>();
		cropParent = GameObject.Find("CropParent").transform;

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

	private void OnAfterSceneLoadEvent()
	{
		currentGrid = FindAnyObjectByType<Grid>();
		cropParent = GameObject.Find("CropParent").transform;
	}

	private void OnPlantSeedEvent(int ID, TileDetails tileDetails)
	{
		CropDetails currentCrop = GetCropDetails(ID);
		//Debug.Log($"{currentCrop}{ID}");
	
		if (currentCrop!=null&&SeasonCanCrop(currentCrop)&&tileDetails.seedItemID==-1)
		{
			tileDetails.seedItemID = ID;
			tileDetails.growthDays = 0;
			//显示农作物
			InstantiateCrop(tileDetails, currentCrop);
		}
		else if (tileDetails.seedItemID!=-1)//用于刷新地图
		{
			//显示农作物
			InstantiateCrop(tileDetails, currentCrop);
		}
	}

	public CropDetails GetCropDetails(int cropId)
	{
		for (int i = 0; i < CropDetails_SO.cropDetailsList.Count; i++)
		{
			if (CropDetails_SO.cropDetailsList[i].seedItemID==cropId)
			{
				return CropDetails_SO.cropDetailsList[i];
			}
		
		}
		return null;
	}

	private bool SeasonCanCrop(CropDetails cropDetails)
	{
		if (TimeManager.Instance.GameSeason== cropDetails.seasons)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	private void InstantiateCrop(TileDetails tileDetails, CropDetails cropDetails)
	{
		int growStages = cropDetails.growthDays.Length;
		int currentStage = 0;
		int dayCount = cropDetails.TotalGrowthDays;
		for (int i = growStages - 1; i >= 0; i--)
		{
			if (tileDetails.growthDays >= dayCount)
			{
				currentStage = i;
				break;
			}
			dayCount -= cropDetails.growthDays[i];
		}
		Debug.Log(currentStage);
		GameObject cropPrefab=cropDetails.growthPrefabs[currentStage];
		Sprite cropSprite=cropDetails.growthSprites[currentStage];
		Vector3 position = new Vector3(tileDetails.gridX + 0.5f, tileDetails.gridY + 0.5f, 0);
		GameObject cropInstantiate=Instantiate(cropPrefab, position,Quaternion.identity,cropParent);
		cropInstantiate.GetComponentInChildren<SpriteRenderer>().sprite=cropSprite;
		Debug.Log("instantiate");
	}
}
