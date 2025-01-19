using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[SerializeField] public GameObject confirmDialogPanel;
	[SerializeField] private TextMeshProUGUI confirmDialogText;
	[SerializeField] private GameObject optionPanel;
	[SerializeField] private Button optionCancel;
	public static UIManager instance;
	private void Awake()
	{
		if (instance == null) instance = this;
		else Destroy(this.gameObject);
		DontDestroyOnLoad(this.gameObject);
		optionCancel.onClick.AddListener(CloseOptionMenu);
	}
	public void ShowConfirmDialogUI(string confirmMessage)
	{
		confirmDialogText.text = confirmMessage;
		confirmDialogPanel.SetActive(true);
	}

	public void CloseConfirmDialogUI()
	{
		confirmDialogPanel.SetActive(false);
	}

	public void ShowOptionMenu()
	{
		optionPanel.SetActive(true);
	}

	public void CloseOptionMenu()
	{
		optionPanel.SetActive(false);
	}

}
