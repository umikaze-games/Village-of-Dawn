using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotUI : MonoBehaviour
{
	// UI elements for displaying data
	public TextMeshProUGUI dataTime;
	public TextMeshProUGUI dataScene;

	// Button component for the save slot
	private Button currentButton;

	// Data for the current save slot
	private DataSlot currentData;

	// Save slot index
	private int index;

	public int Index { get { return index; } }

	private void Awake()
	{
		currentButton = GetComponent<Button>();
		currentButton.onClick.AddListener(LoadGameData);
		index = transform.GetSiblingIndex();
	}
	private void OnEnable()
	{
		EventHandler.UpdateSaveSlotUIEvent += SetupSlotUI;
		EventHandler.CallUpdateSaveSlotUIEvent();
	}
	private void SetupSlotUI()
	{
		currentData = SaveLoadManager.Instance.dataSlots[Index];

		if (currentData != null)
		{
			dataTime.text = currentData.DataTime;
			dataScene.text = currentData.DataScene;
		}
		else
		{
			dataTime.text = "EMPTY";
			dataScene.text = "EMPTY";
		}
	}
	private void LoadGameData()
	{
		if (currentData != null)
		{
			SaveLoadManager.Instance.Load(Index);
		}
		else
		{
			EventHandler.CallStartNewGameEvent(Index);
		}

		if (MenuUIManager.Instance.menuCavans.activeInHierarchy)
		{
			MenuUIManager.Instance.menuCavans.SetActive(false);
		}
	
	}
}
