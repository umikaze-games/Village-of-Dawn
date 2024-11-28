using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneTransition : SingletonMonoBehaviour<SceneTransition>,ISaveable
{
	public string startSceneName = string.Empty;

	public GameObject loadingPannel;
	public Slider loadProgressSlider;
	public Transform player;

	public string GUID => GetComponent<DataGUID>().guid;

	protected override void Awake()
	{
		base.Awake();
	}


	private void OnEnable()
	{
		EventHandler.TransitionEvent += OnSceneTransition;
		EventHandler.EndGameEvent += OnEndGameEvent;
		EventHandler.StartNewGameEvent += OnStartNewGameEvent;
	}
	private void OnDisable()
	{
		EventHandler.TransitionEvent -= OnSceneTransition;
		EventHandler.EndGameEvent -= OnEndGameEvent;
		EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
	}

	private void OnStartNewGameEvent(int obj)
	{
		StartCoroutine(LoadSaveDataScene(startSceneName));
	}

	private void OnEndGameEvent()
	{
		StopAllCoroutines();
		StartCoroutine(UnloadScene());
	}

	private void Start()
	{
		ISaveable saveable = this;
		saveable.RegisterSaveable();
	}
	private IEnumerator UnloadScene()
	{
		EventHandler.CallBeforeSceneUnloadEvent();
		yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
	}

	public IEnumerator LoadSaveDataScene(string sceneName)
	{

		if (SceneManager.GetActiveScene().name != "PersistentScene")
		{
			EventHandler.CallBeforeSceneUnloadEvent();
			yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
		}

		yield return LoadSceneSetActive(sceneName);
		EventHandler.CallAfterSceneLoadEvent();
	}

	private IEnumerator LoadSceneSetActive(string sceneName)
	{
		yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

		Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

		SceneManager.SetActiveScene(newScene);
	}

	private void OnSceneTransition(string fromsceneName, string tosceneName, Vector3 transitionPosition)
	{
		EventHandler.CallBeforeSceneUnloadEvent();
		StartCoroutine(TransitionToScenePosition(fromsceneName, tosceneName,transitionPosition));
	}

	IEnumerator TransitionToScenePosition(string fromsceneName, string tosceneName, Vector3 transitionPosition)
	{
		loadProgressSlider.value = 0;
		loadingPannel.SetActive(true);
		yield return new WaitForSeconds(0.5f);
		SceneManager.UnloadSceneAsync(fromsceneName);
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(tosceneName, LoadSceneMode.Additive);
		asyncOperation.allowSceneActivation = false;

		while (!asyncOperation.isDone) 
		{	
			float loadingProgress = Mathf.Clamp01(asyncOperation.progress/0.9f);
			loadProgressSlider.value = loadingProgress;
			if (loadProgressSlider.value >= 0.9)
			{
				yield return new WaitForSeconds(0.5f);
				asyncOperation.allowSceneActivation=true;
			}
		}
		Scene scene=SceneManager.GetSceneByName(tosceneName);
		SceneManager.SetActiveScene(scene);
		EventHandler.CallAfterSceneLoadEvent();
		player.position = transitionPosition;
		loadingPannel.SetActive(false);

		yield return null;
	}

	public GameSaveData GenerateSaveData()
	{
		GameSaveData saveData = new GameSaveData();
		saveData.dataSceneName = SceneManager.GetActiveScene().name;

		return saveData;
	}

	public void RestoreData(GameSaveData saveData)
	{
		StartCoroutine(LoadSaveDataScene(saveData.dataSceneName));
	}
}
