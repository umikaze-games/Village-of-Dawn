using UnityEngine;
using UnityEngine.EventSystems;

	public class ShowItemTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		private SlotUI slotUI;
		private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();

		private void Awake()
		{
			slotUI = GetComponent<SlotUI>();
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (slotUI.itemDetails != null)
			{
				inventoryUI.itemToolTip.gameObject.SetActive(true);
				inventoryUI.itemToolTip.SetupTooltip(slotUI.itemDetails, slotUI.slotType);

				inventoryUI.itemToolTip.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0);
				inventoryUI.itemToolTip.gameObject.transform.position = transform.position + Vector3.up * 60;

				if (slotUI.itemDetails.itemType == ItemType.Furniture)
				{
					inventoryUI.itemToolTip.requiredPanel.SetActive(true);
					inventoryUI.itemToolTip.SetupResourcePanel(slotUI.itemDetails.itemID);
				}
				else
				{
					inventoryUI.itemToolTip.requiredPanel.SetActive(false);
				}
			}
			else
			{
				inventoryUI.itemToolTip.gameObject.SetActive(false);
			}
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			inventoryUI.itemToolTip.gameObject.SetActive(false);
		}
	}


