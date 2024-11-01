using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
	[SerializeField]
	private Image itemIconImage;
	[SerializeField]
	private TextMeshProUGUI amountText;
	[SerializeField]
	private Image highlightImage;
	[SerializeField]
	private Button button;

	public SlotType inventoryType;

	public bool isSelected;

	public ItemDetails itemDetails;

	public int itemAmout;

	public int slotIndex;

	private void Start()
	{
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
		}
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

}
