using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SystemManager : MonoBehaviour
{
	public static SystemManager instance;
	private void Awake()
	{
		if (instance == null) instance = this;
		else Destroy(this.gameObject);
		DontDestroyOnLoad(gameObject);
	}
	public void CancelQuit()
	{
		UIManager.instance.CloseConfirmDialogUI();
	}
	public void QuitGame()
	{
		Application.Quit();
	}
}
