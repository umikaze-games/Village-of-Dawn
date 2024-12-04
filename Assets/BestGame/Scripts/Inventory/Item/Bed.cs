using UnityEngine;
using UnityEngine.UI;

public class Bed : MonoBehaviour
{
	public GameObject sleepButtonUI;
	private bool canInteractive = false;

	// Handles player entering the bed interaction area
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			canInteractive = true;
			sleepButtonUI.gameObject.SetActive(true); // Show the sleep button UI
		}
	}

	// Handles player exiting the bed interaction area
	private void OnTriggerExit2D(Collider2D collision)
	{
		canInteractive = false;
		sleepButtonUI.gameObject.SetActive(false); // Hide the sleep button UI
	}

	private void Update()
	{
		// Call new day event when the player interacts with the bed
		if (canInteractive && Input.GetMouseButtonDown(1))
		{
			EventHandler.CallNewDayEvent();
		}
	}
}
