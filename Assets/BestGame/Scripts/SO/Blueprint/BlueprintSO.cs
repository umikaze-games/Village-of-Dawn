using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlueprintObjectScript", menuName = "BlueprintSO/BlueprintObjectScript")]
public class BlueprintSO : ScriptableObject
{
	public List<BluePrintDetails> bluePrintDataList;

	public BluePrintDetails GetBluePrintDetails(int itemID)
	{
		for (int i = 0; i < bluePrintDataList.Count; i++)
		{
			if (bluePrintDataList[i].itemID==itemID)
			{
				return bluePrintDataList[i];
			}
		}
		return null;
	}
}

[System.Serializable]
public class BluePrintDetails
{
	public int itemID;
	public InventoryItem[] requireItem;
	public GameObject buildPrefab;
}

