using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
	[SerializeField] private Button startBtn;
	[SerializeField] private Button newgameBtn;
	[SerializeField] private Button OptionBtn;
	[SerializeField] private Button exitgameBtn;
	[SerializeField] private GameObject titlePanel;
	[SerializeField] private Button confirmBtn;
	[SerializeField] private Button cancelBtn;
	[SerializeField] private AudioClip titleMusic;

	public static TitleManager instance;
	private Action onConfirm;
	private Action onCancel;
	private void Awake()
	{
		if (instance == null) instance = this;
		else Destroy(this.gameObject);
		startBtn.onClick.AddListener(StartGame);
		newgameBtn.onClick.AddListener(NewGame);
		exitgameBtn.onClick.AddListener(ShowConfirmDialog);
		confirmBtn.onClick.AddListener(OnConfirmClick);
		cancelBtn.onClick.AddListener(OnCancelClick);
	}
	private void Start()
	{
		OptionBtn.onClick.AddListener(UIManager.instance.ShowOptionMenu);
	}
	private void StartGame()
	{
		AudioManager.instance.PlayBackGroundMusic(titleMusic);
		startBtn.gameObject.SetActive(false);
		titlePanel.SetActive(true);
	}
	private void NewGame()
	{
		string sceneName = "GameScene01";
		startBtn.gameObject.SetActive(false);
		titlePanel.SetActive(true);
		SceneLoadManager.instance.StartLoading(sceneName);
	}
	public void ShowConfirmDialog()
	{
		string confirmMessage = "Exit The Game?";
		UIManager.instance.ShowConfirmDialogUI(confirmMessage);
		onConfirm = SystemManager.instance.QuitGame;
		onCancel = SystemManager.instance.CancelQuit;
	}
	private void OnConfirmClick()
	{
		UIManager.instance.CloseConfirmDialogUI();
		onConfirm?.Invoke();
	}
	private void OnCancelClick()
	{
		UIManager.instance.CloseConfirmDialogUI();
		onCancel?.Invoke();
	}

}
