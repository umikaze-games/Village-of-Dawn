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
	Chop,
	Broken,
	Reap
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

public enum ParticleEffectType
{ 
	None,
	LeaveFalling01,
	LeaveFalling02,
	Rock,
	ReapableScenery
}

public enum LightType
{ 
	Day,
	Night
}

public enum AudioType
{ 
	BGM,
	BGS,
	PlayerSE,
	CropSE,
	ToolSE
}