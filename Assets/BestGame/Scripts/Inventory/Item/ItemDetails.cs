using System;
using UnityEngine;

[Serializable]
public class ItemDetails
{
	public int itemID;
	public string itemName;
	public ItemType itemType;
	public string itemDescription;
	public int itemUseRadius;
	public Sprite itemIcon;
	public Sprite itemOnWorldSprite;
	public bool canPickedup;
	public bool canDropped;
	public bool canCarried;
	public int itemPrice;
	
	[Range(0,1)]
	public float sellPercentage;
	
}

[Serializable]
public struct InventoryItem
{
	public int itemID;
	public int itemAmount;

}

[Serializable]
public class AnimatorType
{
	public PartType partType;

	public PartName partName;

	public AnimatorOverrideController overrideController;
}

[Serializable]
public class SceneItemData
{ 
	public Vector3 position;
	public string sceneName;
	public int itemAmount;
	public int itemID;
}

[Serializable]
public class TileProperty
{
	public Vector2 tileCoordinate;
	public bool boolTypeValue;
	public GridType gridType;
}

[Serializable]
public class TileDetails
{
	public int gridX, gridY;

	public bool canDig;
	public bool canDropItem;
	public bool canPlaceFurniture;
	public bool isNPCObstacle;

	public int daysSinceDug = -1;
	public int daysSinceWatered = -1;
	public int seedItemID = -1;
	public int growthDays = -1;
	public int daysSinceLastHarvest = -1;
}
