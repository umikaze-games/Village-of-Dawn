using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : SingletonMonoBehaviour<SaveLoadManager>
{
	// List of all saveable objects
	private List<ISaveable> saveableList = new List<ISaveable>();

	// Save slots for the game
	public List<DataSlot> dataSlots=new List<DataSlot>(new DataSlot[3]);

	// Path to save JSON files
	private string jsonFolder;

	// Current save slot index
	private int currentDataIndex;

	protected  override void Awake()
	{
		base.Awake();
		jsonFolder = Application.persistentDataPath + "/SAVE/";
		ReadSavaData();

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

	// Registers a saveable object to the save manager
	public void RegisterSaveable(ISaveable saveable)
	{
		if (!saveableList.Contains(saveable))
		{
			saveableList.Add(saveable);
		}
	}

	// Saves the game state to the specified save slot
	public void Save(int index)
	{
		DataSlot data = new DataSlot();
		foreach (var saveable in saveableList)
		{
			data.dataDict.Add(saveable.GUID, saveable.GenerateSaveData());
		}
		dataSlots[index] = data;

		var resultPath = jsonFolder + "save" + index + ".json";
		var jsonData = JsonConvert.SerializeObject(dataSlots[index], Formatting.Indented);

		if (!File.Exists(resultPath))
		{
			Directory.CreateDirectory(jsonFolder);
		}

		File.WriteAllText(resultPath, jsonData);
	}

	// Loads the game state from the specified save slot
	public void Load(int index)
	{
		currentDataIndex = index;

		var resultPath = jsonFolder + "save" + index + ".json";

		var stringData = File.ReadAllText(resultPath);

		var jsonData = JsonConvert.DeserializeObject<DataSlot>(stringData);

		foreach (var saveable in saveableList)
		{
			saveable.RestoreData(jsonData.dataDict[saveable.GUID]);
		}
	}

	// Reads metadata for all save slots
	public void ReadSavaData()
	{
		if (Directory.Exists(jsonFolder))
		{
			for (int i = 0; i < dataSlots.Count; i++)
			{
				var resultPath = jsonFolder + "save" + i + ".json";
				if (File.Exists(resultPath))
				{
					var stringData = File.ReadAllText(resultPath);
					var jsonData = JsonConvert.DeserializeObject<DataSlot>(stringData);
					dataSlots[i] = jsonData;
				}
			}
		}
	}

	// Saves the game when the game ends
	private void OnEndGameEvent()
	{
		Save(currentDataIndex);
	}
	// Sets the current save slot index when a new game starts
	private void OnStartNewGameEvent(int index)
	{
		currentDataIndex = index;
	}

}
