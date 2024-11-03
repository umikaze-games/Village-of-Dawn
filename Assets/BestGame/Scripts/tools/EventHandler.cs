using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventHandler
{
	public static event Action<InventoryType, List<InventoryItem>> updateInventoryUI;

	public static void CallUpdateInventoryUI(InventoryType inventoryType, List<InventoryItem> inventoryItems)
	{
		updateInventoryUI?.Invoke(inventoryType, inventoryItems);
	}

	public static event Action<int, Vector3>instantiateItemInScene;

	public static void CallInstantiateItemInScene(int itemID, Vector3 location)
	{
		instantiateItemInScene?.Invoke(itemID, location);
	}

	public static event Action<ItemDetails, bool> ItemSelectedEvent;
	public static void CallItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
	{
		ItemSelectedEvent?.Invoke(itemDetails, isSelected);
	}

}
