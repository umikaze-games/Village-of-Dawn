using UnityEngine;

public enum ItemType 
{
	seed,
	product,
	furniture,
	HoeTool, ChopTool, BreakTool, ReapTool, WaterTool, CollectTool,
	ReapableScenery
}

public enum SlotType
{
	bag,
	box,
	shop
}

public enum InventoryType
{ 
	player,
	box
}

public enum PartType
{ 
	none,
	carry,
	hoe,
	broken
}

public enum PartName
{ 
	body,
	hair,
	arm,
	tool
}