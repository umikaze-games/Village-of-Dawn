using UnityEngine;

public class ItemShadow : MonoBehaviour
{
	public SpriteRenderer itemSprite;
	private SpriteRenderer shadowSprite;

	private void Awake()
	{
		shadowSprite=gameObject.GetComponent<SpriteRenderer>();
	}

	private void Start()
	{
		shadowSprite.sprite=itemSprite.sprite;
		shadowSprite.color = new (0, 0, 0, 0.5f);
	}
}
