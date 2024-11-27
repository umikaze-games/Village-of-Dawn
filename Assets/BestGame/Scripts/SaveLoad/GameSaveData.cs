using System.Collections.Generic;
using UnityEngine;

public class GameSaveData
{
    public string dataSceneName;

	//save name and position
	public Dictionary<string, SerializableVector3> characterPosDict;

	public Dictionary<string, List<SceneItem>> sceneItemDict;

	public Dictionary<string, List<SceneFurniture>> sceneFurnitureDict;

	//tiledata
	public Dictionary<string, TileDetails> tileDetailsDict;

	public Dictionary<string, bool> firstLoadDict;

	public Dictionary<string, List<InventoryItem>> inventoryDict;

	//game time
	public Dictionary<string, int> timeDict;

	public int playerMoney;

}
