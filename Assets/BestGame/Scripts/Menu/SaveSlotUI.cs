using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotUI : MonoBehaviour
{
	public TextMeshProUGUI dataTime;
	public TextMeshProUGUI dataScene; 
	private Button currentButton;

	private int index;

	public int Index { get { return index; } }

	private void Awake()
	{
		currentButton = GetComponent<Button>();
		currentButton.onClick.AddListener(LoadGameData);

		index=transform.GetSiblingIndex();
	}

	private void LoadGameData()
	{
		Debug.Log(Index);
	
	}
}
