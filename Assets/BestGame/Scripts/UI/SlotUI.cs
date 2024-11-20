using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.FilePathAttribute;

public class SlotUI : MonoBehaviour,IPointerClickHandler,IBeginDragHandler,IDragHandler,IEndDragHandler
{
	[SerializeField]
	private Image itemIconImage;
	[SerializeField]
	private TextMeshProUGUI amountText;
	[SerializeField]
	private Button button;

	public Image highlightImage;

	public InventoryUI inventoryUI;

	[SerializeField]
	private ItemToolTip itemToolTip;

	public SlotType slotType;

	public bool isSelected;

	public ItemDetails itemDetails;

	public int itemAmout;

	public int slotIndex;

	public InventoryLocation Location;

	private void Awake()
	{
		inventoryUI = FindFirstObjectByType<InventoryUI>();
	}
	private void Start()
	{

		if (itemToolTip != null)
		{
			itemToolTip.gameObject.SetActive(false);
		}
		isSelected = false;
		if (itemDetails==null)
		{
			UpdateSlotEmpty();
			if (itemToolTip != null)
			{
				itemToolTip.gameObject.SetActive(false);
			}	
		}
	}

	public void UpdateSlotEmpty()
	{
		if (isSelected)
		{
			isSelected = false;
			inventoryUI.HightlightSlot(-1);
			EventHandler.CallItemSelectedEvent(itemDetails,isSelected);
		}
		itemDetails = null;
		itemIconImage.enabled = false;
		amountText.text=string.Empty;
		button.interactable = false;
	}

	public void UpdateSlot(ItemDetails item, int amount)
	{
		itemDetails = item;
		itemIconImage.sprite = item.itemIcon;
		itemAmout = amount;
		amountText.text = amount.ToString();
		itemIconImage.enabled = true;
		button.interactable = true;

	}

	public void OnPointerClick(PointerEventData eventData)
	{

		if (itemDetails == null) return;
		isSelected = !isSelected;
		inventoryUI.HightlightSlot(slotIndex);
		if (slotType == SlotType.Bag)
		{
			EventHandler.CallItemSelectedEvent(itemDetails, isSelected);
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (itemAmout != 0)
		{
			inventoryUI.dragImage.enabled = true;
			inventoryUI.dragImage.sprite = itemIconImage.sprite;
			inventoryUI.dragImage.SetNativeSize();

			isSelected = true;
			inventoryUI.HightlightSlot(slotIndex);
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		inventoryUI.dragImage.transform.position = Input.mousePosition;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
			inventoryUI.dragImage.enabled = false;

		if (eventData.pointerCurrentRaycast.gameObject != null)
		{
			if (eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>() == null)
				return;

			var targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>();
			int targeIndex = targetSlot.slotIndex;

			if (slotType == SlotType.Bag && targetSlot.slotType == SlotType.Bag)
			{
				InventoryManager.Instance.SwapItem(slotIndex, targeIndex);
			}
			else if (slotType == SlotType.Shop && targetSlot.slotType == SlotType.Bag)//buy item
			{
				EventHandler.CallShowTradeUI(itemDetails, false);
			}
			else if (slotType == SlotType.Bag && targetSlot.slotType == SlotType.Shop)//sell item
			{
				EventHandler.CallShowTradeUI(itemDetails, true);
			}
		else if (slotType != SlotType.Shop && targetSlot.slotType != SlotType.Shop && slotType != targetSlot.slotType)
			{
				InventoryManager.Instance.SwapItem(Location, slotIndex, targetSlot.Location, targetSlot.slotIndex);
			}

			inventoryUI.HightlightSlot(-1);
		}
	}


}
 