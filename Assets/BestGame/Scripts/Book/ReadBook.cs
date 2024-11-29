using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadBook : MonoBehaviour
{
	[SerializeField]
	private int index = 0;
	[SerializeField]
	private float rotateSpeed = 0.01f;
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
		float rotateY;
		if (nextPage == true) rotateY = 180;
		else rotateY = 0;

		Quaternion targeQuaternion = Quaternion.Euler(0, rotateY, 0);

		while (Quaternion.Angle(bookPages[index].rotation, targeQuaternion) > 0.1f)
		{
			bookPages[index].rotation = Quaternion.Lerp(bookPages[index].transform.rotation, targeQuaternion, rotateSpeed);
		}

		foreach (Transform bookpage in bookPages)
		{
			if (bookpage == bookPages[index + 1])
			{
				bookPages[index].transform.GetChild(1).gameObject.SetActive(false);
			}
			else
			{
				bookPages[index].transform.GetChild(1).gameObject.SetActive(true);
			}
		}

		if (nextPage == true && this.index < bookPages.Count - 1) this.index++;
		else if (nextPage == false && this.index > 1) this.index--;

		yield return null;
	}
}
