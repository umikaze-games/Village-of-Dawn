using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
	[Header("PlayerBugUI")]
	[SerializeField]
	private GameObject bagUI;
	private bool bagOpened;
	
	[SerializeField]
	private SlotUI[] playerSlots;


	private void Start()
	{
		for (int i = 0; i < playerSlots.Length; i++)
		{ 
			playerSlots[i].slotIndex = i;
		}
		bagOpened = bagUI.activeInHierarchy;
	}
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.B))
		{
			OpenBagUI();
		}
	}
	private void OnEnable()
	{
		EventHandler.updateInventoryUI += OnUpdateInventoryUI;
	}


	private void OnDisable()
	{
		EventHandler.updateInventoryUI -= OnUpdateInventoryUI;
	}


	private void OnUpdateInventoryUI(InventoryType inventorytype, List<InventoryItem> list)
	{
		switch (inventorytype)
		{
			case InventoryType.player:
				for (int i = 0; i < playerSlots.Length; i++)
				{
					if (list[i].itemAmount>0)
					{
						var item = InventoryManager.Instance.GetItemDetails(list[i].itemID);
						playerSlots[i].UpdateSlot(item, list[i].itemAmount);
					}
					else
					{
						playerSlots[i].UpdateSlotEmpty();
					}
				}

				break;
			case InventoryType.box:
				break;
			default:
				break;
		}
	}

	public void OpenBagUI()
	{
		bagOpened = !bagOpened;
		bagUI.SetActive(bagOpened);
	}
}
