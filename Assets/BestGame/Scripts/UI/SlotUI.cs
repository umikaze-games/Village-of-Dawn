using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour,IPointerClickHandler,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerEnterHandler,IPointerExitHandler
{
	[SerializeField]
	private Image itemIconImage;
	[SerializeField]
	private TextMeshProUGUI amountText;
	[SerializeField]
	private Button button;

	public Image highlightImage;

	private InventoryUI inventoryUI;

	private ItemToolTip itemToolTip;

	public SlotType slotType;

	public bool isSelected;

	public ItemDetails itemDetails;

	public int itemAmout;

	public int slotIndex;

	private void Awake()
	{
		inventoryUI= FindFirstObjectByType<InventoryUI>();
		itemToolTip = FindFirstObjectByType<ItemToolTip>();
	}
	private void Start()
	{
		if (itemToolTip != null)
		{
			itemToolTip.gameObject.SetActive(false);
		}
		isSelected = false;
		if (itemDetails.itemID==0)
		{
			UpdateSlotEmpty();
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

	public void UpdateSlot(ItemDetails item, int amout)
	{
		itemDetails = item;
		itemIconImage.sprite = item.itemIcon;
		itemAmout = amout;
		amountText.text = amout.ToString();
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
		if (itemAmout==0)return;
		inventoryUI.dragImage.enabled = true;
		inventoryUI.dragImage.sprite = itemIconImage.sprite;
		inventoryUI.dragImage.SetNativeSize();
		isSelected = true;
		inventoryUI.HightlightSlot(slotIndex);
	}

	public void OnDrag(PointerEventData eventData)
	{
		CursorManager.Instance.currentItem = itemDetails;

		CursorManager.Instance.SetMouseUI(!CursorManager.Instance.CheckCanUseCursor());
	
		inventoryUI.dragImage.gameObject.transform.position=Input.mousePosition;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		inventoryUI.dragImage.enabled = false;
		if (eventData.pointerCurrentRaycast.gameObject != null)
		{
			if (eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>() == null) return;
			var targeSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>();
			int targetSlotIndex = targeSlot.slotIndex;

			if (slotType == SlotType.Bag && targeSlot.slotType == SlotType.Bag)
			{
				InventoryManager.Instance.SwapItem(slotIndex, targetSlotIndex);
			}
			inventoryUI.HightlightSlot(-1);
		}
		else 
		{
			CursorManager.Instance.currentItem = itemDetails;
			//Debug.Log($"{CursorManager.Instance.CheckCanUseCursor()}");
			if (itemDetails.canDropped&& CursorManager.Instance.CheckCanUseCursor() )
			{
				var pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
				EventHandler.CallInstantiateItemInScene(itemDetails.itemID, pos);
				CursorManager.Instance.SetMouseUI(false);
			}
			else
			{
				CursorManager.Instance.SetMouseUI(false);
			}
		
		}

	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (itemDetails==null) return;
		itemToolTip.gameObject.transform.position = transform.position + new Vector3(0,100, 0);
		itemToolTip.SetupTooltip(itemDetails, slotType);
		itemToolTip.gameObject.SetActive(true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		itemToolTip.gameObject.SetActive(false);
	}
}
 