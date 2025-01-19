using UnityEngine;
using DG.Tweening;
public class ItemFader : MonoBehaviour
{
	private SpriteRenderer spriteRenderer;
	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	public void FadeIn()
	{
		Color targetColor = new Color(1, 1, 1, 1);
		spriteRenderer.DOColor(targetColor, Settings.fadeDuration);

	}
	public void FadeOut()
	{
		Color targetColor = new Color(1, 1, 1, Settings.fadeAlpha);
		spriteRenderer.DOColor(targetColor, Settings.fadeDuration);

	}
}
