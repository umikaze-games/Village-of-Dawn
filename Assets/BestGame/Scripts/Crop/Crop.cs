using System.Collections;
using UnityEngine;

public class Crop : MonoBehaviour
{
	public CropDetails cropDetails;
	private int harvestActionCount;
	public TileDetails tileDetails;
	private Transform playerTransform => FindAnyObjectByType<PlayerController>().transform;
	private Animator animator;
	public bool CanHarvest => tileDetails.growthDays >= cropDetails.TotalGrowthDays;

	public void ProcessToolItem(ItemDetails tool, TileDetails tile)
	{
		animator = GetComponentInChildren<Animator>();
		tileDetails = tile;
		int requiredActionCount = cropDetails.GetTotalRequiredCount();

		// Increment harvest action count if not yet reached the required action count
		if (harvestActionCount < requiredActionCount)
		{
			harvestActionCount++;
			// Trigger animation if available, otherwise play particle effect
			if (animator != null && cropDetails.hasAnimation)
			{
				animator.SetTrigger("TreeRotation");
				EventHandler.CallParticleEffectEvent(cropDetails.effectType, transform.position + cropDetails.effectPosition); // Play particle effect
			}
			else
			{
				EventHandler.CallParticleEffectEvent(cropDetails.effectType, transform.position + cropDetails.effectPosition); // Play particle effect
			}
		}

		// Once required actions are complete, handle harvest or animation
		if (harvestActionCount >= requiredActionCount)
		{
			if (cropDetails.generateAtPlayerPosition || !cropDetails.hasAnimation)
			{
				SpawnHarvestItem();
			}
			else if (cropDetails.hasAnimation)
			{
				// Determine direction of fall animation based on player position
				if (playerTransform.position.x < transform.position.x)
				{
					animator.SetTrigger("FallLeft");
				}
				else
				{
					animator.SetTrigger("FallRight");
				}
				EventHandler.CallPlaySEEvent("TreeFall", AudioType.CropSE);
				StartCoroutine(AfterAnimSpawnSpawnHarvestItem());
			}
		}
	}

	// Coroutine to wait for fall animation to end before spawning harvest item
	private IEnumerator AfterAnimSpawnSpawnHarvestItem()
	{
		while (!animator.GetCurrentAnimatorStateInfo(0).IsName("FallEnd"))
		{
			yield return null;
		}
		SpawnHarvestItem();
		// Create transfer crop if applicable
		if (cropDetails.transferItemID > 0)
		{
			CreateTransferCrop();
		}
	}

	// Create a new crop from transferred item
	private void CreateTransferCrop()
	{
		tileDetails.seedItemID = cropDetails.transferItemID;
		tileDetails.daysSinceLastHarvest = -1;
		tileDetails.growthDays = 0;

		EventHandler.CallRefreshCurrentMap();
	}

	// Spawn the harvest item(s) at the appropriate location
	public void SpawnHarvestItem()
	{
		for (int i = 0; i < cropDetails.producedAmount; i++)
		{
			if (cropDetails.generateAtPlayerPosition)
			{
				EventHandler.CallHarvestAtPlayerPosition(cropDetails.producedItemID);
			}
			else
			{
				var dirX = transform.position.x > playerTransform.position.x ? 1 : -1;
				var spawnPos = new Vector3(
					transform.position.x + Random.Range(dirX, cropDetails.spawnRadius.x * dirX),
					transform.position.y + Random.Range(-cropDetails.spawnRadius.y, cropDetails.spawnRadius.y),
					0
				);

				EventHandler.CallInstantiateItemInScene(cropDetails.producedItemID, spawnPos);
			}
			EventHandler.CallPlaySEEvent("Pluck", AudioType.CropSE);
		}

		// Update tile details after harvest
		if (tileDetails != null)
		{
			tileDetails.daysSinceLastHarvest = -1;
			tileDetails.seedItemID = -1;

			Destroy(gameObject);
		}
	}
}
