using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridMapManager : SingletonMonoBehaviour<GridMapManager>
{
	public List<MapData_SO> mapDatalist;

	private Grid currentGrid;
	
	private Dictionary<string, TileDetails> tileDetailsDict = new Dictionary<string, TileDetails>();

	private Dictionary<string, bool> firstLoadDict = new Dictionary<string, bool>();
	private void Start()
	{
		foreach (var mapData in mapDatalist)
		{
			firstLoadDict.Add(mapData.sceneName, true);
			InitTileDetailsDict(mapData);
		}
	}

	private void OnEnable()
	{
		EventHandler.ExecuteActionAfterAnimation += OnExcuteActionAfterAnimation;
		EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
	}

	private void OnDisable()
	{
		EventHandler.ExecuteActionAfterAnimation -= OnExcuteActionAfterAnimation;
		EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
	}

	private void InitTileDetailsDict(MapData_SO mapData)
	{
		foreach (TileProperty tileProperty in mapData.tileProperties)
		{
			TileDetails tileDetails = new TileDetails
			{
				gridX= (int)tileProperty.tileCoordinate.x,
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
	public TileDetails GetTileDetails(string key)
	{
		if (tileDetailsDict.ContainsKey(key))
		{
			return tileDetailsDict[key];
		}
		else return null;
	}

	public TileDetails GetTileDetailsOnMousePosition(Vector3Int mouseGridPosition)
	{
		string key = mouseGridPosition.x + "x" + mouseGridPosition.y + "y" + SceneManager.GetActiveScene().name;

		return GetTileDetails(key);
	}

	private void OnAfterSceneLoadEvent()
	{
		currentGrid = FindAnyObjectByType<Grid>();
	}

	private void OnExcuteActionAfterAnimation(Vector3 mousePos, ItemDetails itemdetails)
	{
		var mouseGridPos=currentGrid.WorldToCell(mousePos);
		var currentTile = GetTileDetailsOnMousePosition(mouseGridPos);
        if (currentTile!=null)
        {
			switch (itemdetails.itemType)
			{
				case ItemType.Product:
					EventHandler.CallDropItemInScene(itemdetails.itemID, mouseGridPos);
				break;
			}
		}
    }
}
