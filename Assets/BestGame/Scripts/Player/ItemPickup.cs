using UnityEngine;

public class ItemPickup : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		Item item=other.GetComponent<Item>();
		if (item != null)
		{
			if (item.itemDetails.canPickedup)
			{

				InventoryManager.Instance.AddItem(item, item.itemDetails.canPickedup);
			}
		
		}
	}
}
