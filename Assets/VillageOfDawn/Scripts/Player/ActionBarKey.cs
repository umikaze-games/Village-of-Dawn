using UnityEngine;

public class ActionBarKey : MonoBehaviour
{
	[SerializeField]
	private KeyCode keyCode; 
	private SlotUI slotUI; 
	private bool canUse;
	private void Awake()
	{
		slotUI = GetComponent<SlotUI>();
	}

	private void OnEnable()
	{
		EventHandler.GamePauseEvent += OnGamePauseEvent;
	}

	private void OnDisable()
	{
		EventHandler.GamePauseEvent -= OnGamePauseEvent;
	}

	private void OnGamePauseEvent(bool gamePause)
	{
		canUse = !gamePause;
	}

	private void Update()
	{
		if (Input.GetKeyDown(keyCode) && canUse)
		{
			if (slotUI.itemDetails != null)
			{
				// Toggle the selection state of the slot
				slotUI.isSelected = !slotUI.isSelected;

				// Highlight the slot if it is selected, otherwise remove the highlight
				if (slotUI.isSelected)
				{
					slotUI.inventoryUI.HightlightSlot(slotUI.slotIndex);
				}
				else
				{
					slotUI.inventoryUI.HightlightSlot(-1);
				}

				EventHandler.CallItemSelectedEvent(slotUI.itemDetails, slotUI.isSelected);
			}
		}
	}
}
