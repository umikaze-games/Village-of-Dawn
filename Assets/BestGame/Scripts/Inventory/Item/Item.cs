using UnityEngine;

public class Item : MonoBehaviour
{
	public int itemID;

	private SpriteRenderer spriteRenderer;
	private ItemDetails itemDetails;

	private void Awake()
	{
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
	}

	private void Start()
	{
		if (itemID != 0)
		{
			Init(itemID);
		}
	}

	public void Init(int ID)
	{
		itemID = ID;

		// Inventory获取当前数据
		itemDetails = InventoryManager.Instance.GetItemDetails(itemID);

		if (itemDetails != null)
		{
			spriteRenderer.sprite = itemDetails.itemOnWorldSprite != null ? itemDetails.itemOnWorldSprite : itemDetails.itemIcon;
		}
	}
}
