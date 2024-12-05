using UnityEngine;

public class TriggerFader : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Get all ItemFader components in the children of the collided object
		ItemFader[] faders = collision.GetComponentsInChildren<ItemFader>();
		if (faders != null)
		{
			// Iterate through each fader and trigger the FadeOut method
			foreach (var target in faders)
			{
				target.FadeOut();
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		// Get all ItemFader components in the children of the collided object
		ItemFader[] faders = collision.GetComponentsInChildren<ItemFader>();
		if (faders != null)
		{
			// Iterate through each fader and trigger the FadeIn method
			foreach (var target in faders)
			{
				target.FadeIn();
			}
		}
	}
}
