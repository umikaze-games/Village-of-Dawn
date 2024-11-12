using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEditor.FilePathAttribute;

public class ItemManager : MonoBehaviour
{
	public Item itemPrefab;
	//public Item bounceItemPrefab;
	private Transform itemParent;

	private Transform PlayerTransform => FindAnyObjectByType<PlayerController>().transform;

	//public string GUID => GetComponent<DataGUID>().guid;

	private Dictionary<string, List<SceneItem>> sceneItemDict = new Dictionary<string, List<SceneItem>>();

	//private Dictionary<string, List<SceneFurniture>> sceneFurnitureDict = new Dictionary<string, List<SceneFurniture>>();
	private void Awake()
	{

	}
	private void Start()
	{
		itemParent = GameObject.FindWithTag("ItemParent").transform;
	}

	private void OnStartNewGameEvent(int obj)
	{
		sceneItemDict.Clear();
		//sceneFurnitureDict.Clear();
	}
	private void OnEnable()
	{
		EventHandler.InstantiateItemInScene += OnInstantiateItemInScene;
		EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
		EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
		EventHandler.DropItemEvent += OnDropItemEvent;
	}

	

	private void OnDisable()
	{
		EventHandler.InstantiateItemInScene -= OnInstantiateItemInScene;
		EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
		EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
		EventHandler.DropItemEvent -= OnDropItemEvent;
	}
	private void OnDropItemEvent(int ID, Vector3 mousePos, ItemType itemType)
	{
		if (itemType == ItemType.Seed) return;

		//var item = Instantiate(bounceItemPrefab, PlayerTransform.position, Quaternion.identity, itemParent);
		//item.itemID = ID;
		var dir = (mousePos - PlayerTransform.position).normalized;
		//item.GetComponent<ItemBounce>().InitBounceItem(mousePos, dir);
	}

	private void OnInstantiateItemInScene(int itemID, Vector3 location)
	{
		var item = Instantiate(itemPrefab,location,Quaternion.identity,itemParent);
		item.itemID = itemID;
	}
	//public void SaveItemInLoadScene()
	//{
	//	sceneItemDatas.Clear();
	//	itemsInScene.Clear();
	//	itemsInScene = new List<Item>();
	//	Item[]items=FindObjectsByType<Item>(FindObjectsSortMode.None);
	//	if (items!=null)
	//	{
	//		foreach (var item in items)
	//		{
	//			itemsInScene.Add(item);
	//		}

	//		for (int i = 0; i < itemsInScene.Count; i++)
	//		{
	//			sceneItemDatas.Add(new SceneItemData());
	//		}

	//		string sceneName = SceneManager.GetActiveScene().name;

	//		for (int i = 0; i < itemsInScene.Count; i++)
	//		{
	//			sceneItemDatas[i].position = itemsInScene[i].transform.position;
	//			sceneItemDatas[i].itemID = itemsInScene[i].itemID;
	//			sceneItemDatas[i].itemAmount = 1;
	//			sceneItemDatas[i].sceneName = sceneName;
	//		}
	//	}

	//}

	private void OnBeforeSceneUnloadEvent()
	{
		GetAllSceneItems();
		//GetAllSceneFurniture();
	}

	private void OnAfterSceneLoadEvent()
	{
		itemParent = GameObject.FindWithTag("ItemParent").transform;
		RecreateAllItems();
		//RebuildFurniture();
	}

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

	//private void GetAllSceneFurniture()
	//{
	//	List<SceneFurniture> currentSceneFurniture = new List<SceneFurniture>();

	//	foreach (var item in FindObjectsOfType<Furniture>())
	//	{
	//		SceneFurniture sceneFurniture = new SceneFurniture
	//		{
	//			itemID = item.itemID,
	//			position = new SerializableVector3(item.transform.position)
	//		};
	//		if (item.GetComponent<Box>())
	//			sceneFurniture.boxIndex = item.GetComponent<Box>().index;

	//		currentSceneFurniture.Add(sceneFurniture);
	//	}

	//	if (sceneFurnitureDict.ContainsKey(SceneManager.GetActiveScene().name))
	//	{

	//		sceneFurnitureDict[SceneManager.GetActiveScene().name] = currentSceneFurniture;
	//	}
	//	else
	//	{
	//		sceneFurnitureDict.Add(SceneManager.GetActiveScene().name, currentSceneFurniture);
	//	}
	//}

	//private void RebuildFurniture()
	//{
	//	List<SceneFurniture> currentSceneFurniture = new List<SceneFurniture>();

	//	if (sceneFurnitureDict.TryGetValue(SceneManager.GetActiveScene().name, out currentSceneFurniture))
	//	{
	//		if (currentSceneFurniture != null)
	//		{
	//			foreach (var sceneFurniture in currentSceneFurniture)
	//			{
	//				BluePrintDetails bluePrint = InventoryManager.Instance.bluePrintData.GetBluePrintDetails(sceneFurniture.itemID);
	//				var buildItem = Instantiate(bluePrint.buildPrefab, sceneFurniture.position.ToVector3(), Quaternion.identity, itemParent);
	//				if (buildItem.GetComponent<Box>())
	//				{
	//					buildItem.GetComponent<Box>().InitBox(sceneFurniture.boxIndex);
	//				}
	//			}
	//		}
	//	}
	//}
}
