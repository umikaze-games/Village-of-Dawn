using UnityEngine;

public class Item : MonoBehaviour
{
	public int itemID;

	private SpriteRenderer spriteRenderer;
	public ItemDetails itemDetails;
	private BoxCollider2D boxCollider2D;

	private void Awake()
	{
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		boxCollider2D= GetComponent<BoxCollider2D>();
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

		itemDetails = InventoryManager.Instance.GetItemDetails(itemID);

		if (itemDetails != null)
		{
			spriteRenderer.sprite = itemDetails.itemOnWorldSprite != null ? itemDetails.itemOnWorldSprite : itemDetails.itemIcon;
			Vector2 newSize = new Vector2(spriteRenderer.sprite.bounds.size.x, spriteRenderer.sprite.bounds.size.y);
			boxCollider2D.size = newSize;
			boxCollider2D.offset = new Vector2(0, spriteRenderer.sprite.bounds.center.y);
	
		}
	}
}
