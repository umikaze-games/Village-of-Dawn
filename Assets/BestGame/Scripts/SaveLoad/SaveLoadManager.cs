using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : SingletonMonoBehaviour<SaveLoadManager>
{
	private List<ISaveable> saveablesList = new List<ISaveable>();

	public void RegisterSaveable(ISaveable saveable)
	{
		if (!saveablesList.Contains(saveable))
		{
			saveablesList.Add(saveable);
		}
	}
}
