using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeUI : MonoBehaviour
{
	public RectTransform dayNightImage;
	public RectTransform clockParent;
	public Image seasonImage;
	public TextMeshProUGUI dateText;
	public TextMeshProUGUI timeText;
	public Sprite[] seasonSprites;

	public List<GameObject> clockGameObject;

	private void Awake()
	{
		foreach (var gameObject in clockGameObject)
		{
			gameObject.SetActive(false);
		}

	}
	public void UpdateTimeUI(int minute,int hour)
	{
		timeText.text=string.Format("{0:D2}:{1:D2}", hour, minute);
		//timeText.text = $"{hour:D2}:{minute:D2}:{second:D2}";
	}
	public void UpdateDayMonthYearUI(int year, int month,int day)
	{
		dateText.text = string.Format("{0:D2}.{1:D2}.{2:D2}", year, month,day);
	}

	public void UpdateSeasonUI(int season)
	{
		
		seasonImage.sprite=seasonSprites[season]; 
		
	}

	public void UpdateDayNight()
	{
		dayNightImage.transform.Rotate(0, 0, 15);
	}

	public void UpdateClockUI(int hour)
	{
		for (int i = 0; i < (hour+1)/4; i++)
		{
			clockGameObject[i].SetActive(true);
		}
	}

	public void initialClockUI()
	{
		for (int i = 0; i < clockGameObject.Count; i++)
		{
			clockGameObject[i].SetActive(false);
		}
	}

}
