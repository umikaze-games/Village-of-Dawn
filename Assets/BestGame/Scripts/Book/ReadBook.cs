using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ReadBook : MonoBehaviour
{
	[SerializeField]
	private int index = 0;
	[SerializeField]
	private float rotateSpeed = 10f;
	[SerializeField]
	private List<Transform> bookPages;
	public void PageDown()
	{
		StartCoroutine(RotatePage(true));
	}
	public void PageUp()
	{
		StartCoroutine(RotatePage(false));
	}

	private IEnumerator RotatePage(bool nextPage)
	{

		if (nextPage == true&& index == 3 || nextPage == false&& index == 0) yield break;

		float rotateY;
		if (nextPage == true) rotateY = 180;
		else rotateY = 0;

		Quaternion targeQuaternion = Quaternion.Euler(0, rotateY, 0);

		EventHandler.CallPlaySEEvent("Page", AudioType.PlayerSE);

		while (Quaternion.Angle(bookPages[index].rotation, targeQuaternion) > 0.1f)
		{
			bookPages[index].rotation = Quaternion.Slerp(
			bookPages[index].rotation,
			targeQuaternion,
			rotateSpeed
			);

		}

		for (int i = 0; i < bookPages.Count; i++)
		{
			if (nextPage==true && i == index+1||nextPage==false&&i==index-1)
			{
				bookPages[i].transform.gameObject.SetActive(true);
				bookPages[i].rotation =Quaternion.identity;
			}
			else
			{
				bookPages[i].transform.gameObject.SetActive(false);
			}
		}

		if (nextPage == true) index++;
		else if (nextPage == false) index--;

		yield return null;
	}
}
