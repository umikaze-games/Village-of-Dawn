using UnityEngine;

public enum ItemType 
{
	Seed,
	Product,
	Furniture,
	HoeTool, ChopTool, BreakTool, ReapTool, WaterTool, CollectTool,
	ReapableScenery
}

public enum SlotType
{
	Bag,
	Box,
	Shop
}

public enum InventoryLocation
{
	Player,
	Box
}

public enum PartType
{ 
	None,
	Carry,
	Hoe,
	Collect,
	Water,
	Broken
}

public enum PartName
{ 
	Body,
	Hair,
	Arm,
	Tool
}

public enum GridType
{ 
	Diggable,
	DropItem,
	PlaceFurniture,
	NpcObstacle

}

