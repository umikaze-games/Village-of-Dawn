using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DataSlot : MonoBehaviour
{
	//string=GUID
	public Dictionary<string, GameSaveData> dataDict=new Dictionary<string, GameSaveData>();

	public string DataTime
	{
		get
		{
			var key = TimeManager.Instance.GUID;

			if (dataDict.ContainsKey(key))
			{
				var timeData = dataDict[key];
				return timeData.timeDict["gameYear"] + "”N/" +
					   timeData.timeDict["gameSeason"] + " / " +
					   timeData.timeDict["gameMonth"] + "ŒŽ/" +
					   timeData.timeDict["gameDay"] + "“ú/";
			}
			else return string.Empty;
		}
	}
}
