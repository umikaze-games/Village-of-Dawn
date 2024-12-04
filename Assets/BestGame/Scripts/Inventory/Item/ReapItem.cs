using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReapItem : MonoBehaviour
{
	private CropDetails cropDetails;

	private Transform PlayerTransform => FindAnyObjectByType<PlayerController>().transform;

	// Initialize the crop data with the given ID
	public void InitCropData(int ID)
	{
		cropDetails = CropManager.Instance.GetCropDetails(ID);
	}

	// Spawn the harvest items based on crop details
	public void SpawnHarvestItems()
	{
		for (int i = 0; i < cropDetails.producedAmount; i++)
		{
			if (cropDetails.generateAtPlayerPosition)
			{
				EventHandler.CallHarvestAtPlayerPosition(cropDetails.producedItemID); // Spawn item at player's position
			}
			else
			{
				// Calculate spawn position based on player's position
				var dirX = transform.position.x > PlayerTransform.position.x ? 1 : -1;
				var spawnPos = new Vector3(
					transform.position.x + Random.Range(dirX, cropDetails.spawnRadius.x * dirX),
					transform.position.y + Random.Range(-cropDetails.spawnRadius.y, cropDetails.spawnRadius.y),
					0
				);

				EventHandler.CallInstantiateItemInScene(cropDetails.producedItemID, spawnPos); // Spawn item in the scene
			}
		}
	}
}
