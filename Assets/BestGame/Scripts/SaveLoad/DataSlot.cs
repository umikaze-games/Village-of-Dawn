using System.Collections.Generic;
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
				return timeData.timeDict["gameYear"] + "/" +
					   timeData.timeDict["gameSeason"] + "/ " +
					   timeData.timeDict["gameMonth"] + "/" +
					   timeData.timeDict["gameDay"] + "/";
			}
			else return string.Empty;
		}
	}
}
