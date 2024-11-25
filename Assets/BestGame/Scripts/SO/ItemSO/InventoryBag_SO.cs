using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryBag_SO", menuName = "Inventory/InventoryBag_SO")]
public class InventoryBag_SO : ScriptableObject
{
    public List<InventoryItem> inventoryItems;

    public InventoryItem GetInventoryItem(int ID)
    {
        return inventoryItems.Find(i => i.itemID == ID);
    }
}
 