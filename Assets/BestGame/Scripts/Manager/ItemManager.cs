using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ItemManager : SingletonMonoBehaviour<ItemManager>, ISaveable
{
	public Item itemPrefab;

	private Transform itemParent;
	private Transform PlayerTransform => FindAnyObjectByType<PlayerController>().transform;

	public string GUID => GetComponent<DataGUID>().guid;

	private Dictionary<string, List<SceneItem>> sceneItemDict = new Dictionary<string, List<SceneItem>>();

	private Dictionary<string, List<SceneFurniture>> sceneFurnitureDict = new Dictionary<string, List<SceneFurniture>>();

	// Register this class as saveable to support game saving and loading
	private void Start()
	{
		ISaveable saveable = this;
		saveable.RegisterSaveable();
	}

	private void OnEnable()
	{
		// Subscribe to various game events
		EventHandler.InstantiateItemInScene += OnInstantiateItemInScene;
		EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
		EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
		EventHandler.DropItemEvent += OnDropItemEvent;
		EventHandler.BuildFurnitureEvent += OnBuildFurnitureEvent;
		EventHandler.StartNewGameEvent += OnStartNewGameEvent;
	}

	private void OnDisable()
	{
		// Unsubscribe from game events to avoid memory leaks
		EventHandler.InstantiateItemInScene -= OnInstantiateItemInScene;
		EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
		EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
		EventHandler.DropItemEvent -= OnDropItemEvent;
		EventHandler.BuildFurnitureEvent -= OnBuildFurnitureEvent;
		EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
	}

	// Called when starting a new game, resetting scene items and furniture
	private void OnStartNewGameEvent(int obj)
	{
		sceneItemDict.Clear();
		sceneFurnitureDict.Clear();
	}

	// Handle building furniture in the scene at the specified position
	private void OnBuildFurnitureEvent(int ID, Vector3 mosPosition)
	{
		BluePrintDetails bluePrintDetails = InventoryManager.Instance.bluePrintSO.GetBluePrintDetails(ID);
		var buildItem = Instantiate(bluePrintDetails.buildPrefab, mosPosition, Quaternion.identity, itemParent);
		if (buildItem.GetComponent<Box>())
		{
			buildItem.GetComponent<Box>().index = InventoryManager.Instance.BoxDataAmount;
			buildItem.GetComponent<Box>().InitBox(buildItem.GetComponent<Box>().index);
		}
	}

	// Handle dropping an item at a specific position
	private void OnDropItemEvent(int ID, Vector3 mousePos, ItemType itemType)
	{
		if (itemType == ItemType.Seed) return;

		var item = Instantiate(itemPrefab, mousePos, Quaternion.identity, itemParent);
		item.itemID = ID;
	}

	// Handle instantiating an item in the scene at a specific location
	private void OnInstantiateItemInScene(int itemID, Vector3 location)
	{
		var item = Instantiate(itemPrefab, location, Quaternion.identity, itemParent);
		item.itemID = itemID;
	}

	// Handle actions before the scene is unloaded, such as saving scene items and furniture
	private void OnBeforeSceneUnloadEvent()
	{
		GetAllSceneItems();
		GetAllSceneFurniture();
	}

	// Handle actions after the scene is loaded, such as recreating items and furniture
	private void OnAfterSceneLoadEvent()
	{
		itemParent = GameObject.FindWithTag("ItemParent").transform;
		foreach (Transform child in itemParent.transform)
		{
			Destroy(child.gameObject);
		}
		RecreateAllItems();
		RebuildFurniture();
	}

	// Retrieve all items in the current scene and store them in the dictionary
	private void GetAllSceneItems()
	{
		List<SceneItem> currentSceneItems = new List<SceneItem>();

		foreach (var item in FindObjectsByType<Item>(FindObjectsSortMode.None))
		{
			SceneItem sceneItem = new SceneItem
			{
				itemID = item.itemID,
				position = new SerializableVector3(item.transform.position)
			};

			currentSceneItems.Add(sceneItem);
		}

		if (sceneItemDict.ContainsKey(SceneManager.GetActiveScene().name))
		{
			sceneItemDict[SceneManager.GetActiveScene().name] = currentSceneItems;
		}
		else
		{
			sceneItemDict.Add(SceneManager.GetActiveScene().name, currentSceneItems);
		}
	}

	// Recreate all items in the current scene based on saved data
	private void RecreateAllItems()
	{
		List<SceneItem> currentSceneItems = new List<SceneItem>();

		if (sceneItemDict.TryGetValue(SceneManager.GetActiveScene().name, out currentSceneItems))
		{
			if (currentSceneItems != null)
			{
				foreach (var item in FindObjectsByType<Item>(FindObjectsSortMode.None))
				{
					Destroy(item.gameObject);
				}

				foreach (var item in currentSceneItems)
				{
					Item newItem = Instantiate(itemPrefab, item.position.ToVector3(), Quaternion.identity, itemParent);
					newItem.Init(item.itemID);
				}
			}
		}
	}

	// Retrieve all furniture in the current scene and store them in the dictionary
	private void GetAllSceneFurniture()
	{
		List<SceneFurniture> currentSceneFurniture = new List<SceneFurniture>();

		foreach (var item in FindObjectsByType<Furniture>(FindObjectsSortMode.None))
		{
			SceneFurniture sceneFurniture = new SceneFurniture
			{
				itemID = item.itemID,
				position = new SerializableVector3(item.transform.position)
			};
			if (item.GetComponent<Box>())
			{
				sceneFurniture.boxIndex = item.GetComponent<Box>().index;
			}

			currentSceneFurniture.Add(sceneFurniture);
		}

		if (sceneFurnitureDict.ContainsKey(SceneManager.GetActiveScene().name))
		{
			sceneFurnitureDict[SceneManager.GetActiveScene().name] = currentSceneFurniture;
		}
		else
		{
			sceneFurnitureDict.Add(SceneManager.GetActiveScene().name, currentSceneFurniture);
		}
	}

	// Rebuild all furniture in the current scene based on saved data
	public void RebuildFurniture()
	{
		List<SceneFurniture> currentSceneFurniture = new List<SceneFurniture>();

		if (sceneFurnitureDict.TryGetValue(SceneManager.GetActiveScene().name, out currentSceneFurniture))
		{
			if (currentSceneFurniture != null)
			{
				foreach (var sceneFurniture in currentSceneFurniture)
				{
					BluePrintDetails bluePrint = InventoryManager.Instance.bluePrintSO.GetBluePrintDetails(sceneFurniture.itemID);
					var buildItem = Instantiate(bluePrint.buildPrefab, sceneFurniture.position.ToVector3(), Quaternion.identity, itemParent);
					if (buildItem.GetComponent<Box>())
					{
						buildItem.GetComponent<Box>().InitBox(sceneFurniture.boxIndex);
					}
				}
			}
		}
	}

	// Generate save data for the game, including scene items and furniture
	public GameSaveData GenerateSaveData()
	{
		GetAllSceneFurniture();
		GetAllSceneItems();
		GameSaveData gameSaveData = new GameSaveData();
		gameSaveData.sceneItemDict = this.sceneItemDict;
		gameSaveData.sceneFurnitureDict = this.sceneFurnitureDict;

		return gameSaveData;
	}

	// Restore game data from a save file
	public void RestoreData(GameSaveData saveData)
	{
		this.sceneFurnitureDict = saveData.sceneFurnitureDict;
		this.sceneItemDict = saveData.sceneItemDict;
		RecreateAllItems();
		RebuildFurniture();
	}
}
