using System;
using UnityEngine;

[Serializable]
public class AudioDetails
{
	public string sceneName;
	public string clipName;
	public AudioClip audioClip;

	[Range(0,1)]
	public float audioVolume;
	public AudioType audioType;
}
