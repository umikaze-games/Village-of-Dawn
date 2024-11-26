using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : SingletonMonoBehaviour<SaveLoadManager>
{
	private List<ISaveable> saveableList = new List<ISaveable>();

	public List<DataSlot> dataSlots = new List<DataSlot>(new DataSlot[3]);

	private string jsonFolder;

	private int currentDataIndex;

	protected  override void Awake()
	{
		base.Awake();
		jsonFolder = Application.persistentDataPath + "/SAVE/";
	}

	private void OnEnable()
	{
		EventHandler.StartNewGameEvent += OnStartNewGameEvent;
		EventHandler.EndGameEvent += OnEndGameEvent;
	}

	private void OnDisable()
	{
		EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
		EventHandler.EndGameEvent -= OnEndGameEvent;
	}

	private void OnEndGameEvent()
	{
		Save(currentDataIndex);
	}

	private void OnStartNewGameEvent(int index)
	{
		currentDataIndex = index;
	}

	public void RegisterSaveable(ISaveable saveable)
	{
		if (!saveableList.Contains(saveable))
		{
			saveableList.Add(saveable);
		}
	}

	public void Save(int index)
	{
		DataSlot data = new DataSlot();
		foreach (var saveable in saveableList)
		{
			data.dataDict.Add(saveable.GUID, saveable.GenerateSaveData());
		}
		dataSlots[index] = data;

		var resultPath = jsonFolder + "data" + index + ".json";
		var jsonData = JsonConvert.SerializeObject(dataSlots[index], Formatting.Indented);

		if (!File.Exists(resultPath))
		{
			Directory.CreateDirectory(jsonFolder);
		}

		File.WriteAllText(resultPath, jsonData);
	}

	public void Load(int index)
	{
		currentDataIndex = index;

		var resultPath = jsonFolder + "data" + index + ".json";

		var stringData = File.ReadAllText(resultPath);

		var jsonData = JsonConvert.DeserializeObject<DataSlot>(stringData);

		foreach (var saveable in saveableList)
		{
			saveable.RestoreData(jsonData.dataDict[saveable.GUID]);
		}
	}

	public void ReadSavaData()
	{
		if (Directory.Exists(jsonFolder))
		{
			for (int i = 0; i < dataSlots.Count; i++)
			{
				var resultPath = jsonFolder + "data" + i + ".json";
				if (File.Exists(resultPath))
				{
					var stringData = File.ReadAllText(resultPath);
					var jsonData = JsonConvert.DeserializeObject<DataSlot>(stringData);
					dataSlots[i] = jsonData;
				}
			}
		}
	}


}
