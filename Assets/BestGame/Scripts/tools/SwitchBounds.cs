using Unity.Cinemachine;
using UnityEngine;

public class SwitchBounds : MonoBehaviour
{
	private void Start()
	{

	}

	private void OnEnable()
	{
		EventHandler.AfterSceneLoadEvent += SwitchConfinerShape;
	}

	private void OnDisable()
	{
		EventHandler.AfterSceneLoadEvent -= SwitchConfinerShape;
	}
	private void SwitchConfinerShape()
	{
		PolygonCollider2D ConfinerShape2D = GameObject.FindGameObjectWithTag("BoundConfiner").GetComponent<PolygonCollider2D>();
		CinemachineConfiner2D confiner2D = GetComponent<CinemachineConfiner2D>();
		confiner2D.BoundingShape2D = ConfinerShape2D;
	}
}