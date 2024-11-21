using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : SingletonMonoBehaviour<InventoryUI>
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

	[Header("ShopUI")]
	[SerializeField]
	private GameObject baseBag;
	public GameObject shopSlotPrefab;

	[SerializeField]
	private List<SlotUI> baseBagSlots;

	[Header("TradeUI")]
	public TradeUI tradeUI;
	public TextMeshProUGUI playerMoneyUI;


	private void Start()
	{
		HightlightSlot(-1);
		for (int i = 0; i < playerSlots.Length; i++)
		{ 
			playerSlots[i].slotIndex = i;
		}
		bagOpened = bagUI.activeInHierarchy;
		UpdateMoneyUI();
	}
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.B))
		{
			OpenBagUI();
		}
	}

	public void UpdateMoneyUI()
	{
		playerMoneyUI.text=Convert.ToString(InventoryManager.Instance.playerMoney);
	}
	private void OnEnable()
	{
		EventHandler.UpdateInventoryUI += OnUpdateInventoryUI;
		EventHandler.BagOpenEvent += OnBagOpenEvent;
		EventHandler.BagCloseEvent += OnBagCloseEvent;
		EventHandler.ShowTradeUI += OnShowTradeUI;
	}


	private void OnDisable()
	{
		EventHandler.UpdateInventoryUI -= OnUpdateInventoryUI;
		EventHandler.BagOpenEvent -= OnBagOpenEvent;
		EventHandler.BagCloseEvent -= OnBagCloseEvent;
		EventHandler.ShowTradeUI -= OnShowTradeUI;
	}

	private void OnShowTradeUI(ItemDetails item, bool isSell)
	{
		tradeUI.gameObject.SetActive(true);
		tradeUI.SetupTradeUI(item, isSell);
	}

	private void OnBagCloseEvent(SlotType slotType, InventoryBag_SO BagSO)
	{
		baseBag.SetActive(false);
		itemToolTip.gameObject.SetActive(false);
		HightlightSlot(-1);
		foreach (SlotUI slotUI in baseBagSlots)
		{
			Destroy(slotUI.gameObject);
		}
		baseBagSlots.Clear();

		if (slotType == SlotType.Shop)
		{
			bagUI.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
			bagUI.SetActive(false);
			bagOpened = false;
		}
	}

	private void OnBagOpenEvent(SlotType slotType, InventoryBag_SO BagSO)
	{
		GameObject prefab = slotType switch
		{
			SlotType.Shop => shopSlotPrefab,
			_ => null,

		};
		baseBag.gameObject.SetActive(true);
		baseBagSlots=new List<SlotUI>();
		for (int i = 0; i < BagSO.inventoryItems.Count; i++)
		{
			var slot = Instantiate(prefab, baseBag.transform.GetChild(0)).GetComponent<SlotUI>();
			slot.slotIndex = i;
			baseBagSlots.Add(slot);
		}

		if (slotType==SlotType.Shop)
		{
			bagUI.GetComponent<RectTransform>().pivot = new Vector2(-1, 0.5f);
			bagUI.SetActive(true);
			bagOpened = true;
		}
		OnUpdateInventoryUI(InventoryLocation.Box, BagSO.inventoryItems);
	}

	private void OnUpdateInventoryUI(InventoryLocation inventorytype, List<InventoryItem> list)
	{
		switch (inventorytype)
		{
			case InventoryLocation.Player:
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
			case InventoryLocation.Box:
				for (int i = 0; i < baseBagSlots.Count; i++)
				{
					if (list[i].itemAmount > 0)
					{
						var item = InventoryManager.Instance.GetItemDetails(list[i].itemID);
						baseBagSlots[i].UpdateSlot(item, list[i].itemAmount);
					}
					else
					{
						baseBagSlots[i].UpdateSlotEmpty();
					}
				}
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
