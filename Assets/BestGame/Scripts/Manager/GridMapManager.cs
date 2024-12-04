using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GridMapManager : SingletonMonoBehaviour<GridMapManager>, ISaveable
{
	public List<MapData_SO> mapDatalist;

	private Grid currentGrid;

	private Dictionary<string, TileDetails> tileDetailsDict = new Dictionary<string, TileDetails>();

	private Dictionary<string, bool> firstLoadDict = new Dictionary<string, bool>();

	public RuleTile digTile;
	public RuleTile waterTile;

	[SerializeField]
	private Tilemap digTilemap;
	private Tilemap waterTilemap;

	private int currentSeason;

	private List<ReapItem> itemInRadius;

	public string GUID => GetComponent<DataGUID>().guid;

	private void Start()
	{
		ISaveable saveable = this;
		saveable.RegisterSaveable();

		currentGrid = FindAnyObjectByType<Grid>();
		foreach (var mapData in mapDatalist)
		{
			firstLoadDict.Add(mapData.sceneName, true);
			InitTileDetailsDict(mapData);
		}
		GameObject digObject = GameObject.FindGameObjectWithTag("Dig");
		if (digObject != null)
		{
			digTilemap = digObject.GetComponent<Tilemap>();
		}

		GameObject waterObject = GameObject.FindGameObjectWithTag("Water");
		if (waterObject != null)
		{
			waterTilemap = waterObject.GetComponent<Tilemap>();
		}
	}

	private void OnEnable()
	{
		EventHandler.ExecuteActionAfterAnimation += OnExcuteActionAfterAnimation;
		EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
		EventHandler.GameDayEvent += OnGameDayEvent;
		EventHandler.RefreshCurrentMap += RefreshTilemap;
	}

	private void OnDisable()
	{
		EventHandler.ExecuteActionAfterAnimation -= OnExcuteActionAfterAnimation;
		EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
		EventHandler.GameDayEvent -= OnGameDayEvent;
		EventHandler.RefreshCurrentMap -= RefreshTilemap;
	}

	// Initialize the tile details dictionary based on the provided map data
	private void InitTileDetailsDict(MapData_SO mapData)
	{
		foreach (TileProperty tileProperty in mapData.tileProperties)
		{
			TileDetails tileDetails = new TileDetails
			{
				gridX = (int)tileProperty.tileCoordinate.x,
				gridY = (int)tileProperty.tileCoordinate.y
			};

			string key = tileDetails.gridX + "x" + tileDetails.gridY + "y" + mapData.sceneName;

			if (GetTileDetails(key) != null)
			{
				tileDetails = GetTileDetails(key);
			}

			switch (tileProperty.gridType)
			{
				case GridType.Diggable:
					tileDetails.canDig = tileProperty.boolTypeValue;
					break;
				case GridType.DropItem:
					tileDetails.canDropItem = tileProperty.boolTypeValue;
					break;
				case GridType.PlaceFurniture:
					tileDetails.canPlaceFurniture = tileProperty.boolTypeValue;
					break;
				case GridType.NpcObstacle:
					tileDetails.isNPCObstacle = tileProperty.boolTypeValue;
					break;
			}

			if (GetTileDetails(key) != null)
			{
				tileDetailsDict[key] = tileDetails;
			}
			else
			{
				tileDetailsDict.Add(key, tileDetails);
			}
		}
	}

	// Retrieve the tile details based on the key
	public TileDetails GetTileDetails(string key)
	{
		if (tileDetailsDict.ContainsKey(key))
		{
			return tileDetailsDict[key];
		}
		else return null;
	}

	// Retrieve tile details based on mouse position
	public TileDetails GetTileDetailsOnMousePosition(Vector3Int mouseGridPosition)
	{
		string key = mouseGridPosition.x + "x" + mouseGridPosition.y + "y" + SceneManager.GetActiveScene().name;

		return GetTileDetails(key);
	}

	// Handle actions after the scene is loaded
	private void OnAfterSceneLoadEvent()
	{
		currentGrid = FindAnyObjectByType<Grid>();
		GameObject digObject = GameObject.FindGameObjectWithTag("Dig");
		if (digObject != null)
		{
			digTilemap = digObject.GetComponent<Tilemap>();
		}

		GameObject waterObject = GameObject.FindGameObjectWithTag("Water");
		if (waterObject != null)
		{
			waterTilemap = waterObject.GetComponent<Tilemap>();
		}
		if (firstLoadDict[SceneManager.GetActiveScene().name] == true)
		{
			EventHandler.CallGenerateCropEvent();
			firstLoadDict[SceneManager.GetActiveScene().name] = false;
		}

		RefreshTilemap();
	}

	// Execute action after animation, e.g., using tools or planting seeds
	private void OnExcuteActionAfterAnimation(Vector3 mouseWorldPos, ItemDetails itemDetails)
	{
		var mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);
		var currentTile = GetTileDetailsOnMousePosition(mouseGridPos);
		if (currentTile != null)
		{
			Crop currentCrop = GetCrop(mouseWorldPos);
			switch (itemDetails.itemType)
			{
				case ItemType.Product:
					EventHandler.CallDropItemEvent(itemDetails.itemID, mouseWorldPos, itemDetails.itemType);
					break;

				case ItemType.HoeTool:
					SetDigTilemap(currentTile);
					currentTile.daySinceDug = 0;
					currentTile.canDig = false;
					currentTile.canDropItem = false;
					EventHandler.CallPlaySEEvent("Hoe", AudioType.ToolSE);
					break;

				case ItemType.WaterTool:
					SetWaterTilemap(currentTile);
					currentTile.daysSinceWatered = 0;
					currentTile.canDig = false;
					currentTile.canDropItem = false;
					EventHandler.CallPlaySEEvent("Water", AudioType.ToolSE);
					break;

				case ItemType.Seed:
					EventHandler.CallPlantSeedEvent(itemDetails.itemID, currentTile);
					EventHandler.CallPlaySEEvent("Seed", AudioType.ToolSE);
					break;

				case ItemType.CollectTool:
					currentCrop.ProcessToolItem(itemDetails, currentTile);
					break;

				case ItemType.BreakTool:
					EventHandler.CallPlaySEEvent("PickAxe", AudioType.ToolSE);
					currentCrop?.ProcessToolItem(itemDetails, currentCrop.tileDetails);
					break;

				case ItemType.ChopTool:
					EventHandler.CallPlaySEEvent("PickAxe", AudioType.ToolSE);
					currentCrop?.ProcessToolItem(itemDetails, currentCrop.tileDetails);
					break;
				case ItemType.ReapTool:
					for (int i = 0; i < itemInRadius.Count; i++)
					{
						EventHandler.CallParticleEffectEvent(ParticleEffectType.ReapableScenery, itemInRadius[i].transform.position + Vector3.up);
						EventHandler.CallPlaySEEvent("Scythe", AudioType.ToolSE);
						itemInRadius[i].SpawnHarvestItems();
						Destroy(itemInRadius[i].gameObject);
					}
					break;
				case ItemType.Furniture:
					EventHandler.CallBuildFurnitureEvent(itemDetails.itemID, mouseWorldPos);
					break;
			}
			UpdateTileDetails(currentTile);
		}
		else if (itemDetails.itemType == ItemType.ReapTool)
		{
			for (int i = 0; i < itemInRadius.Count; i++)
			{
				EventHandler.CallParticleEffectEvent(ParticleEffectType.ReapableScenery, itemInRadius[i].transform.position + Vector3.up);
				EventHandler.CallPlaySEEvent("Scythe", AudioType.ToolSE);
				itemInRadius[i].SpawnHarvestItems();
				Destroy(itemInRadius[i].gameObject);
			}
		}
	}

	// Get the crop based on mouse position
	public Crop GetCrop(Vector3 mouseWorldPosition)
	{
		Collider2D[] colliders = Physics2D.OverlapPointAll(mouseWorldPosition);
		Crop currentCrop = null;
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].GetComponent<Crop>())
			{
				currentCrop = colliders[i].GetComponent<Crop>();
			}
		}
		return currentCrop;
	}

	// Set the dig tile on the tilemap
	private void SetDigTilemap(TileDetails tile)
	{
		Vector3Int position = new Vector3Int(tile.gridX, tile.gridY, 0);
		if (digTilemap != null)
		{
			digTilemap.SetTile(position, digTile);
		}
	}

	// Set the water tile on the tilemap
	private void SetWaterTilemap(TileDetails tile)
	{
		Vector3Int position = new Vector3Int(tile.gridX, tile.gridY, 0);
		if (waterTilemap != null)
		{
			waterTilemap.SetTile(position, waterTile);
		}
	}

	// Update the details of a tile in the dictionary
	public void UpdateTileDetails(TileDetails tileDetails)
	{
		string key = tileDetails.gridX + "x" + tileDetails.gridY + "y" + SceneManager.GetActiveScene().name;
		if (tileDetailsDict.ContainsKey(key))
		{
			tileDetailsDict[key] = tileDetails;
		}
		else
		{
			tileDetailsDict.Add(key, tileDetails);
		}
	}

	// Display the tilemap for the specified scene
	private void DisplayTilemap(string sceneName)
	{
		foreach (var tile in tileDetailsDict)
		{
			var key = tile.Key;
			var tileDetails = tile.Value;
			if (key.Contains(sceneName))
			{
				if (tileDetails.daySinceDug > -1)
				{
					SetDigTilemap(tileDetails);
				}
				if (tileDetails.daysSinceWatered > -1)
				{
					SetWaterTilemap(tileDetails);
				}
				if (tileDetails.seedItemID > -1)
				{
					EventHandler.CallPlantSeedEvent(tileDetails.seedItemID, tileDetails);
				}
			}
		}
	}

	// Refresh the tilemap visuals by clearing and re-displaying tiles
	private void RefreshTilemap()
	{
		if (digTilemap != null)
		{
			digTilemap.ClearAllTiles();
		}
		if (waterTilemap != null)
		{
			waterTilemap.ClearAllTiles();
		}
		foreach (var crop in FindObjectsByType<Crop>(FindObjectsSortMode.None))
		{
			Destroy(crop.gameObject);
		}
		DisplayTilemap(SceneManager.GetActiveScene().name);
	}

	// Handle game day event to update tile properties
	private void OnGameDayEvent(int day, int season)
	{
		currentSeason = season;
		foreach (var tile in tileDetailsDict)
		{
			if (tile.Value.daysSinceWatered > -1)
			{
				tile.Value.daysSinceWatered = -1;
			}

			if (tile.Value.daySinceDug > -1)
			{
				tile.Value.daySinceDug++;
			}

			if (tile.Value.daySinceDug > 5 && tile.Value.seedItemID == -1)
			{
				tile.Value.daySinceDug = -1;
				tile.Value.canDig = true;
				tile.Value.growthDays = -1;
			}
			if (tile.Value.seedItemID != -1)
			{
				tile.Value.growthDays++;
			}
		}
		RefreshTilemap();
	}

	// Check if there are any reapable items in a specified radius
	public bool HaveReapableItemInRadius(Vector3 mouseWorldPosition, ItemDetails tool)
	{
		itemInRadius = new List<ReapItem>();
		Collider2D[] colliders = Physics2D.OverlapCircleAll(mouseWorldPosition, tool.itemUseRadius);
		foreach (Collider2D collider in colliders)
		{
			if (collider.GetComponent<ReapItem>())
			{
				itemInRadius.Add(collider.GetComponent<ReapItem>());
			}
		}
		return itemInRadius.Count > 0;
	}

	// Generate game save data for tiles and other properties
	public GameSaveData GenerateSaveData()
	{
		GameSaveData saveData = new GameSaveData();
		saveData.tileDetailsDict = this.tileDetailsDict;
		saveData.firstLoadDict = this.firstLoadDict;
		return saveData;
	}

	// Restore data from saved game
	public void RestoreData(GameSaveData saveData)
	{
		this.tileDetailsDict = saveData.tileDetailsDict;
		this.firstLoadDict = saveData.firstLoadDict;
	}
}
