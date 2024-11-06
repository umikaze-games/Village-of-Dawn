using UnityEngine;
using UnityEngine.InputSystem;

public class TimeManager : MonoBehaviour
{
	private int gameSecond;
	private int gameMinute;
	private int gameHour;
	private int gameDay;
	private int gameMonth;
	private int gameYear;
	private int gameSeason;
	private int monthInSeason = 3;
	public bool gamePause;
	public float tikTime;
	private void Awake()
	{
		initialTime();
	}

	private void Update()
	{
		if (!gamePause)
		{
			tikTime += Time.deltaTime;
			if (tikTime > Settings.secondThreshold)
			{
				tikTime -= Settings.secondThreshold;
				gameSecond++;
				UpdateGameTime();
			}

		}
	}

	public void initialTime()
	{

		gameSecond = 0;
		gameMinute = 0;
		gameHour = 6;
		gameDay = 1;
		gameMonth = 1;
		gameYear = 2024;
		gameSeason = 0;
		gamePause = false;
	}
	private void UpdateGameTime()
	{
		gameSecond++;
		if (gameSecond > Settings.secondHold)
		{
			gameMinute++;
			gameSecond = 0;

			if (gameMinute > Settings.minuteHold)
			{
				gameHour++;
				gameMinute = 0;

				if (gameHour > Settings.hourHold)
				{
					gameDay++;
					gameHour = 0;

					if (gameDay > Settings.dayHold)
					{
						gameDay = 1;
						gameMonth++;
						monthInSeason--;
						if (gameMonth > 12)
							gameMonth = 1;
						if (monthInSeason == 0)
						{
							gameSeason++;

							if (gameSeason > Settings.seasonHold)
							{
								gameSeason = 0;
								gameYear++;
								if (gameYear>9999)
								{
									gameYear = 2024;
								}
							}

						}
					}
				}
			}
		}
	}
}

