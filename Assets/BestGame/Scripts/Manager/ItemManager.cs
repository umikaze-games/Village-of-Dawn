using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemManager : MonoBehaviour
{
	public Item itemPrefab;
	private Transform itemParent;

	public List<Item>itemsInScene;
	public List<SceneItemData> sceneItemDatas;
	private void Awake()
	{

	}
	private void Start()
	{
		itemParent = GameObject.FindWithTag("ItemParent").transform;
	}
	private void OnEnable()
	{
		EventHandler.instantiateItemInScene += OnInstantiateItemInScene;
	}

	private void OnDisable()
	{
		EventHandler.instantiateItemInScene -= OnInstantiateItemInScene;
	}


	private void OnInstantiateItemInScene(int itemID, Vector3 location)
	{
		var item = Instantiate(itemPrefab,location,Quaternion.identity,itemParent);
		item.itemID = itemID;
	}

	public void SaveItemInLoadScene()
	{
		sceneItemDatas.Clear();
		itemsInScene.Clear();
		itemsInScene = new List<Item>();
		Item[]items=FindObjectsByType<Item>(FindObjectsSortMode.None);
		if (items!=null)
		{
			foreach (var item in items)
			{
				itemsInScene.Add(item);
			}

			for (int i = 0; i < itemsInScene.Count; i++)
			{
				sceneItemDatas.Add(new SceneItemData());
			}

			string sceneName = SceneManager.GetActiveScene().name;

			for (int i = 0; i < itemsInScene.Count; i++)
			{
				sceneItemDatas[i].position = itemsInScene[i].transform.position;
				sceneItemDatas[i].itemID = itemsInScene[i].itemID;
				sceneItemDatas[i].itemAmount = 1;
				sceneItemDatas[i].sceneName = sceneName;
			}
		}

	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.V))
		{
			SaveItemInLoadScene();
		}
	}
}
