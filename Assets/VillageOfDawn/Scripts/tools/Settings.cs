using UnityEngine;

public class Settings
{
	public const float fadeDuration=0.35f;

	public const float fadeAlpha = 0.7f;

	//time 
	public const float secondThreshold = 0.1f;
	public const int secondHold = 59;
	public const int minuteHold = 59;
	public const int hourHold = 23;
	public const int dayHold = 30;
	public const int seasonHold = 3;

	//day and night
	public const int dayHour = 6;
	public const int nightHour = 18;

	//player data
	public static Vector3 playerInitialPosition=new Vector3(0, 0, 0);
	public const int playerMoney = 2000;
}
