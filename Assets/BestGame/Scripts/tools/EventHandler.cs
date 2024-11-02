using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventHandler
{
	public static event Action<InventoryType, List<InventoryItem>> updateInventoryUI;

	public static void OnUpdateInventoryUI(InventoryType inventoryType, List<InventoryItem> inventoryItems)
	{
		updateInventoryUI?.Invoke(inventoryType, inventoryItems);
	}

	public static event Action<int, Vector3>instantiateItemInScene;

	public static void OninstantiateItemInScene(int itemID, Vector3 location)
	{
		instantiateItemInScene?.Invoke(itemID, location);
	}

}
