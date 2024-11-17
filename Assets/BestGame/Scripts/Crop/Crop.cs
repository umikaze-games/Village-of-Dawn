using System.Collections;
using UnityEngine;

public class Crop : MonoBehaviour
{
	public CropDetails cropDetails;
	[SerializeField]
	private int harvestActionCount;
	public TileDetails tileDetails;
	private Transform playerTransform=>FindAnyObjectByType<PlayerController>().transform;
	private Animator animator;
	public bool CanHarvest => tileDetails.growthDays >= cropDetails.TotalGrowthDays;

	public void ProcessToolItem(ItemDetails tool, TileDetails tile)
	{
		animator=GetComponentInChildren<Animator>();
		tileDetails = tile;
		int requiredActionCount = cropDetails.GetTotalRequiredCount();
		if (harvestActionCount<requiredActionCount)
		{
			harvestActionCount++;
			if (animator!=null&&cropDetails.hasAnimation)
			{
				animator.SetTrigger("TreeRotation");
				EventHandler.CallParticleEffectEvent(cropDetails.effectType, transform.position + cropDetails.effectPosition); //play particle effect
			}
			else
			{
				EventHandler.CallParticleEffectEvent(cropDetails.effectType, transform.position + cropDetails.effectPosition); //play particle effect
			}
		}

		if (harvestActionCount>=requiredActionCount)
		{
			if (cropDetails.generateAtPlayerPosition||!cropDetails.hasAnimation)
			{
				SpawnHarvestItem();
			}
			else if (cropDetails.hasAnimation)
			{
				if (playerTransform.position.x < transform.position.x)
				{
					animator.SetTrigger("FallLeft");
				}
				else
				{
					animator.SetTrigger("FallRight");
				}
				
				StartCoroutine(AfterAnimSpawnSpawnHarvestItem());
			}
		}
	}

	private IEnumerator AfterAnimSpawnSpawnHarvestItem()
	{
		while (!animator.GetCurrentAnimatorStateInfo(0).IsName("FallEnd"))
		{
			yield return null;
		}
		SpawnHarvestItem();
		if (cropDetails.transferItemID>0)
		{
			CreateTransferCrop();
		}
	}

	private void CreateTransferCrop()
	{
		
		tileDetails.seedItemID = cropDetails.transferItemID;
		tileDetails.daysSinceLastHarvest = -1;
		tileDetails.growthDays = 0;
		//Debug.Log($"{tileDetails.seedItemID}");

		EventHandler.CallRefreshCurrentMap();
	}
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
				var spawnPos = new Vector3(transform.position.x + Random.Range(dirX, cropDetails.spawnRadius.x * dirX),
					transform.position.y + Random.Range(-cropDetails.spawnRadius.y, cropDetails.spawnRadius.y), 0);

				EventHandler.CallInstantiateItemInScene(cropDetails.producedItemID, spawnPos);
			}
			//EventHandler.CallHarvestAtPlayerPosition(cropDetails.producedItemID);
			//Debug.Log("SpawnHarvestItem");
		}

		if (tileDetails != null)
		{
			tileDetails.daysSinceLastHarvest = -1;
			tileDetails.seedItemID = -1;

			Destroy(gameObject);
		}
	}
}
