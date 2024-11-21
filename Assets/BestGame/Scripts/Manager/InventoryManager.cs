using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : SingletonMonoBehaviour<InventoryManager>
{
	[Header("item data")]
	public ItemDataList_SO itemDataList_SO;

	[Header("inventory data")]
	public InventoryBag_SO playerBag;

	private InventoryBag_SO currentBoxBag;

	public int playerMoney=1000;
	private void Start()
	{
		EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.inventoryItems);
	}
	private void OnEnable()
	{
		EventHandler.DropItemEvent += OnDropItemEvent;
		EventHandler.HarvestAtPlayerPosition += OnHarvestAtPlayerPosition;
	}


	private void OnDisable()
	{
		EventHandler.DropItemEvent -= OnDropItemEvent;
		EventHandler.HarvestAtPlayerPosition -= OnHarvestAtPlayerPosition;
	}

	private void OnHarvestAtPlayerPosition(int ID)
	{
		var index = GetItemIdexInBag(ID);
		AddItemAtIndex(ID, index, 1);
		EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.inventoryItems);
	}

	public void OnDropItemEvent(int ID, Vector3 pos, ItemType itemType)
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

	public void SwapItem(InventoryLocation locationFrom, int fromIndex, InventoryLocation locationTarget, int targetIndex)
	{
		var currentList = GetItemList(locationFrom);
		var targetList = GetItemList(locationTarget);

		InventoryItem currentItem = currentList[fromIndex];

		if (targetIndex < targetList.Count)
		{
			InventoryItem targetItem = targetList[targetIndex];

			if (targetItem.itemID != 0 && currentItem.itemID != targetItem.itemID)
			{
				currentList[fromIndex] = targetItem;
				targetList[targetIndex] = currentItem;
			}
			else if (currentItem.itemID == targetItem.itemID)
			{
				targetItem.itemAmount += currentItem.itemAmount;
				targetList[targetIndex] = targetItem;
				currentList[fromIndex] = new InventoryItem();
			}
			else
			{
				targetList[targetIndex] = currentItem;
				currentList[fromIndex] = new InventoryItem();
			}
			EventHandler.CallUpdateInventoryUI(locationFrom, currentList);
			EventHandler.CallUpdateInventoryUI(locationTarget, targetList);
		}
	}
	private List<InventoryItem> GetItemList(InventoryLocation location)
	{
		return location switch
		{
			InventoryLocation.Player => playerBag.inventoryItems,
			InventoryLocation.Box => currentBoxBag.inventoryItems,
			_ => null,
		};
	}

	private void RemoveItem(int itemID, int removeAmount)
	{ 
		var index =GetItemIdexInBag(itemID);
		if (playerBag.inventoryItems[index].itemAmount>removeAmount)
		{
			var amount = playerBag.inventoryItems[index].itemAmount-removeAmount;
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

	public void TradeItem(ItemDetails item, int amount, bool isSell)
	{
		int cost = item.itemPrice * amount;
		int index = GetItemIdexInBag(item.itemID);
		if (isSell)
		{
			if (playerBag.inventoryItems[index].itemAmount>=amount)
			{
				RemoveItem(item.itemID, amount);
				cost=(int)(cost*item.sellPercentage);
				playerMoney += cost;
		
			}
		}
		else
		{
			if (playerMoney>=cost)
			{
				if (CheckInventoryCapacity())
				{
					AddItemAtIndex(item.itemID, index,amount);
					playerMoney-=cost;
				}
			
			}
		}
		InventoryUI.Instance.UpdateMoneyUI();
		EventHandler.CallUpdateInventoryUI(InventoryLocation.Player,playerBag.inventoryItems);
	}

}
