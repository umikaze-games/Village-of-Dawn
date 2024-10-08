using UnityEngine;

public class DonotDestroy : MonoBehaviour
{
	private static DonotDestroy instance;
	private void Awake()
	{
		if (instance == null)instance = this;
		else Destroy(this.gameObject);
		DontDestroyOnLoad(this.gameObject);
	}
}
