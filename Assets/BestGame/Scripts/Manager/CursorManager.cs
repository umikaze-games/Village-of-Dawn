using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : SingletonMonoBehaviour<CursorManager>
{
	public Image cannotUseCursorImage;
	private Grid currentGrid;

	private CropDetails currentCrop;
	private Vector3 mouseWorldPosition;
	private Vector3Int mouseGridPosition;
	public ItemDetails currentItem;

	public Transform playerTransform;

	private bool cursorEnable=true;
	private bool cursorPositionValid;

	public Image buildImage;

	public GameObject gridHightlight;
	private void Start()
	{
		currentGrid = FindAnyObjectByType<Grid>();
		buildImage.gameObject.SetActive(false);
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
		if (currentGrid!=null)
		{
			CheckCursorValid();
			CheckPlayerInput();
		}
		else
		{
			buildImage.gameObject.SetActive(false);
		}

	}

	public void SetMouseUI(bool boolValue)
	{
		cursorEnable=boolValue;
	}

	private void CheckCursorValid()
	{
		mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
		mouseGridPosition = currentGrid.WorldToCell(mouseWorldPosition);

		var playerGridPos = currentGrid.WorldToCell(playerTransform.position);

		buildImage.rectTransform.position = Input.mousePosition;

		if (currentItem == null) return;
	
		if (Mathf.Abs(mouseGridPosition.x - playerGridPos.x) > currentItem.itemUseRadius || Mathf.Abs(mouseGridPosition.y - playerGridPos.y) > currentItem.itemUseRadius)
		{
			HideGridHighlight(mouseWorldPosition);
			SetCursorInValid();
			return;
		}

		TileDetails currentTile = GridMapManager.Instance.GetTileDetailsOnMousePosition(mouseGridPosition);

		if (currentTile != null)
		{
			CropDetails currentCrop = CropManager.Instance.GetCropDetails(currentTile.seedItemID);
			Crop crop = GridMapManager.Instance.GetCrop(mouseWorldPosition);

			switch (currentItem.itemType)
			{
				case ItemType.Seed:
					if (currentTile.daySinceDug > -1 && currentTile.seedItemID == -1)
					{
						ShowGridHighlight(mouseWorldPosition);
						SetCursorValid();
					} 
					else 
					{
						HideGridHighlight(mouseWorldPosition);
						SetCursorInValid();
					}
					break;
				case ItemType.Product:
					if (currentTile.canDropItem && currentItem.canDropped)
					{
						ShowGridHighlight(mouseWorldPosition);
						SetCursorValid();
					} 
					else
					{
						HideGridHighlight(mouseWorldPosition);
						SetCursorInValid();
					} 
					break;
				case ItemType.HoeTool:
					if (currentTile.canDig)
					{
						ShowGridHighlight(mouseWorldPosition);
						SetCursorValid();
					}
					else
					{
						HideGridHighlight(mouseWorldPosition);
						SetCursorInValid();	
					} 
					break;
				case ItemType.WaterTool:
					if (currentTile.daySinceDug > -1 && currentTile.daysSinceWatered == -1)
					{
						ShowGridHighlight(mouseWorldPosition);
						SetCursorValid();
					}
					else
					{
						HideGridHighlight(mouseWorldPosition);
						SetCursorInValid();
					}
					
					break;
				case ItemType.BreakTool:
				case ItemType.ChopTool:
				
					if (crop != null && crop.CanHarvest && crop.cropDetails.CheckToolAvaliable(currentItem.itemID))
					{
						ShowGridHighlight(mouseWorldPosition);
						SetCursorValid();
					}
					else
					{
						HideGridHighlight(mouseWorldPosition);
						SetCursorInValid();
					}
					break;
				case ItemType.CollectTool:
					if (currentCrop != null)
					{
						if (currentCrop.CheckToolAvaliable(currentItem.itemID))
							if (currentTile.growthDays >= currentCrop.TotalGrowthDays)
							{
								ShowGridHighlight(mouseWorldPosition);
								SetCursorValid();
							}
							else 
							{
								HideGridHighlight(mouseWorldPosition);
								SetCursorInValid();
							} 
					}
					else
						SetCursorInValid();
					break;
				case ItemType.ReapTool:
					if (GridMapManager.Instance.HaveReapableItemInRadius(mouseWorldPosition, currentItem))
					{
						ShowGridHighlight(mouseWorldPosition);
						SetCursorValid();
					}

					else
					{
						HideGridHighlight(mouseWorldPosition);
						SetCursorInValid();
					} 
					break;

				case ItemType.Furniture:
					buildImage.gameObject.SetActive(true);
					var bluePrintDetails = InventoryManager.Instance.bluePrintSO.GetBluePrintDetails(currentItem.itemID);

					if (currentTile.canPlaceFurniture && InventoryManager.Instance.CheckStock(currentItem.itemID))
					{

						ShowGridHighlight(mouseWorldPosition);
						SetCursorValid();
					}
					else
					{
						HideGridHighlight(mouseWorldPosition);
						SetCursorInValid();	
					} 
					break;
			}
		}
		else
		{
			HideGridHighlight(mouseWorldPosition);
			SetCursorInValid();
		}
	}
	private void ShowGridHighlight(Vector3 mouseWorldPosition)
	{
		Vector3Int gridPosition = currentGrid.WorldToCell(mouseWorldPosition);
		Vector3 cellCenterPosition = currentGrid.GetCellCenterWorld(gridPosition);
		gridHightlight.gameObject.SetActive(true);
		gridHightlight.transform.position = cellCenterPosition;
	}

	private void HideGridHighlight(Vector3 mouseWorldPosition)
	{
		Vector3Int gridPosition = currentGrid.WorldToCell(mouseWorldPosition);
		Vector3 cellCenterPosition = currentGrid.GetCellCenterWorld(gridPosition);
		gridHightlight.gameObject.SetActive(false);
		gridHightlight.transform.position = cellCenterPosition;
	}

	private void CheckPlayerInput()
	{
		if (Input.GetMouseButtonDown(0)&& cursorPositionValid)
		{
			EventHandler.CallMouseClickedEvent(mouseWorldPosition, currentItem);
		}	
	}
	private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
	{

		if (!isSelected)
		{
			currentItem = null;
			cursorEnable = false;
			buildImage.gameObject.SetActive(false);
		}
		else
		{
			currentItem = itemDetails;
			cursorEnable = true;
			if (itemDetails.itemType == ItemType.Furniture)
			{
				buildImage.gameObject.SetActive(true);
				buildImage.sprite = itemDetails.itemOnWorldSprite;
				buildImage.SetNativeSize();
			}
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
