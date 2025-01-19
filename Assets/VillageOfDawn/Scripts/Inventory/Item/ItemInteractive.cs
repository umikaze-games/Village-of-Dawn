using System.Collections;
using UnityEngine;

public class ItemInteractive : MonoBehaviour
{
	private bool isAnimating;
	private WaitForSeconds pause = new WaitForSeconds(0.04f);

	// Handle player entering the interaction area of the item
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!isAnimating)
		{
			// Determine direction of player to start appropriate rotation
			if (collision.transform.position.x < transform.position.x)
			{
				StartCoroutine(RotateRight()); // Player is on the left, rotate right
			}
			else
			{
				StartCoroutine(RotateLeft()); // Player is on the right, rotate left
			}

			if (collision.gameObject.CompareTag("Player"))
			{
				EventHandler.CallPlaySEEvent("Rustle", AudioType.PlayerSE); // Play rustling sound
			}
		}
	}

	// Handle player exiting the interaction area of the item
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (!isAnimating)
		{
			// Determine direction of player to start appropriate rotation
			if (collision.transform.position.x > transform.position.x)
			{
				StartCoroutine(RotateRight()); // Player is on the right, rotate right
			}
			else
			{
				StartCoroutine(RotateLeft()); // Player is on the left, rotate left
			}

			if (collision.gameObject.CompareTag("Player"))
			{
				EventHandler.CallPlaySEEvent("Rustle", AudioType.PlayerSE); // Play rustling sound
			}
		}
	}

	// Coroutine to rotate the item to the left
	private IEnumerator RotateLeft()
	{
		isAnimating = true;

		for (int i = 0; i < 4; i++)
		{
			transform.GetChild(0).Rotate(0, 0, 2);
			yield return pause;
		}
		for (int i = 0; i < 5; i++)
		{
			transform.GetChild(0).Rotate(0, 0, -2);
			yield return pause;
		}
		transform.GetChild(0).Rotate(0, 0, 2);
		yield return pause;

		isAnimating = false;
	}

	// Coroutine to rotate the item to the right
	private IEnumerator RotateRight()
	{
		isAnimating = true;

		for (int i = 0; i < 4; i++)
		{
			transform.GetChild(0).Rotate(0, 0, -2);
			yield return pause;
		}
		for (int i = 0; i < 5; i++)
		{
			transform.GetChild(0).Rotate(0, 0, 2);
			yield return pause;
		}
		transform.GetChild(0).Rotate(0, 0, -2);
		yield return pause;

		isAnimating = false;
	}
}
