using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotUI : MonoBehaviour
{
	public TextMeshProUGUI dataTime; 
	public TextMeshProUGUI dataScene;

	private Button currentButton;
	private DataSlot currentData; 

	private int index; // Index of this save slot
	public int Index { get { return index; } }

	private void Awake()
	{
		currentButton = GetComponent<Button>();
		currentButton.onClick.AddListener(LoadGameData);
		// Set the index of this save slot based on its sibling index
		index = transform.GetSiblingIndex();
	}

	private void OnEnable()
	{
		EventHandler.UpdateSaveSlotUIEvent += SetupSlotUI;
		EventHandler.CallUpdateSaveSlotUIEvent();
	}

	// Method to set up the save slot UI based on current data
	private void SetupSlotUI()
	{
		currentData = SaveLoadManager.Instance.dataSlots[Index];

		if (currentData != null)
		{
			// Display the save data if available
			dataTime.text = currentData.DataTime; 
			dataScene.text = currentData.DataScene;
		}
		else
		{
			// Set slot as empty if no data available
			dataTime.text = "EMPTY";
			dataScene.text = "EMPTY";
		}
	}

	// Method to load game data when the save slot button is clicked
	private void LoadGameData()
	{
		// Load the saved game if data is present, otherwise start a new game
		if (currentData != null)
		{
			SaveLoadManager.Instance.Load(Index); 
		}
		else
		{
			EventHandler.CallStartNewGameEvent(Index); 
		}

		// Hide the menu canvas after loading or starting a new game
		if (MenuUIManager.Instance.menuCanvas.activeInHierarchy)
		{
			MenuUIManager.Instance.menuCanvas.SetActive(false); // Hide the menu
		}

		EventHandler.CallPlaySEEvent("Confirm", AudioType.PlayerSE);
	}
}
