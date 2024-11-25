using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeManager : SingletonMonoBehaviour<TimeManager>,ISaveable
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

	public string GUID =>GetComponent<DataGUID>().guid;

	private void Start()
	{
		initialTime();
		timeUI.UpdateClockUI(gameHour);
		EventHandler.CallUpdateLightEvent();
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
	public int GetGameHour()
	{ 
		return gameHour;
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

	public GameSaveData GenerateSaveData()
	{
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
		gameYear = saveData.timeDict["gameYear"];
		gameSeason = saveData.timeDict["gameSeason"];
		gameMonth = saveData.timeDict["gameMonth"];
		gameDay = saveData.timeDict["gameDay"];
		gameHour = saveData.timeDict["gameHour"];
		gameMinute = saveData.timeDict["gameMinute"];
		gameSecond = saveData.timeDict["gameSecond"];
	}
}

