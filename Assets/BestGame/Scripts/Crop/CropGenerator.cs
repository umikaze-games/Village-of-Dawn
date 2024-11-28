using UnityEngine;

public class CropGenerator : MonoBehaviour
{
		[SerializeField]
		private Grid currentGrid;

		public int seedItemID;

		public int growthDays;

		private void Awake()
		{
			currentGrid = FindFirstObjectByType<Grid>();
		}

		private void OnEnable()
		{
			EventHandler.GenerateCropEvent += GenerateCrop;
		}
		private void OnDisable()
		{
			EventHandler.GenerateCropEvent -= GenerateCrop;
		}

		private void GenerateCrop()
		{
			if (currentGrid == null)
			{
				currentGrid = FindFirstObjectByType<Grid>();
			}
		
			Vector3Int cropGridPos = currentGrid.WorldToCell(transform.position);

			if (seedItemID != 0)
			{
				var tile = GridMapManager.Instance.GetTileDetailsOnMousePosition(cropGridPos);

				if (tile == null)
				{
					tile = new TileDetails();
					tile.gridX = cropGridPos.x;
					tile.gridY = cropGridPos.y;
				}

				tile.daysSinceWatered = -1;
				tile.seedItemID = seedItemID;
				tile.growthDays = growthDays;

				GridMapManager.Instance.UpdateTileDetails(tile);
			}
		}

}
