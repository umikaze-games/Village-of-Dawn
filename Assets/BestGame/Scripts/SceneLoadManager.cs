using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
public class SceneLoadManager : MonoBehaviour
{
	[SerializeField] private GameObject loadingPanel;
	[SerializeField] private Slider loadProgressSlider;

	private Action onsceneLoaded;
	public static SceneLoadManager instance;
	private void Awake()
	{
		onsceneLoaded = onsceneLoadedCallback;
		if (instance == null)  instance = this;
		else Destroy(this.gameObject);
		DontDestroyOnLoad(this.gameObject);
	}

	public void StartLoading(string sceneName)
	{

		loadingPanel.gameObject.SetActive(true);
		StartCoroutine(LoadSceneAsync(sceneName));
	}
	public IEnumerator LoadSceneAsync(string sceneName)
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
		asyncLoad.allowSceneActivation = false;
		yield return new WaitForSeconds(0.5f);
		while (!asyncLoad.isDone)
		{
			float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
			loadProgressSlider.value = progress; 

			if (asyncLoad.progress >= 0.9f)
			{
				yield return new WaitForSeconds(0.5f);
				asyncLoad.allowSceneActivation = true;
			}

			yield return null;
			onsceneLoaded.Invoke();
		}
	}

	private void onsceneLoadedCallback()
	{
		loadingPanel.gameObject.SetActive(false);
	}

}
