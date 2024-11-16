using UnityEngine;

public class ActionBarKey : MonoBehaviour
{
   [SerializeField]
	private KeyCode keyCode;
	private SlotUI slotUI;
	private void Awake()
	{
		slotUI = GetComponent<SlotUI>();
	}
	private void Update()
	{
		if (Input.GetKeyDown(keyCode))
		{
			if (slotUI.itemDetails!=null)
			{
				slotUI.isSelected = !slotUI.isSelected;
				if (slotUI.isSelected)
				{
					slotUI.inventoryUI.HightlightSlot(slotUI.slotIndex);
				}
				else slotUI.inventoryUI.HightlightSlot(-1);
				EventHandler.CallItemSelectedEvent(slotUI.itemDetails, slotUI.isSelected);
			}
		}
	}
}
