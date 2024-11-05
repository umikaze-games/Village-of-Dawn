using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : SingletonMonoBehaviour<InventoryManager>
{
	[Header("item data")]
	public ItemDataList_SO itemDataList_SO;

	[Header("inventory data")]
	public InventoryBag_SO playerBag;

	private void Start()
	{
		EventHandler.CallUpdateInventoryUI(InventoryType.Player, playerBag.inventoryItems);
	}

	public ItemDetails GetItemDetails(int ID)
	{
		for (int i = itemDataList_SO.itemDataList.Count - 1; i >= 0; i--)
		{
			if (itemDataList_SO.itemDataList[i].itemID==ID)
			{
				return itemDataList_SO.itemDataList[i];
			}
			
		}
		return null;
	}
	public void AddItem(Item item, bool toDestroy)
	{
		var index = GetItemIdexInBag(item.itemID);
		AddItemAtIndex(item.itemID, index, 1);

		//Debug.Log(GetItemDetails(item.itemID).itemID + " Name: " + GetItemDetails(item.itemID).itemName);
		if (toDestroy)
		{
			Destroy(item.gameObject);
		}
		EventHandler.CallUpdateInventoryUI(InventoryType.Player, playerBag.inventoryItems);
	}

	private int GetItemIdexInBag(int itemID)
	{
		for (int i = 0; i < playerBag.inventoryItems.Count; i++)
		{
			if (playerBag.inventoryItems[i].itemID==itemID)
			{
				return i;
			}
		
		}
		return -1;
	}

	private bool CheckInventoryCapacity()
	{
		for (int i = 0; i < playerBag.inventoryItems.Count; i++)
		{
			if (playerBag.inventoryItems[i].itemID==0)
			{
				return true;
			}
		}
		return false;
	}

	private void AddItemAtIndex(int ID, int index, int amount)
	{
		if (index == -1&& CheckInventoryCapacity())
		{
			// 背包里没有这个物品
			var item = new InventoryItem { itemID = ID, itemAmount = amount };
			for (int i = 0; i < playerBag.inventoryItems.Count; i++)
			{
				if (playerBag.inventoryItems[i].itemID == 0)
				{
					playerBag.inventoryItems[i] = item;
					break;
				}
			}
		}
		else
		{
			// 背包有这个物品
			int currentAmount = playerBag.inventoryItems[index].itemAmount + amount;
			var item = new InventoryItem { itemID = ID, itemAmount = currentAmount };

			playerBag.inventoryItems[index] = item;
		}
	}

	public void SwapItem(int fromIndex,int targetIndex)
	{ 
		InventoryItem currentItem=playerBag.inventoryItems[fromIndex];
		InventoryItem targetItem=playerBag.inventoryItems[targetIndex];

		if (targetItem.itemID!=0)
		{
			playerBag.inventoryItems[fromIndex] = targetItem;
			playerBag.inventoryItems[targetIndex]=currentItem;
		}

		else
		{
			playerBag.inventoryItems[targetIndex] =currentItem;
			playerBag.inventoryItems[fromIndex]=new InventoryItem();
		}
		EventHandler.CallUpdateInventoryUI(InventoryType.Player, playerBag.inventoryItems);
	}

}
