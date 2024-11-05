using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
	public ItemToolTip itemToolTip;

	[Header("DragImage")]
	public Image dragImage;

	[Header("PlayerBugUI")]
	[SerializeField]
	private GameObject bagUI;
	private bool bagOpened;
	
	[SerializeField]
	private SlotUI[]playerSlots;



	private void Start()
	{
		HightlightSlot(-1);
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
			case InventoryType.Player:
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
			case InventoryType.Box:
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

	public void HightlightSlot(int slotIndex)
	{
		for (int i = 0; i < playerSlots.Length; i++)
		{
			if (playerSlots[i].isSelected && i == slotIndex)
			{
				playerSlots[i].highlightImage.gameObject.SetActive(true);
			}
			else
			{
				playerSlots[i].isSelected = false;
				playerSlots[i].highlightImage.gameObject.SetActive(false);
			}


		}


	}
}
