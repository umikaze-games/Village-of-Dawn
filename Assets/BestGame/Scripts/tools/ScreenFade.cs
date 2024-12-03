using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFade : MonoBehaviour
{
	public Image fadeImage;
	public float fadeDuration = 2f;

	private void Start()
	{
	
	}

	private void OnEnable()
	{
		EventHandler.NewDayEvent += SleepEvent;
	}

	private void OnDisable()
	{
		EventHandler.NewDayEvent -= SleepEvent;
	}
	public void SleepEvent()
	{
		StartCoroutine(SleepEffect());
	}
	private IEnumerator FadeToBlack()
	{
		PlayerController.Instance.inputDisable = true;
		float timer = 0f;
		Color color = fadeImage.color;

		while (timer < fadeDuration)
		{
			timer += Time.deltaTime;
			color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
			fadeImage.color = color;
			yield return null;
		}
		color.a = 1f;
		fadeImage.color = color;
	}

	private IEnumerator FadeToClear()
	{
		float timer = 0f;
		Color color = fadeImage.color;

		while (timer < fadeDuration)
		{
			timer += Time.deltaTime;
			color.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
			fadeImage.color = color;
			yield return null;
		}
		color.a = 0f;
		fadeImage.color = color;
		PlayerController.Instance.inputDisable = false;
	}

	private IEnumerator SleepEffect()
	{
		EventHandler.CallPlaySEEvent("Sleep", AudioType.PlayerSE);
		yield return FadeToBlack(); 
		yield return new WaitForSeconds(1f); 
		yield return FadeToClear(); 
	}
}
