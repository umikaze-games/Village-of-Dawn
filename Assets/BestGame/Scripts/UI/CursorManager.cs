using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : SingletonMonoBehaviour<CursorManager>
{
	public Image cannotUseCursorImage;
	[SerializeField]
	private Grid currentGrid;

	private Vector3 mouseWorldPosition;
	private Vector3Int mouseGridPosition;
	public ItemDetails currentItem;

	public Transform playerTransform;

	[SerializeField]
	private bool cursorEnable=true;
	private bool cursorPositionValid;

	private void Start()
	{
		currentGrid = FindAnyObjectByType<Grid>();
	}
	private void OnEnable()
	{
		EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
		EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
		EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
	}

	
	private void OnDisable()
	{
		EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
		EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
		EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
	}
	private void Update()
	{
		CheckCursorValid();
		CheckPlayerInput();
	}

	public void SetMouseUI(bool boolValue)
	{
		cursorEnable=boolValue;
	}

	private void CheckCursorValid()
	{
		mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
		mouseGridPosition = currentGrid.WorldToCell(mouseWorldPosition);

		//Debug.Log("WorldPos:" + mouseWorldPos + "  GridPos:" + mouseGridPos);

		var playerGridPos = currentGrid.WorldToCell(playerTransform.position);

		if (Mathf.Abs(mouseGridPosition.x - playerGridPos.x) > currentItem.itemUseRadius || Mathf.Abs(mouseGridPosition.y - playerGridPos.y) > currentItem.itemUseRadius)
		{
			SetCursorInValid();
			return;
		}

		TileDetails currentTile = GridMapManager.Instance.GetTileDetailsOnMousePosition(mouseGridPosition);

		if (currentTile != null)
		{
			//CropDetails currentCrop = CropManager.Instance.GetCropDetails(currentTile.seedItemID);
			//Crop crop = GridMapManager.Instance.GetCropObject(mouseWorldPos);

			switch (currentItem.itemType)
			{
				case ItemType.Seed: 
					if (currentTile.daySinceDug > -1 && currentTile.seedItemID == -1) SetCursorValid(); 
					else SetCursorInValid();
					break;
				case ItemType.Product: 
					if (currentTile.canDropItem && currentItem.canDropped) SetCursorValid();
					else SetCursorInValid();
					break;
				case ItemType.HoeTool: 
					if (currentTile.canDig) SetCursorValid(); else SetCursorInValid();
					break;
				case ItemType.WaterTool:
					if (currentTile.daySinceDug > -1 && currentTile.daysSinceWatered == -1) SetCursorValid(); 
					else SetCursorInValid();
					break;
				//case ItemType.BreakTool: 
				//case ItemType.ChopTool: 
				//	if (crop != null)
				//	{
				//		if (crop.CanHarvest && crop.cropDetails.CheckToolAvailable(currentItem.itemID)) SetCursorValid(); else SetCursorInValid();
				//	}
				//	else SetCursorInValid();
				//	break;
				//case ItemType.CollectTool:
				//	if (currentCrop != null)
				//	{
				//		if (currentCrop.CheckToolAvailable(currentItem.itemID))
				//			if (currentTile.growthDays >= currentCrop.TotalGrowthDays) SetCursorValid(); else SetCursorInValid();
				//	}
				//	else
				//		SetCursorInValid();
				//	break;
				//case ItemType.ReapTool:
				//	if (GridMapManager.Instance.HaveReapableItemsInRadius(mouseWorldPos, currentItem)) SetCursorValid(); else SetCursorInValid();
				//	break;
				//case ItemType.Furniture:
				//	buildImage.gameObject.SetActive(true);
				//	var bluePrintDetails = InventoryManager.Instance.bluePrintData.GetBluePrintDetails(currentItem.itemID);

				//	if (currentTile.canPlaceFurniture && InventoryManager.Instance.CheckStock(currentItem.itemID) && !HaveFurnitureInRaduis(bluePrintDetails))
				//		SetCursorValid();
				//	else SetCursorInValid();
				//	break;
			}
		}
		else
		{
			SetCursorInValid();
		}
	}

	private void CheckPlayerInput()
	{
		if (Input.GetMouseButtonDown(0)&& cursorPositionValid)
		{
			EventHandler.CallMouseClickedEvent(mouseWorldPosition, currentItem);
		}	
	}

	//xuyao xiugai
	private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
	{

		if (!isSelected)
		{
			currentItem = null;
			cursorEnable = false;
		}
		else
		{
			currentItem = itemDetails;
			cursorEnable = true;
		}
	}

	private void OnBeforeSceneUnloadEvent()
	{
		cursorEnable = false;
	}

	private void OnAfterSceneLoadEvent()
	{
		currentGrid = FindAnyObjectByType<Grid>();
	}

	private void SetCursorValid()
	{
		cursorPositionValid = true;
	}

	private void SetCursorInValid()
	{
		cursorPositionValid = false;
	}

}
