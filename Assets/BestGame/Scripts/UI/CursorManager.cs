using UnityEngine;
using UnityEngine.UI;

public class CursorManager : SingletonMonoBehaviour<CursorManager>
{
	private Image cannotUseCursorImage;
	[SerializeField]
	private Grid currentGrid;

	private Vector3 mouseWorldPosition;
	private Vector3Int mouseGridPosition;
	public ItemDetails currentItem;

	public Transform playerTransform;

	private bool cursorEnable;
	private bool cursorPositionValid;

	protected override void Awake()
	{
		base.Awake();
		cannotUseCursorImage = GetComponentInChildren<Image>();
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
	
		cannotUseCursorImage.transform.position = Input.mousePosition;
		CheckPlayerInput();
	}

	public void UseXCursor(bool boolValue)
	{
		cannotUseCursorImage.enabled = boolValue;

	}

	public bool CheckCanUseCursor()
	{
		mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
		mouseGridPosition = currentGrid.WorldToCell(mouseWorldPosition);
		var playerGridPosition = currentGrid.WorldToCell(playerTransform.position);

		if (currentItem == null || Mathf.Abs(playerGridPosition.x - mouseGridPosition.x) > currentItem.itemUseRadius || Mathf.Abs(playerGridPosition.y - mouseGridPosition.y) > currentItem.itemUseRadius)
		{
			Debug.Log($"{playerGridPosition}{mouseGridPosition}");
			return false;
		}

		TileDetails tileDetails = GridMapManager.Instance.GetTileDetailsOnMousePosition(mouseGridPosition);
	
		if (tileDetails != null)
		{
			switch (currentItem.itemType)
			{
				case ItemType.Product:
					return currentItem.canDropped;
				default:
					return false;
			}
		}
		else
		{
			Debug.Log("No tile details, cannot use cursor.");
			return false;
		}
	}

	private void CheckPlayerInput()
	{
		if (Input.GetMouseButtonDown(0)&&cursorPositionValid)
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

}
