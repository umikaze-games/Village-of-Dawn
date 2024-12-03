using UnityEngine;
using UnityEngine.UI;

public class Bed : MonoBehaviour
{
	public GameObject sleepButtonUI;
	private bool canInteractive=false;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			canInteractive=true;
			sleepButtonUI.gameObject.SetActive(true);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		canInteractive=false;
		sleepButtonUI.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (canInteractive && Input.GetMouseButtonDown(1))
		{
			EventHandler.CallNewDayEvent();
		}
	}
}
