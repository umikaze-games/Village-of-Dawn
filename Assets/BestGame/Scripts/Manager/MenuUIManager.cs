using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : SingletonMonoBehaviour<MenuUIManager>
{
	public GameObject menuCavans;
	public GameObject menuPanel;	
	public Button gamePauseBtn;
	public GameObject gamePausePanel;
	public Slider volumeSlider;

	protected override void Awake()
	{
		base.Awake();
		gamePauseBtn.onClick.AddListener(TogglePausePanel);

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

		menuCavans.gameObject.SetActive(true);
		menuPanel.gameObject.SetActive(true);
		EventHandler.CallEndGameEvent();
	}


}
