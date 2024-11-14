using UnityEngine;
using UnityEngine.InputSystem;

public class TimeManager : SingletonMonoBehaviour<TimeManager>
{
	private int gameSecond;
	private int gameMinute;
	private int gameHour;
	private int gameDay;
	private int gameMonth;
	private int gameYear;
	private int gameSeason;
	private int monthInSeason = 3;
	private float tikTime;

	public bool gamePause;
	public TimeUI timeUI;

	public int GameSeason
	{
		get { return gameSeason; }
	}
	private void Start()
	{
		initialTime();
		timeUI.UpdateClockUI(gameHour);
	}

	private void OnEnable()
	{
		EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
		EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;

	}

	private void OnDisable()
	{
		EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
		EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;

	}
	private void OnBeforeSceneUnloadEvent()
	{
		gamePause = true;
	}

	private void OnAfterSceneLoadEvent()
	{
		gamePause = false;
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
		if (Input.GetKey(KeyCode.T))
		{
			for (int i = 0; i < 600; i++)
			{
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
		gameYear = 2025;
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
			timeUI.UpdateTimeUI(gameMinute, gameHour);

			if (gameMinute > Settings.minuteHold)
			{
				gameHour++;
				gameMinute = 0;
				timeUI.UpdateDayNight();
				timeUI.UpdateTimeUI(gameMinute, gameHour);
				timeUI.UpdateClockUI(gameHour);

				if (gameHour > Settings.hourHold)
				{
					gameDay++;
					gameHour = 0;
					timeUI.UpdateDayMonthYearUI(gameYear, gameMonth, gameDay);
					timeUI.initialClockUI();

					EventHandler.CallGameDayEvent(gameDay, gameSeason);

					if (gameDay > Settings.dayHold)
					{
						gameDay = 1;
						gameMonth++;
						monthInSeason--;
						timeUI.UpdateDayMonthYearUI(gameYear, gameMonth, gameDay);

						if (gameMonth > 12)
							gameMonth = 1;
						if (monthInSeason == 0)
						{
							gameSeason++;
							timeUI.UpdateSeasonUI(gameSeason);

							if (gameSeason > Settings.seasonHold)
							{
								gameSeason = 0;
								timeUI.UpdateSeasonUI(gameSeason);

								gameYear++;
								timeUI.UpdateDayMonthYearUI(gameYear, gameMonth, gameDay);

								if (gameYear>9999)
								{
									gameYear = 2025;
									timeUI.UpdateDayMonthYearUI(gameYear, gameMonth, gameDay);

								}
							}

						}
					}
				}
			}
		}
	}
}

