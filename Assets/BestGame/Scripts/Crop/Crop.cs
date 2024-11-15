using UnityEngine;

public class Crop : MonoBehaviour
{
	public CropDetails cropDetails;
	private int harvestActionCount;
	public TileDetails tileDetails;

	public void ProcessToolItem(ItemDetails tool)
	{
		int requiredActionCount = cropDetails.GetTotalRequiredCount();
		if (harvestActionCount<requiredActionCount)
		{
			harvestActionCount++;
		}

		if (harvestActionCount>=requiredActionCount)
		{
			SpawnHarvestItem();
		}
	}

	public void SpawnHarvestItem()
	{
		for (int i = 0; i < cropDetails.producedAmount; i++)
		{
			EventHandler.CallHarvestAtPlayerPosition(cropDetails.producedItemID);
			Debug.Log("SpawnHarvestItem");
		}
	}
}
