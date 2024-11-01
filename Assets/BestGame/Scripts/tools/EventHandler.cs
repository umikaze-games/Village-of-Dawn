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

}
