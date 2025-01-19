using UnityEngine;

public class Item : MonoBehaviour
{
	public int itemID;

	private SpriteRenderer spriteRenderer;
	public ItemDetails itemDetails;
	private BoxCollider2D boxCollider2D;

	private void Awake()
	{
		spriteRenderer = GetComponentInChildren<SpriteRenderer>(); // Get the sprite renderer from child objects
		boxCollider2D = GetComponent<BoxCollider2D>(); // Get the box collider component
	}

	private void Start()
	{
		if (itemID != 0)
		{
			Init(itemID); // Initialize the item if an ID is provided
		}
	}

	// Initialize the item with the given ID
	public void Init(int ID)
	{
		itemID = ID;

		itemDetails = InventoryManager.Instance.GetItemDetails(itemID); // Get the item details from the inventory manager

		if (itemDetails != null)
		{
			// Set the sprite based on the item details, either from world sprite or icon
			spriteRenderer.sprite = itemDetails.itemOnWorldSprite != null ? itemDetails.itemOnWorldSprite : itemDetails.itemIcon;
			Vector2 newSize = new Vector2(spriteRenderer.sprite.bounds.size.x, spriteRenderer.sprite.bounds.size.y);
			boxCollider2D.size = newSize; // Adjust the box collider size to fit the sprite
			boxCollider2D.offset = new Vector2(0, spriteRenderer.sprite.bounds.center.y); // Set the box collider offset
		}

		// If the item type is ReapableScenery, add necessary components for reaping
		if (itemDetails.itemType == ItemType.ReapableScenery)
		{
			gameObject.AddComponent<ReapItem>();
			gameObject.GetComponent<ReapItem>().InitCropData(itemDetails.itemID);
			gameObject.AddComponent<ItemInteractive>();
		}
	}
}
