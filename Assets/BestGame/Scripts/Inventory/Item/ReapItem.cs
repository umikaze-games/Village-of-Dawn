using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReapItem : MonoBehaviour
{
		private CropDetails cropDetails;

		private Transform PlayerTransform => FindAnyObjectByType<PlayerController>().transform;

		public void InitCropData(int ID)
		{
			cropDetails = CropManager.Instance.GetCropDetails(ID);
		}

	public void SpawnHarvestItems()
	{
		for (int i = 0; i < cropDetails.producedAmount; i++)
		{
			if (cropDetails.generateAtPlayerPosition)
			{
				EventHandler.CallHarvestAtPlayerPosition(cropDetails.producedItemID);
			}
			else
			{

				var dirX = transform.position.x > PlayerTransform.position.x ? 1 : -1;
				var spawnPos = new Vector3(transform.position.x + Random.Range(dirX, cropDetails.spawnRadius.x * dirX),
					transform.position.y + Random.Range(-cropDetails.spawnRadius.y, cropDetails.spawnRadius.y), 0);

				EventHandler.CallInstantiateItemInScene(cropDetails.producedItemID, spawnPos);
			}
			//EventHandler.CallHarvestAtPlayerPosition(cropDetails.producedItemID);
			//Debug.Log("SpawnHarvestItem");
		}
	}

}


