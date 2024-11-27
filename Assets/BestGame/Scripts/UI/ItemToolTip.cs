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

	[Header("Build")]
	[SerializeField]
	public GameObject requiredPanel;
	[SerializeField]
	private Image[] requeiredImage;

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

	public void SetupResourcePanel(int ID)
	{
		var bluePrintDetails = InventoryManager.Instance.bluePrintSO.GetBluePrintDetails(ID);
		for (int i = 0; i < requeiredImage.Length; i++)
		{
			if (i < bluePrintDetails.requireItem.Length)
			{
				var item = bluePrintDetails.requireItem[i];
				requeiredImage[i].gameObject.SetActive(true);
				requeiredImage[i].sprite = InventoryManager.Instance.GetItemDetails(item.itemID).itemIcon;
				requeiredImage[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.itemAmount.ToString();
			}
			else
			{
				requeiredImage[i].gameObject.SetActive(false);
			}
		}
	}
}
