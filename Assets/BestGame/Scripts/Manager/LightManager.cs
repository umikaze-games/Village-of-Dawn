using UnityEngine;

public class LightManager : MonoBehaviour
{
	private LightController[] lightControllers;
	private LightType lightType;

	private void OnEnable()
	{
		EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
	}

	private void OnDisable()
	{
		EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
	}

	private void OnAfterSceneLoadEvent()
	{
		throw new System.NotImplementedException();
	}
}
