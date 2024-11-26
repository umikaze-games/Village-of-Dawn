using UnityEngine;

public interface ISaveable
{
	string GUID { get; }
	GameSaveData GenerateSaveData();

	void RestoreData(GameSaveData saveData);

	void RegisterSaveable()
	{
		SaveLoadManager.Instance.RegisterSaveable(this);
	}

}
