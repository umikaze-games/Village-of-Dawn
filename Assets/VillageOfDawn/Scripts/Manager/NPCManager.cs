using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCManager : SingletonMonoBehaviour<NPCManager>
{
	//public SceneRouteDataList_SO sceneRouteData;

	//public List<NPCPosition> nPCPositionList;

	//private Dictionary<string, SceneRoute> sceneRouteDict = new Dictionary<string, SceneRoute>();

	public NPCAction[] nPCActions;
	protected override void Awake()
	{
		base.Awake();

		//InitSceneRouteDict();

	}
	private void Start()
	{

	}
	//private void OnEnable()
	//{
	//	EventHandler.StartNewGameEvent += OnStartNewGameEvent;
	//}
	//private void OnDisable()
	//{
	//	EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
	//}

	//private void OnStartNewGameEvent(int obj)
	//{
	//	foreach (var character in nPCPositionList)
	//	{
	//		character.npc.position = character.position;
	//		character.npc.GetComponent<NPCMovement>().StartScene = character.startScene;
	//	}
	//}

	//private void InitSceneRouteDict()
	//{
	//	if (sceneRouteData.sceneRouteList.Count > 0)
	//	{
	//		foreach (SceneRoute route in sceneRouteData.sceneRouteList)
	//		{
	//			var key = route.fromSceneName + route.gotoSceneName;

	//			if (sceneRouteDict.ContainsKey(key))
	//				continue;
	//			else
	//				sceneRouteDict.Add(key, route);
	//		}
	//	}
	//}
	//public SceneRoute GetSceneRoute(string fromSceneName, string gotoSceneName)
	//{
	//	return sceneRouteDict[fromSceneName + gotoSceneName];
	//}

	public void InitSceneNPCInScene()
	{
		foreach (var npc in nPCActions)
		{
			if (npc.nPCInScene == SceneManager.GetActiveScene().name)
			{
				npc.SetNPCVisable();
			
			}

			else
			{
				npc.SetNPCInVisable();
			}
		
		}
	}
}