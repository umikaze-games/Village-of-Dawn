using UnityEngine;

public class InventoryManager : SingletonMonoBehaviour<InventoryManager>
{
	[Header("•¨•i”˜")]
	public ItemDataList_SO itemDataList_SO;
	[Header("”w•ï”˜")]
	public InventoryBag_SO inventoryBag_SO;

	public ItemDetails GetItemDetails(int ID)
	{
		return itemDataList_SO.itemDataList.Find(i => i.itemID == ID);
	}
	public void AddItem(Item item, bool toDestroy)
	{
		Debug.Log(GetItemDetails(item.itemID).itemID + " Name: " + GetItemDetails(item.itemID).itemName);
		if (toDestroy)
		{
			Destroy(item.gameObject);
		}
	}
}
