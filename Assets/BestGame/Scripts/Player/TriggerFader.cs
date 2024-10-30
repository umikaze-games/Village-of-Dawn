using UnityEngine;

public class TriggerFader : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		ItemFader[] faders = collision.GetComponentsInChildren<ItemFader>();
		if (faders!=null)
		{
			foreach (var target in faders)
			{ 
				target.FadeOut();
			
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		ItemFader[] faders = collision.GetComponentsInChildren<ItemFader>();
		if (faders != null)
		{
			foreach (var target in faders)
			{
				target.FadeIn();

			}
		}
	}
}
