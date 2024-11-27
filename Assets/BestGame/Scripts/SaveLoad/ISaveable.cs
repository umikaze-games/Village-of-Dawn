using UnityEngine;

public interface ISaveable
{
	// Unique identifier for the saveable object
	string GUID { get; }

	// Generates save data for the object
	GameSaveData GenerateSaveData();

	// Restores object state from save data
	void RestoreData(GameSaveData saveData);

	// Registers the object to the SaveLoadManager
	void RegisterSaveable()
	{
		SaveLoadManager.Instance.RegisterSaveable(this);
	}

}
