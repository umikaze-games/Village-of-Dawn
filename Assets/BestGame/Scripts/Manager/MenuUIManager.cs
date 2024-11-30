using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : SingletonMonoBehaviour<MenuUIManager>
{
	public GameObject menuCavans;
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

	private void OnAfterSceneLoadEvent()
	{
		menuCavans.transform.GetChild(0).gameObject.SetActive(false);
	}

	private void ToggleGuideBook()
	{
		bool isOpen = guidebook.activeInHierarchy;
		if (isOpen)
		{ 
			guidebook.SetActive(false);
			Time.timeScale = 1.0f;
			EventHandler.CallPlaySEEvent("Page", AudioType.PlayerSE);
		}
		else
		{
			guidebook.SetActive(true);
			Time.timeScale = 0.0f;
			EventHandler.CallPlaySEEvent("Page", AudioType.PlayerSE);
		}
	}
	private void TogglePausePanel()
	{
		bool isOpen = gamePausePanel.activeInHierarchy;
		if (isOpen)
		{
			gamePausePanel.SetActive(false);
			Time.timeScale = 1.0f;
		}
		else
		{
			gamePausePanel.SetActive(true);
			Time.timeScale = 0.0f;
		}
	}

	public void ReturnMenuCavans()
	{
		gamePausePanel.SetActive(false);

		Time.timeScale = 1.0f;
		EventHandler.CallUpdateSaveSlotUIEvent();
		EventHandler.CallEndGameEvent();

		menuCavans.gameObject.SetActive(true);
		menuPanel.gameObject.SetActive(true);
	}


}
