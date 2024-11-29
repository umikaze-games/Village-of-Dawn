using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.InputSystem;

public class DataSlot
{
	// Dictionary storing save data for objects by GUID
	public Dictionary<string, GameSaveData> dataDict=new Dictionary<string, GameSaveData>();

	public string DataTime
	{
		get
		{
			var key = TimeManager.Instance.GUID;

			if (dataDict.ContainsKey(key))
			{
				var timeData = dataDict[key];
				string season;
				switch (timeData.timeDict["gameSeason"])
				{
					case 0:
						season= "Spring";
						break;
					case 1:
						season = "Summer";
						break;
					case 2:
						season = "Autumn";
						break;
					case 3:
						season = "Winter";
						break;
					default:
						season = "Season";
						break;
				}
				return
						season + "/ " + 
						timeData.timeDict["gameYear"] + "/" +
						((int)timeData.timeDict["gameMonth"]).ToString("D2") + "/" +
						((int)timeData.timeDict["gameDay"]).ToString("D2") + "/";
			}
			else return string.Empty;
		}
	}

	public string DataScene
	{
		get
		{
			var key = SceneTransition.Instance.GUID;
			if (dataDict.ContainsKey(key))
			{
				var transitionData = dataDict[key];
				return transitionData.dataSceneName switch
				{
					"Scene01"=>"Farm",
					"Scene02"=>"Home",
					_ => string.Empty
				};
			}
			else return string.Empty;
		}
	}
}
