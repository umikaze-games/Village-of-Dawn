using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    public LightSO lightSO;
    private Light2D currentlight2D;
    private LightDetails currentlightDetails;
	

	private void Awake()
	{
		currentlight2D = GetComponent<Light2D>();
	}

	private void OnEnable()
	{
		EventHandler.UpdateLightEvent += OnUpdateLightEvent;
		EventHandler.AfterSceneLoadEvent += OnUpdateLightEvent;
	}

	private void OnDisable()
	{
		EventHandler.UpdateLightEvent -= OnUpdateLightEvent;
		EventHandler.AfterSceneLoadEvent -= OnUpdateLightEvent;
	}
	private void OnUpdateLightEvent()
	{
		int hour=TimeManager.Instance.GetGameHour();
		if (hour>=10&&hour<=15)
		{
			currentlight2D.intensity=lightSO.GetLightDetails(LightType.Day).lightIntensity;
			currentlight2D.color = lightSO.GetLightDetails(LightType.Day).lightColor;
		}

		else if (hour >= 23||hour <= 4)
		{
			currentlight2D.intensity = lightSO.GetLightDetails(LightType.Night).lightIntensity;
			currentlight2D.color = lightSO.GetLightDetails(LightType.Night).lightColor;
		}

		else if (hour>=5&&hour<=9)
		{
			var nightDetails = lightSO.GetLightDetails(LightType.Night);
			var dayDetails = lightSO.GetLightDetails(LightType.Day);

			float t = (hour - 5) / 4f;

			currentlight2D.intensity = Mathf.Lerp(nightDetails.lightIntensity, dayDetails.lightIntensity, t);
			currentlight2D.color = Color.Lerp(nightDetails.lightColor, dayDetails.lightColor, t);
		}

		else if (hour >= 16 && hour <= 22)
		{
			var dayDetails = lightSO.GetLightDetails(LightType.Day);
			var nightDetails = lightSO.GetLightDetails(LightType.Night);

			float t = (hour - 16) / 6f;

			currentlight2D.intensity = Mathf.Lerp(dayDetails.lightIntensity, nightDetails.lightIntensity, t);
			currentlight2D.color = Color.Lerp(dayDetails.lightColor, nightDetails.lightColor, t);
		}

	}
}
