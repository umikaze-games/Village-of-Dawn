using UnityEngine;

public class Box : MonoBehaviour
{
	public InventoryBag_SO boxBagTemplate;

	public InventoryBag_SO boxBagData;

	public GameObject mouseIcon;

	private bool canOpen = false;

	private bool isOpen=false;

	public int index;

	private void OnEnable()
	{
		if (boxBagData == null)
		{
			boxBagData = Instantiate(boxBagTemplate);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			canOpen = true;
			mouseIcon.SetActive(true);
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			canOpen = false;
			mouseIcon.SetActive(false);
		}
	}

	private void Update()
	{
		if (!isOpen && canOpen && Input.GetMouseButtonDown(1))
		{
			EventHandler.CallBagOpenEvent(SlotType.Box, boxBagData);
			isOpen = true;
		}

		if (!canOpen && isOpen)
		{
			EventHandler.CallBagCloseEvent(SlotType.Box, boxBagData);
			isOpen = false;
		}

		if (isOpen && Input.GetKeyDown(KeyCode.Escape))
		{
			EventHandler.CallBagCloseEvent(SlotType.Box, boxBagData);
			isOpen = false;
		}
	}

	public void InitBox(int boxIndex)
	{
		// Set the box index
		index = boxIndex;
		string key = this.name + index;
		if (InventoryManager.Instance.GetBoxDataList(key) != null) // Existing box record
		{
			boxBagData.inventoryItems = InventoryManager.Instance.GetBoxDataList(key);
		}
		else // New box record
		{
			if (index == 0)
			{
				index = InventoryManager.Instance.BoxDataAmount;
			} 
			InventoryManager.Instance.AddBoxDataDict(this);
		}
	}
}