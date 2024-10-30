using System;
using UnityEngine;

[Serializable]
public class ItemDetails
{
	public int itemID;
	public string itemName;
	public ItemType itemType;
	public string itemDescription;
	public int itemUseRadius;
	public Sprite itemIcon;
	public Sprite itemOnWorldSprite;
	public bool canPickedup;
	public bool canDropped;
	public bool canCarried;
	public int itemPrice;
	
	[Range(0,1)]
	public float sellPercentage;
	
}
