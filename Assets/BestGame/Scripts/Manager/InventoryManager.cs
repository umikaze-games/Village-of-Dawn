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
		EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.inventoryItems);
	}
	private void OnEnable()
	{
		EventHandler.DropItemEvent += OnDropItemEvent;
	}

	

	private void OnDisable()
	{
		EventHandler.DropItemEvent -= OnDropItemEvent;
	}

	private void OnDropItemEvent(int ID, Vector3 pos, ItemType itemType)
	{
		RemoveItem(ID, 1);
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

		if (toDestroy)
		{
			Destroy(item.gameObject);
		}
		EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.inventoryItems);
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
		EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.inventoryItems);
	}

	private void RemoveItem(int itemID, int removeAmount)
	{ 
		var index =GetItemIdexInBag(itemID);
		if (playerBag.inventoryItems[index].itemAmount>removeAmount)
		{
			var amount = playerBag.inventoryItems[index].itemAmount;
			var item = new InventoryItem { itemID = itemID, itemAmount = amount };
			playerBag.inventoryItems[index]=item;
		}
		else if (true)
		{
			var item = new InventoryItem {  };
			playerBag.inventoryItems[index]=item;
		}
		EventHandler.CallUpdateInventoryUI(InventoryLocation.Player,playerBag.inventoryItems);
	}

}
