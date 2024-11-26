using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotUI : MonoBehaviour
{
	public TextMeshProUGUI dataTime;
	public TextMeshProUGUI dataScene; 
	private Button currentButton;
	private DataSlot currentData;

	private int index;

	public int Index { get { return index; } }

	private void Awake()
	{
		currentButton = GetComponent<Button>();
		currentButton.onClick.AddListener(LoadGameData);

		index=transform.GetSiblingIndex();
	}
	private void OnEnable()
	{
		SetupSlotUI();
	}

	private void SetupSlotUI()
	{
		currentData = SaveLoadManager.Instance.dataSlots[Index];

		if (currentData != null)
		{
			dataTime.text = currentData.DataTime;
			//dataScene.text = currentData.DataScene;
		}
		else
		{
			dataTime.text = "EMPTY";
			//dataScene.text = "EMPTY";
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
			Debug.Log("新游戏");
			EventHandler.CallStartNewGameEvent(Index);
		}
		MenuUIManager.Instance.menuCavans.SetActive(false);
	}
}
