using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
	private static T instance;

	public static T Instance
	{
		get
		{
			return instance;
		}
	}

	protected virtual void Awake()
	{
		if (instance==null)
		{
			instance = this as T;
		}
		else
		{
			Destroy(this.gameObject);
		}
	}

}
