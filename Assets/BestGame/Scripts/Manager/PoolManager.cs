using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
	public List<GameObject> poolPrefabs;
	public List<ObjectPool<GameObject>> poolEffectList = new List<ObjectPool<GameObject>>();

	private Queue<GameObject> soundQueue = new Queue<GameObject>();

	private void OnEnable()
	{
		EventHandler.ParticleEffectEvent += OnParticalEffectEvent;
	}
	private void OnDisable()
	{
		EventHandler.ParticleEffectEvent -= OnParticalEffectEvent;
	}

	private void Start()
	{
		CreatePool();
	}

	// Create object pools for each prefab in the list
	private void CreatePool()
	{
		foreach (GameObject item in poolPrefabs)
		{
			Transform parent = new GameObject(item.name).transform;
			parent.SetParent(transform);

			var newPool = new ObjectPool<GameObject>(
				() => Instantiate(item, parent),
				e => e.SetActive(true),
				e => e.SetActive(false),
				e => Destroy(e)
			);

			poolEffectList.Add(newPool);
		}
	}

	// Handle the particle effect event to spawn the appropriate effect
	private void OnParticalEffectEvent(ParticleEffectType effectType, Vector3 pos)
	{
		ObjectPool<GameObject> objPool = effectType switch
		{
			ParticleEffectType.LeaveFalling01 => poolEffectList[0],
			ParticleEffectType.Rock => poolEffectList[1],
			ParticleEffectType.ReapableScenery => poolEffectList[2],
			ParticleEffectType.LeaveFalling02 => poolEffectList[3],
			_ => null,
		};
		if (objPool == null) return;

		GameObject obj = objPool.Get();
		obj.transform.position = pos;
		StartCoroutine(ReleaseRoutine(objPool, obj));
	}

	// Release an object back to the pool after a delay
	private IEnumerator ReleaseRoutine(ObjectPool<GameObject> pool, GameObject obj)
	{
		yield return new WaitForSeconds(1.5f);
		pool.Release(obj);
	}

	// Create a pool for sound effect objects
	private void CreateSoundPool()
	{
		var parent = new GameObject(poolPrefabs[4].name).transform;
		parent.SetParent(transform);

		for (int i = 0; i < 20; i++)
		{
			GameObject newObj = Instantiate(poolPrefabs[4], parent);
			newObj.SetActive(false);
			soundQueue.Enqueue(newObj);
		}
	}

	// Get a sound object from the pool
	private GameObject GetPoolObject()
	{
		if (soundQueue.Count < 2)
			CreateSoundPool();
		return soundQueue.Dequeue();
	}

	// Disable a sound object after its duration
	private IEnumerator DisableSound(GameObject obj, float duration)
	{
		yield return new WaitForSeconds(duration);
		obj.SetActive(false);
		soundQueue.Enqueue(obj);
	}
}