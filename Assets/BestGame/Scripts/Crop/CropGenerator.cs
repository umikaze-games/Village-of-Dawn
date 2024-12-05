using UnityEngine;

public class CropGenerator : MonoBehaviour
{
	[SerializeField]
	private Grid currentGrid;

	public int seedItemID;

	public int growthDays;

	private void Awake()
	{
		currentGrid = FindFirstObjectByType<Grid>(); // Find the Grid component in the scene
	}

	private void OnEnable()
	{
		EventHandler.GenerateCropEvent += GenerateCrop;
	}
	private void OnDisable()
	{
		EventHandler.GenerateCropEvent -= GenerateCrop;
	}

	// Generate a crop on the current grid position
	private void GenerateCrop()
	{
		if (currentGrid == null)
		{
			currentGrid = FindFirstObjectByType<Grid>(); // Reassign currentGrid if null
		}

		Vector3Int cropGridPos = currentGrid.WorldToCell(transform.position); // Get the grid position of the current object

		if (seedItemID != 0)
		{
			var tile = GridMapManager.Instance.GetTileDetailsOnMousePosition(cropGridPos); // Get tile details at the specified grid position

			if (tile == null)
			{
				tile = new TileDetails(); // Create a new TileDetails if none exist
				tile.gridX = cropGridPos.x;
				tile.gridY = cropGridPos.y;
			}

			tile.daysSinceWatered = -1; // Initialize tile details
			tile.seedItemID = seedItemID;
			tile.growthDays = growthDays;

			GridMapManager.Instance.UpdateTileDetails(tile); // Update the tile details in the GridMapManager
		}
	}
}
