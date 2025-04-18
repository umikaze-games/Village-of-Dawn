using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeManager : SingletonMonoBehaviour<TimeManager>, ISaveable
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

	public string GUID => GetComponent<DataGUID>().guid;

	private void Start()
	{
		// Initialize the clock UI and register this script as saveable
		timeUI.UpdateClockUI(gameHour);
		EventHandler.CallUpdateLightEvent();

		ISaveable saveable = this;
		saveable.RegisterSaveable();
		gamePause = true;
	}

	private void OnEnable()
	{
		EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
		EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
		EventHandler.StartNewGameEvent += OnStartNewGameEvent;
		EventHandler.EndGameEvent += OnEndGameEvent;
		EventHandler.NewDayEvent += OnNewDayEvent;
	}

	private void OnDisable()
	{
		EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
		EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
		EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
		EventHandler.EndGameEvent -= OnEndGameEvent;
		EventHandler.NewDayEvent -= OnNewDayEvent;
	}

	private void OnEndGameEvent()
	{
		// Pause the game when it ends
		gamePause = true;
	}

	private void OnStartNewGameEvent(int obj)
	{
		// Initialize the game time and resume game
		InitialTime();
		gamePause = false;
		timeUI.UpdateDayNight();
		timeUI.UpdateTimeUI(gameMinute, gameHour);
		timeUI.UpdateClockUI(gameHour);
		timeUI.UpdateDayMonthYearUI(gameYear, gameMonth, gameDay);
	}

	private void OnBeforeSceneUnloadEvent()
	{
		// Pause the game when unloading a scene
		gamePause = true;
	}

	private void OnAfterSceneLoadEvent()
	{
		// Resume the game after loading a scene
		gamePause = false;
		timeUI.UpdateDayNight();
		timeUI.UpdateTimeUI(gameMinute, gameHour);
		timeUI.UpdateClockUI(gameHour);
		timeUI.UpdateDayMonthYearUI(gameYear, gameMonth, gameDay);
	}

	private void Update()
	{
		// Update the in-game time if the game is not paused
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

		// Fast-forward time when the T key is held down
		if (Input.GetKey(KeyCode.T))
		{
			for (int i = 0; i < 100; i++)
			{
				UpdateGameTime();
			}
		}
	}

	public int GetGameHour()
	{
		return gameHour;
	}

	public void InitialTime()
	{
		// Set the initial game time values
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
		// Increment game time and handle changes for minutes, hours, days, etc.
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
				EventHandler.CallUpdateLightEvent();

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

								if (gameYear > 9999)
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

	public GameSaveData GenerateSaveData()
	{
		// Generate save data for the current game time
		GameSaveData saveData = new GameSaveData();
		saveData.timeDict = new Dictionary<string, int>();
		saveData.timeDict.Add("gameYear", gameYear);
		saveData.timeDict.Add("gameSeason", gameSeason);
		saveData.timeDict.Add("gameMonth", gameMonth);
		saveData.timeDict.Add("gameDay", gameDay);
		saveData.timeDict.Add("gameHour", gameHour);
		saveData.timeDict.Add("gameMinute", gameMinute);
		saveData.timeDict.Add("gameSecond", gameSecond);

		return saveData;
	}

	public void RestoreData(GameSaveData saveData)
	{
		// Restore game time from saved data
		gameYear = saveData.timeDict["gameYear"];
		gameSeason = saveData.timeDict["gameSeason"];
		gameMonth = saveData.timeDict["gameMonth"];
		gameDay = saveData.timeDict["gameDay"];
		gameHour = saveData.timeDict["gameHour"];
		gameMinute = saveData.timeDict["gameMinute"];
		gameSecond = saveData.timeDict["gameSecond"];
	}

	public void OnNewDayEvent()
	{
		// Advance the game to a new day
		gameDay++;
		gameHour = 6;
		gameMinute = 0;
		gameSecond = 0;
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

					if (gameYear > 9999)
					{
						gameYear = 2025;
						timeUI.UpdateDayMonthYearUI(gameYear, gameMonth, gameDay);
					}
				}
			}
		}
		timeUI.UpdateDayMonthYearUI(gameYear, gameMonth, gameDay);
		timeUI.NewDay();
		timeUI.UpdateTimeUI(gameMinute, gameHour);
		timeUI.UpdateClockUI(gameHour);
	}
}
