using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneTransition : MonoBehaviour
{
	public GameObject loadingPannel;
	public Slider loadProgressSlider;
	public Transform player;
	private void Awake()
	{
		loadingPannel.GetComponent<CanvasGroup>();
	}
	private void OnSceneTransition(string fromsceneName, string tosceneName, Vector3 transitionPosition)
	{
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

		player.position = transitionPosition;
		loadingPannel.SetActive(false);

		yield return null;
	}


	private void OnEnable()
	{
		EventHandler.transitionEvent += OnSceneTransition;
	}
	private void OnDisable()
	{
		EventHandler.transitionEvent -= OnSceneTransition;
	}

}
