using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorOverride : MonoBehaviour
{
	private Animator[] animators;
	public SpriteRenderer holdItem;

	public List<AnimatorType> animatorTypes;

	private Dictionary<string, Animator> animatorNameDict = new Dictionary<string, Animator>();

	private void Awake()
	{
		animators = GetComponentsInChildren<Animator>();

		foreach (Animator animator in animators)
		{
			animatorNameDict.Add(animator.name, animator);
		}
	}

	private void OnEnable()
	{
		EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
		EventHandler.HarvestAtPlayerPosition += OnHarvestAtPlayerPosition;
	}

	private void OnDisable()
	{
		EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
		EventHandler.HarvestAtPlayerPosition -= OnHarvestAtPlayerPosition;
	}

	// Handles harvesting at the player position and shows the item sprite
	private void OnHarvestAtPlayerPosition(int ID)
	{
		Sprite itemSprite = InventoryManager.Instance.GetItemDetails(ID).itemOnWorldSprite;

		if (holdItem.enabled == false)
		{
			StartCoroutine(ShowItem(itemSprite));
		}
	}

	// Coroutine to display the item sprite briefly
	private IEnumerator ShowItem(Sprite itemSprite)
	{
		holdItem.enabled = true;
		holdItem.sprite = itemSprite;
		yield return new WaitForSeconds(1);
		holdItem.enabled = false;
	}

	// Handles the event when an item is selected or deselected
	private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
	{
		PartType currentType = itemDetails.itemType switch
		{
			ItemType.Seed => PartType.Carry,
			ItemType.Product => PartType.Carry,
			ItemType.HoeTool => PartType.Hoe,
			ItemType.CollectTool => PartType.Collect,
			ItemType.WaterTool => PartType.Water,
			ItemType.ChopTool => PartType.Chop,
			ItemType.BreakTool => PartType.Broken,
			ItemType.ReapTool => PartType.Reap,
			_ => PartType.None
		};

		if (isSelected == false)
		{
			currentType = PartType.None;
			holdItem.enabled = false;
		}
		else
		{
			if (currentType == PartType.Carry)
			{
				holdItem.sprite = itemDetails.itemOnWorldSprite;
				holdItem.enabled = true;
			}
			else
			{
				holdItem.enabled = false;
			}
		}

		SwitchAnimator(currentType);
	}

	// Switches the animator controller to match the selected part type
	private void SwitchAnimator(PartType partType)
	{
		foreach (var item in animatorTypes)
		{
			if (item.partType == partType)
			{
				animatorNameDict[item.partName.ToString()].runtimeAnimatorController = item.overrideController;
			}
		}
	}
}
