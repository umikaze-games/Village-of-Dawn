using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapData_SO", menuName = "Map/Map_SO")]
public class MapData_SO : ScriptableObject
{
    public string SceneName;
    public List<TileProperty> tileProperties;
}
