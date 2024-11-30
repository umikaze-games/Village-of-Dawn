using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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

		EventHandler.UpdateBoxEvent += OnUpdateBoxEvent;
	}
	private void OnDisable()
	{
		EventHandler.UpdateBoxEvent -= OnUpdateBoxEvent;
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
			EventHandler.CallPlaySEEvent("OpenBox", AudioType.PlayerSE);
			isOpen = true;
		}

		if (!canOpen && isOpen)
		{
			EventHandler.CallBagCloseEvent(SlotType.Box, boxBagData);
			EventHandler.CallPlaySEEvent("OpenBox", AudioType.PlayerSE);
			isOpen = false;
		}

		if (isOpen && Input.GetKeyDown(KeyCode.Escape))
		{
			EventHandler.CallBagCloseEvent(SlotType.Box, boxBagData);
			EventHandler.CallPlaySEEvent("OpenBox", AudioType.PlayerSE);
			isOpen = false;
		}

	}
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
	private void OnUpdateBoxEvent()
	{
		var key = this.name + index;
		boxBagData.inventoryItems = InventoryManager.Instance.GetBoxDataList(key);
		EventHandler.CallUpdateInventoryUI(InventoryLocation.Box, boxBagData.inventoryItems);

	}
}