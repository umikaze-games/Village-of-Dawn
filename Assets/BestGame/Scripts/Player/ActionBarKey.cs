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
		if (Input.GetKeyDown(keyCode)&&canUse)
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
