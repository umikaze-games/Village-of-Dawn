using UnityEngine;

public class ItemManager : MonoBehaviour
{
	public Item itemPrefab;
	private Transform itemParent;

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

}
