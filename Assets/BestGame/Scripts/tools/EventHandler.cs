using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventHandler
{
	public static event Action<InventoryLocation, List<InventoryItem>> UpdateInventoryUI;
	public static void CallUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
	{
		UpdateInventoryUI?.Invoke(location, list);
	}

	public static event Action<int, Vector3> InstantiateItemInScene;
	public static void CallInstantiateItemInScene(int ID, Vector3 pos)
	{
		InstantiateItemInScene?.Invoke(ID, pos);
	}

	public static event Action<int, Vector3, ItemType> DropItemEvent;
	public static void CallDropItemEvent(int ID, Vector3 pos, ItemType itemType)
	{
		DropItemEvent?.Invoke(ID, pos, itemType);
	}

	public static event Action<ItemDetails, bool> ItemSelectedEvent;
	public static void CallItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
	{
		ItemSelectedEvent?.Invoke(itemDetails, isSelected);
	}

	public static event Action<int, int, int> GameMinuteEvent;
	public static void CallGameMinuteEvent(int minute, int hour, int day)
	{
		GameMinuteEvent?.Invoke(minute, hour, day);
	}

	public static event Action<int, int, int, int> GameDataEvent;
	public static void CallGameDataEvent(int hour, int day, int month, int year)
	{
		GameDataEvent?.Invoke(hour, day, month, year);
	}

	public static event Action<string, string, Vector3> TransitionEvent;
	public static void CallTransitionEvent(string fromsceneName, string tosceneName, Vector3 transitionPosition)
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

	public static event Action<int, int> GameDayEvent;
	public static void CallGameDayEvent(int day, int season)
	{
		GameDayEvent?.Invoke(day, season);
	}

	public static event Action<int, TileDetails > PlantSeedEvent;
	public static void CallPlantSeedEvent(int ID, TileDetails tile)
	{ 
		PlantSeedEvent?.Invoke(ID, tile);
	
	}

	public static event Action<int> HarvestAtPlayerPosition;
	public static void CallHarvestAtPlayerPosition(int ID)
	{
		HarvestAtPlayerPosition?.Invoke(ID);
	}

	public static event Action RefreshCurrentMap;
	public static void CallRefreshCurrentMap()
	{
		RefreshCurrentMap?.Invoke();
	}

	public static event Action<ParticleEffectType, Vector3> ParticleEffectEvent;
	public static void CallParticleEffectEvent(ParticleEffectType effectType, Vector3 pos)
	{
		ParticleEffectEvent?.Invoke(effectType, pos);
	}

	public static event Action GenerateCropEvent;
	public static void CallGenerateCropEvent()
	{
		GenerateCropEvent?.Invoke(); 
	}
}

