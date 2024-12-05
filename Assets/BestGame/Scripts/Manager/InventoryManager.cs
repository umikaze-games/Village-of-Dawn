using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : SingletonMonoBehaviour<InventoryManager>, ISaveable
{
	[Header("item data")]
	public ItemDataList_SO itemDataList_SO;

	[Header("inventory data")]
	public InventoryBag_SO playerBag;

	public InventoryBag_SO bagTemplate;

	private InventoryBag_SO currentBoxBag;

	[Header("Blueprint data")]
	public BlueprintSO bluePrintSO;

	[Header("Box")]
	public int playerMoney;

	private Dictionary<string, List<InventoryItem>> boxDataDict = new Dictionary<string, List<InventoryItem>>();

	public GameSaveData gameSaveData;
	public int BoxDataAmount => boxDataDict.Count;

	public string GUID => GetComponent<DataGUID>().guid;

	private void Start()
	{
		// Register this class as saveable to support game saving and loading
		ISaveable saveable = this;
		saveable.RegisterSaveable();
	}

	private void OnEnable()
	{
		// Subscribe to various game events
		EventHandler.DropItemEvent += OnDropItemEvent;
		EventHandler.HarvestAtPlayerPosition += OnHarvestAtPlayerPosition;
		EventHandler.BagOpenEvent += OnBagOpenEvent;
		EventHandler.BuildFurnitureEvent += OnBuildFurnitureEvent;
		EventHandler.StartNewGameEvent += OnStartNewGameEvent;
	}

	private void OnDisable()
	{
		// Unsubscribe from game events to avoid memory leaks
		EventHandler.DropItemEvent -= OnDropItemEvent;
		EventHandler.HarvestAtPlayerPosition -= OnHarvestAtPlayerPosition;
		EventHandler.BagOpenEvent -= OnBagOpenEvent;
		EventHandler.BuildFurnitureEvent -= OnBuildFurnitureEvent;
		EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
	}

	// Called when starting a new game, resetting player inventory and money
	private void OnStartNewGameEvent(int obj)
	{
		playerBag = Instantiate(bagTemplate);
		playerMoney = Settings.playerMoney;
		boxDataDict.Clear();
		EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.inventoryItems);
	}

	// Handle building furniture, reducing items from inventory
	private void OnBuildFurnitureEvent(int ID, Vector3 position)
	{
		RemoveItem(ID, 1);
		BluePrintDetails bluePrintDetails = bluePrintSO.GetBluePrintDetails(ID);
		foreach (var item in bluePrintDetails.requireItem)
		{
			RemoveItem(item.itemID, item.itemAmount);
		}
		EventHandler.CallPlaySEEvent("Pluck", AudioType.CropSE);
	}

	// Handle opening a bag (e.g., box inventory)
	private void OnBagOpenEvent(SlotType slottype, InventoryBag_SO bagSO)
	{
		currentBoxBag = bagSO;
	}

	// Handle harvesting item and adding it to the player inventory
	private void OnHarvestAtPlayerPosition(int ID)
	{
		var index = GetItemIdexInBag(ID);
		AddItemAtIndex(ID, index, 1);
		EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.inventoryItems);
	}

	// Handle dropping an item from the inventory
	public void OnDropItemEvent(int ID, Vector3 pos, ItemType itemType)
	{
		RemoveItem(ID, 1);
		EventHandler.CallPlaySEEvent("Pluck", AudioType.CropSE);
	}

	// Retrieve item details based on the item ID
	public ItemDetails GetItemDetails(int ID)
	{
		for (int i = itemDataList_SO.itemDataList.Count - 1; i >= 0; i--)
		{
			if (itemDataList_SO.itemDataList[i].itemID == ID)
			{
				return itemDataList_SO.itemDataList[i];
			}
		}
		return null;
	}

	// Add item to player inventory and optionally destroy the item in the scene
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

	// Get the index of an item in the player bag
	private int GetItemIdexInBag(int itemID)
	{
		for (int i = 0; i < playerBag.inventoryItems.Count; i++)
		{
			if (playerBag.inventoryItems[i].itemID == itemID)
			{
				return i;
			}
		}
		return -1;
	}

	// Check if there is enough capacity in the inventory
	private bool CheckInventoryCapacity()
	{
		for (int i = 0; i < playerBag.inventoryItems.Count; i++)
		{
			if (playerBag.inventoryItems[i].itemID == 0)
			{
				return true;
			}
		}
		return false;
	}

	// Add item at a specific index in the inventory
	private void AddItemAtIndex(int ID, int index, int amount)
	{
		if (index == -1 && CheckInventoryCapacity())
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

	// Swap items within the player inventory
	public void SwapItem(int fromIndex, int targetIndex)
	{
		InventoryItem currentItem = playerBag.inventoryItems[fromIndex];
		InventoryItem targetItem = playerBag.inventoryItems[targetIndex];

		if (targetItem.itemID != 0)
		{
			playerBag.inventoryItems[fromIndex] = targetItem;
			playerBag.inventoryItems[targetIndex] = currentItem;
		}
		else
		{
			playerBag.inventoryItems[targetIndex] = currentItem;
			playerBag.inventoryItems[fromIndex] = new InventoryItem();
		}
		EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.inventoryItems);
	}

	// Exchange item between player bag and box
	public void SwapItem(InventoryLocation locationFrom, int fromIndex, InventoryLocation locationTarget, int targetIndex)
	{
		var currentList = GetItemList(locationFrom);
		var targetList = GetItemList(locationTarget);

		InventoryItem currentItem = currentList[fromIndex];

		if (targetIndex < targetList.Count)
		{
			InventoryItem targetItem = targetList[targetIndex];

			if (targetItem.itemID != 0 && currentItem.itemID != targetItem.itemID) // different item
			{
				currentList[fromIndex] = targetItem;
				targetList[targetIndex] = currentItem;
			}
			else if (currentItem.itemID == targetItem.itemID) // same item
			{
				targetItem.itemAmount += currentItem.itemAmount;
				targetList[targetIndex] = targetItem;
				currentList[fromIndex] = new InventoryItem(); // clear item
			}
			else // target is empty
			{
				targetList[targetIndex] = currentItem;
				currentList[fromIndex] = new InventoryItem();
			}
			EventHandler.CallUpdateInventoryUI(locationFrom, currentList); // update UI
			EventHandler.CallUpdateInventoryUI(locationTarget, targetList);
		}
	}

	// Get the list of inventory items based on the inventory location
	private List<InventoryItem> GetItemList(InventoryLocation location)
	{
		return location switch
		{
			InventoryLocation.Player => playerBag.inventoryItems,
			InventoryLocation.Box => currentBoxBag.inventoryItems,
			_ => null,
		};
	}

	// Remove a specific amount of an item from the player inventory
	private void RemoveItem(int itemID, int removeAmount)
	{
		var index = GetItemIdexInBag(itemID);
		if (playerBag.inventoryItems[index].itemAmount > removeAmount)
		{
			var amount = playerBag.inventoryItems[index].itemAmount - removeAmount;
			var item = new InventoryItem { itemID = itemID, itemAmount = amount };
			playerBag.inventoryItems[index] = item;
		}
		else
		{
			var item = new InventoryItem { };
			playerBag.inventoryItems[index] = item;
		}
		EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.inventoryItems);
	}

	// Handle trading items, including buying and selling
	public void TradeItem(ItemDetails item, int amount, bool isSell)
	{
		int cost = item.itemPrice * amount;
		int index = GetItemIdexInBag(item.itemID);
		bool tradeSuccess = true;
		if (isSell)
		{
			if (playerBag.inventoryItems[index].itemAmount >= amount)
			{
				RemoveItem(item.itemID, amount);
				cost = (int)(cost * item.sellPercentage);
				playerMoney += cost;
				EventHandler.CallPlaySEEvent("Coin", AudioType.PlayerSE);
			}
			else tradeSuccess = false;
		}
		else
		{
			if (playerMoney >= cost)
			{
				if (CheckInventoryCapacity())
				{
					AddItemAtIndex(item.itemID, index, amount);
					playerMoney -= cost;
					EventHandler.CallPlaySEEvent("Coin", AudioType.PlayerSE);
				}
				else tradeSuccess = false;
			}
			else tradeSuccess = false;
		}
		InventoryUI.Instance.UpdateMoneyUI();

		EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.inventoryItems);

		EventHandler.CallTradeNotifyEvent(tradeSuccess);
	}

	// Check if the player has enough items for a blueprint
	public bool CheckStock(int ID)
	{
		var bluePrintDetails = bluePrintSO.GetBluePrintDetails(ID);

		foreach (var requireItem in bluePrintDetails.requireItem)
		{
			var itemStock = playerBag.GetInventoryItem(requireItem.itemID);
			if (itemStock.itemAmount >= requireItem.itemAmount)
			{
				continue;
			}
			else return false;
		}
		return true;
	}

	// Get the box data list based on the key
	public List<InventoryItem> GetBoxDataList(string key)
	{
		if (boxDataDict.ContainsKey(key))
		{
			return boxDataDict[key];
		}
		return null;
	}

	// Add box data to the dictionary
	public void AddBoxDataDict(Box box)
	{
		var key = box.name + box.index;
		if (!boxDataDict.ContainsKey(key))
		{
			boxDataDict.Add(key, box.boxBagData.inventoryItems);
		}
	}

	// Generate save data for the game, including inventory details
	public GameSaveData GenerateSaveData()
	{
		GameSaveData gameSaveData = new GameSaveData();
		gameSaveData.playerMoney = playerMoney;
		gameSaveData.inventoryDict = new Dictionary<string, List<InventoryItem>>();
		gameSaveData.inventoryDict.Add(playerBag.name, playerBag.inventoryItems);

		foreach (var item in boxDataDict)
		{
			gameSaveData.inventoryDict.Add(item.Key, item.Value);
		}
		return gameSaveData;
	}

	// Restore game data from a save file
	public void RestoreData(GameSaveData saveData)
	{
		this.playerMoney = saveData.playerMoney;
		playerBag = Instantiate(bagTemplate);
		playerBag.inventoryItems = saveData.inventoryDict[playerBag.name];

		foreach (var item in saveData.inventoryDict)
		{
			if (boxDataDict.ContainsKey(item.Key))
			{
				boxDataDict[item.Key] = item.Value;
			}
		}
		EventHandler.CallUpdateBoxEvent();
		EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.inventoryItems);
	}
}
