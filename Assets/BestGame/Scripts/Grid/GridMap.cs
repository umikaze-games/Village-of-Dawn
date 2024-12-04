using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

[ExecuteInEditMode]
public class GridMap : MonoBehaviour
{
	public MapData_SO mapData;
	public GridType gridType;
	private Tilemap currentTilemap;

	private void OnEnable()
	{
		if (!Application.IsPlaying(this))
		{
			currentTilemap = GetComponent<Tilemap>();

			if (mapData != null)
			{
				mapData.tileProperties.Clear(); // Clear existing tile properties when enabled in edit mode
			}
		}
	}

	private void OnDisable()
	{
		if (!Application.IsPlaying(this))
		{
			currentTilemap = GetComponent<Tilemap>();

			UpdateTileProperties(); // Update the tile properties when disabling in edit mode

#if UNITY_EDITOR
			if (mapData != null)
			{
				EditorUtility.SetDirty(mapData); // Mark the map data as dirty to save changes
			}
#endif
		}
	}

	// Updates the properties of all tiles in the current tilemap
	private void UpdateTileProperties()
	{
		currentTilemap.CompressBounds(); // Compress bounds to remove empty space

		if (!Application.IsPlaying(this))
		{
			if (mapData != null)
			{
				Vector3Int startPos = currentTilemap.cellBounds.min;
				Vector3 endPos = currentTilemap.cellBounds.max;

				// Iterate through all the tiles in the tilemap bounds
				for (int x = startPos.x; x < endPos.x; x++)
				{
					for (int y = startPos.y; y < endPos.y; y++)
					{
						TileBase tile = currentTilemap.GetTile(new Vector3Int(x, y, 0));

						if (tile != null)
						{
							TileProperty newTile = new TileProperty
							{
								tileCoordinate = new Vector2Int(x, y),
								gridType = this.gridType,
								boolTypeValue = true
							};

							mapData.tileProperties.Add(newTile); // Add new tile property to map data
						}
					}
				}
			}
		}
	}
}
