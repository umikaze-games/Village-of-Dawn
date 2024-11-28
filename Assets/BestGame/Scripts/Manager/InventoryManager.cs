using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : SingletonMonoBehaviour<InventoryManager>,ISaveable
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
		ISaveable saveable = this;
		saveable.RegisterSaveable();

		//EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.inventoryItems);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.N))
		{
			foreach (var key in InventoryManager.Instance.boxDataDict.Keys)
			{
				Debug.Log($"Key in boxDataDict: {key}");
			}
			foreach (var key in InventoryManager.Instance.gameSaveData.inventoryDict.Keys)
			{
				Debug.Log($"Key in inventoryDict: {key}");
			}
			EventHandler.CallUpdateBoxEvent();
		}
	}
	private void OnEnable()
	{
		EventHandler.DropItemEvent += OnDropItemEvent;
		EventHandler.HarvestAtPlayerPosition += OnHarvestAtPlayerPosition;
		EventHandler.BagOpenEvent += OnBagOpenEvent;
		EventHandler.BuildFurnitureEvent += OnBuildFurnitureEvent;
		EventHandler.StartNewGameEvent += OnStartNewGameEvent;
	}
	private void OnDisable()
	{
		EventHandler.DropItemEvent -= OnDropItemEvent;
		EventHandler.HarvestAtPlayerPosition -= OnHarvestAtPlayerPosition;
		EventHandler.BagOpenEvent -= OnBagOpenEvent;
		EventHandler.BuildFurnitureEvent += OnBuildFurnitureEvent;
		EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
	}

	private void OnStartNewGameEvent(int obj)
	{
		playerBag = Instantiate(bagTemplate);
		playerMoney = Settings.playerMoney;
		boxDataDict.Clear();
		EventHandler.CallUpdateInventoryUI(InventoryLocation.Player,playerBag.inventoryItems);
	}

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
	private void OnBagOpenEvent(SlotType slottype, InventoryBag_SO bagSO)
	{
		currentBoxBag = bagSO;
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

	//exchange item(playerbag and box)
	public void SwapItem(InventoryLocation locationFrom, int fromIndex, InventoryLocation locationTarget, int targetIndex)
	{
		var currentList = GetItemList(locationFrom);
		var targetList = GetItemList(locationTarget);

		InventoryItem currentItem = currentList[fromIndex];

		if (targetIndex < targetList.Count)
		{
			InventoryItem targetItem = targetList[targetIndex];

			if (targetItem.itemID != 0 && currentItem.itemID != targetItem.itemID)//different item
			{
				currentList[fromIndex] = targetItem;
				targetList[targetIndex] = currentItem;
			}
			else if (currentItem.itemID == targetItem.itemID)//same item
			{
				targetItem.itemAmount += currentItem.itemAmount;
				targetList[targetIndex] = targetItem;
				currentList[fromIndex] = new InventoryItem();//clear item
			}
			else//targe is empty box	
			{
				targetList[targetIndex] = currentItem;
				currentList[fromIndex] = new InventoryItem();
			}
			EventHandler.CallUpdateInventoryUI(locationFrom, currentList);//update UI
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
		bool tradeSuccess = true;
		if (isSell)
		{
			if (playerBag.inventoryItems[index].itemAmount>=amount)
			{
				RemoveItem(item.itemID, amount);
				cost=(int)(cost*item.sellPercentage);
				playerMoney += cost;
				EventHandler.CallPlaySEEvent("Coin", AudioType.PlayerSE);
			}
			else tradeSuccess = false;
		}
		else
		{
			if (playerMoney>=cost)
			{
				if (CheckInventoryCapacity())
				{
					AddItemAtIndex(item.itemID, index,amount);
					playerMoney-=cost;
					EventHandler.CallPlaySEEvent("Coin", AudioType.PlayerSE);
				}
				else tradeSuccess = false;
			}
			else tradeSuccess = false;
		}
		InventoryUI.Instance.UpdateMoneyUI();

		EventHandler.CallUpdateInventoryUI(InventoryLocation.Player,playerBag.inventoryItems);

		EventHandler.CallTradeNotifyEvent(tradeSuccess);
	}

	//ID=blueprintID
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

	public List<InventoryItem> GetBoxDataList(string key)
	{
		if (boxDataDict.ContainsKey(key))
		{
			return boxDataDict[key];
		} 
		return null;
	}

	public void AddBoxDataDict(Box box)
	{
		var key = box.name + box.index;
		if (!boxDataDict.ContainsKey(key))
		{
			boxDataDict.Add(key, box.boxBagData.inventoryItems);
		}
	}

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

	public void RestoreData(GameSaveData saveData)
	{
		this.playerMoney = saveData.playerMoney;
		playerBag = Instantiate(bagTemplate);
		playerBag.inventoryItems = saveData.inventoryDict[playerBag.name];

		gameSaveData = saveData;
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
