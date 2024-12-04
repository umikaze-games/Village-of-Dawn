using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Box : MonoBehaviour
{
	public InventoryBag_SO boxBagTemplate;
	public InventoryBag_SO boxBagData;
	public GameObject mouseIcon;

	private bool canOpen = false;
	private bool isOpen = false;
	public int index;

	private void OnEnable()
	{
		if (boxBagData == null)
		{
			boxBagData = Instantiate(boxBagTemplate); // Instantiate a new bag if none exists
		}

		EventHandler.UpdateBoxEvent += OnUpdateBoxEvent;
	}

	private void OnDisable()
	{
		EventHandler.UpdateBoxEvent -= OnUpdateBoxEvent;
	}

	// Handles player entering the box interaction area
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			canOpen = true;
			mouseIcon.SetActive(true); // Show mouse icon to indicate interaction
		}
	}

	// Handles player exiting the box interaction area
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			canOpen = false;
			mouseIcon.SetActive(false); // Hide mouse icon
		}
	}

	private void Update()
	{
		// Open the box if player presses the right mouse button
		if (!isOpen && canOpen && Input.GetMouseButtonDown(1))
		{
			EventHandler.CallBagOpenEvent(SlotType.Box, boxBagData);
			EventHandler.CallPlaySEEvent("OpenBox", AudioType.PlayerSE);
			isOpen = true;
		}

		// Close the box if player moves away
		if (!canOpen && isOpen)
		{
			EventHandler.CallBagCloseEvent(SlotType.Box, boxBagData);
			EventHandler.CallPlaySEEvent("OpenBox", AudioType.PlayerSE);
			isOpen = false;
		}

		// Close the box if player presses the escape key
		if (isOpen && Input.GetKeyDown(KeyCode.Escape))
		{
			EventHandler.CallBagCloseEvent(SlotType.Box, boxBagData);
			EventHandler.CallPlaySEEvent("OpenBox", AudioType.PlayerSE);
			isOpen = false;
		}
	}

	// Initializes the box with a specific index
	public void InitBox(int boxIndex)
	{
		index = boxIndex;
		var key = this.name + index;
		if (InventoryManager.Instance.GetBoxDataList(key) != null)
		{
			boxBagData.inventoryItems = InventoryManager.Instance.GetBoxDataList(key);
		}
		else
		{
			if (index == 0)
				index = InventoryManager.Instance.BoxDataAmount;
			InventoryManager.Instance.AddBoxDataDict(this);
		}
	}

	// Updates the box inventory data
	private void OnUpdateBoxEvent()
	{
		var key = this.name + index;
		boxBagData.inventoryItems = InventoryManager.Instance.GetBoxDataList(key);
		EventHandler.CallUpdateInventoryUI(InventoryLocation.Box, boxBagData.inventoryItems);
	}
}
