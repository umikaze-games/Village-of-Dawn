using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventHandler
{
	public static event Action<InventoryType, List<InventoryItem>> UpdateInventoryUI;

	public static void CallUpdateInventoryUI(InventoryType inventoryType, List<InventoryItem> inventoryItems)
	{
		UpdateInventoryUI?.Invoke(inventoryType, inventoryItems);
	}

	public static event Action<int, Vector3>InstantiateItemInScene;

	public static void CallInstantiateItemInScene(int itemID, Vector3 location)
	{
		InstantiateItemInScene?.Invoke(itemID, location);
	}

	public static event Action<int, Vector3> DropItemInScene;

	public static void CallDropItemInScene(int itemID, Vector3 location)
	{
		InstantiateItemInScene?.Invoke(itemID, location);
	}

	public static event Action<ItemDetails, bool> ItemSelectedEvent;
	public static void CallItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
	{
		ItemSelectedEvent?.Invoke(itemDetails, isSelected);
	}

	public static event Action<string,string,Vector3>TransitionEvent;
	public static void CalltransitionEvent(string fromsceneName, string tosceneName, Vector3 transitionPosition)
	{
		TransitionEvent?.Invoke(fromsceneName, tosceneName, transitionPosition);
	}

	public static event Action BeforeSceneUnloadEvent;
	public static void CallBeforeSceneUnloadEvent()
	{
		BeforeSceneUnloadEvent?.Invoke();
	}

	public static event Action AfterSceneLoadEvent;
	public static void CallAfterSceneLoadEvent()
	{
		AfterSceneLoadEvent?.Invoke();
	}

	public static event Action<Vector3> MoveToPosition;
	public static void CallMoveToPosition(Vector3 targetPosition)
	{
		MoveToPosition?.Invoke(targetPosition);
	}

	public static event Action<Vector3, ItemDetails> MouseClickedEvent;
	public static void CallMouseClickedEvent(Vector3 pos, ItemDetails itemDetails)
	{
		MouseClickedEvent?.Invoke(pos, itemDetails);
	}

	public static event Action<Vector3, ItemDetails> ExecuteActionAfterAnimation;
	public static void CallExecuteActionAfterAnimation(Vector3 pos, ItemDetails itemDetails)
	{
		ExecuteActionAfterAnimation?.Invoke(pos, itemDetails);
	}
}
