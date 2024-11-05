using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemToolTip : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI nameText;
	[SerializeField] private TextMeshProUGUI typeText;
	[SerializeField] private TextMeshProUGUI descriptionText;
	[SerializeField] private Text coinValueText;
	[SerializeField] private GameObject bottom;

	public void SetupTooltip(ItemDetails itemDetails, SlotType slotType)
	{
		nameText.text = itemDetails.itemName;
		typeText.text = itemDetails.itemType.ToString();
		descriptionText.text = itemDetails.itemDescription;

		if (itemDetails.itemType == ItemType.Seed || itemDetails.itemType == ItemType.Product || itemDetails.itemType == ItemType.Furniture)
		{
			bottom.SetActive(true);

			var price = itemDetails.itemPrice;
			if (slotType == SlotType.Bag)
			{
				price = (int)(price * itemDetails.sellPercentage);
			}

			coinValueText.text = price.ToString();
		}
		else
		{
			bottom.SetActive(false);
		}
	}
}
