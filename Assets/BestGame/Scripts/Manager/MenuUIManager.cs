using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : SingletonMonoBehaviour<MenuUIManager>
{
	public GameObject menuCanvas;
	public GameObject menuPanel;
	public Button gamePauseBtn;
	public GameObject gamePausePanel;
	public GameObject guidebook;
	public Button guidebookBtn;
	public Slider volumeSlider;

	protected override void Awake()
	{
		base.Awake();
		gamePauseBtn.onClick.AddListener(TogglePausePanel);
		guidebookBtn.onClick.AddListener(ToggleGuideBook);
	}

	private void Start()
	{
		volumeSlider.onValueChanged.AddListener(FarmAudioManager.Instance.SetMasterVolume);
	}

	private void OnEnable()
	{
		EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
	}

	private void OnDisable()
	{
		EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
	}

	// Hides the menu canvas after a scene load
	private void OnAfterSceneLoadEvent()
	{
		menuCanvas.transform.GetChild(0).gameObject.SetActive(false);
	}

	// Toggles the visibility of the guidebook and pauses/unpauses the game
	private void ToggleGuideBook()
	{
		bool isOpen = guidebook.activeInHierarchy;
		guidebook.SetActive(!isOpen);
		Time.timeScale = isOpen ? 1.0f : 0.0f;
		EventHandler.CallPlaySEEvent("Page", AudioType.PlayerSE);
	}

	// Toggles the visibility of the pause panel and pauses/unpauses the game
	private void TogglePausePanel()
	{
		bool isOpen = gamePausePanel.activeInHierarchy;
		gamePausePanel.SetActive(!isOpen);
		Time.timeScale = isOpen ? 1.0f : 0.0f;
	}

	// Returns to the main menu, resumes game time, updates save slots, and ends the game session
	public void ReturnMenuCanvas()
	{
		gamePausePanel.SetActive(false);
		Time.timeScale = 1.0f;
		EventHandler.CallUpdateSaveSlotUIEvent();
		EventHandler.CallEndGameEvent();
		menuCanvas.SetActive(true);
		menuPanel.SetActive(true);
	}
}
