using UnityEngine;

public class TransionArea : MonoBehaviour
{
	[SerializeField]
	private string fromSceneName;

	[SerializeField]
	private string toSceneName;

	[SerializeField]
	private Vector3 loadScenePosition;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			EventHandler.CalltransitionEvent(fromSceneName,toSceneName, loadScenePosition);		
		}
	}
}
