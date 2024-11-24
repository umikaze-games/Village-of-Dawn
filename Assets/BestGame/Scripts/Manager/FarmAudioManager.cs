using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class FarmAudioManager : MonoBehaviour
{
	private AudioSource bGMAudioSource;
	private AudioSource bGSAudioSource;
	private AudioSource playerSEAudioSource;
	private AudioSource cropSEAudioSource;
	private AudioSource toolSEAudioSource;

	public AudioSO bGMDatas;
	public AudioSO bGSDatas;
	public AudioSO playeSEDatas;
	public AudioSO cropSEDatas;
	public AudioSO toolSEDatas;

	public AudioMixer audioMixer;
	private void Awake()
	{
		bGMAudioSource = transform.GetChild(0).GetComponent<AudioSource>();
		bGSAudioSource = transform.GetChild(1).GetComponent<AudioSource>();
		cropSEAudioSource = transform.GetChild(2).GetComponent<AudioSource>();
		toolSEAudioSource = transform.GetChild(3).GetComponent<AudioSource>();
		playerSEAudioSource = transform.GetChild(4).GetComponent<AudioSource>();
	}

	private void OnEnable()
	{
		EventHandler.AfterSceneLoadEvent += PlayBGM;
		EventHandler.AfterSceneLoadEvent += PlayBGS;
		EventHandler.PlaySEEvent += OnPlaySEEvent;
	}

	private void OnDisable()
	{
		EventHandler.AfterSceneLoadEvent -= PlayBGM;
		EventHandler.AfterSceneLoadEvent += PlayBGS;
		EventHandler.PlaySEEvent -= OnPlaySEEvent;
	}

	private void OnPlaySEEvent(string sEName,AudioType audioType)
	{
		string audioMixGropName=null;
		AudioSO sEDatas=new AudioSO();
		AudioSource audioSource=GetSESoure(audioType);
		switch (audioType)
		{
			case AudioType.BGM:
				break;
			case AudioType.BGS:
				break;
			case AudioType.PlayerSE:
				sEDatas = playeSEDatas;
				audioMixGropName = "PlayerSEVolume";
				break;
			case AudioType.CropSE:
				sEDatas = cropSEDatas;
				audioMixGropName = "CropSEVolume";
				break;
			case AudioType.ToolSE:
				sEDatas = toolSEDatas;
				audioMixGropName = "ToolSEVolume";
				break;
			default:
				break;
		}
		for (int i = 0; i < sEDatas.audioDetails.Length; i++)
		{
			if (sEDatas.audioDetails[i].clipName == sEName)
			{
				audioSource.clip = sEDatas.audioDetails[i].audioClip;
				audioSource.volume = sEDatas.audioDetails[i].audioVolume;
			}
		}
		audioMixer.SetFloat(audioMixGropName, audioSource.volume * 100 - 80);
		if (audioSource.isPlaying) return;
		audioSource.Play();
	}

	private void PlayBGM()
	{
		String sceneName=SceneManager.GetActiveScene().name;
		for (int i = 0; i < bGMDatas.audioDetails.Length; i++)
		{
			if (bGMDatas.audioDetails[i].sceneName==sceneName)
			{
				bGMAudioSource.clip = bGMDatas.audioDetails[i].audioClip;
				bGMAudioSource.volume= bGMDatas.audioDetails[i].audioVolume;
			}
		}
		audioMixer.SetFloat("BGMVolume", bGMAudioSource.volume * 100 - 80);
		bGMAudioSource.Play();
	}

	private void PlayBGS()
	{
		String sceneName = SceneManager.GetActiveScene().name;
		for (int i = 0; i < bGSDatas.audioDetails.Length; i++)
		{
			if (bGSDatas.audioDetails[i].sceneName == sceneName)
			{
				bGSAudioSource.clip = bGSDatas.audioDetails[i].audioClip;
				bGSAudioSource.volume = bGSDatas.audioDetails[i].audioVolume;
			}
		}
		audioMixer.SetFloat("BGSVolume", (bGMAudioSource.volume * 100 - 60));
		bGSAudioSource.Play();
	}

	private AudioSource GetSESoure(AudioType audioType)
	{
		AudioSource audioSource;
		switch (audioType)
		{
			case AudioType.BGM:
				audioSource = bGMAudioSource;
				break;
			case AudioType.BGS:
				audioSource = bGSAudioSource;
				break;
			case AudioType.PlayerSE:
				audioSource = playerSEAudioSource;
				break;
			case AudioType.CropSE:
				audioSource = cropSEAudioSource;
				break;
			case AudioType.ToolSE:
				audioSource = toolSEAudioSource;
				break;
			default:
				return null;
				break;
		}
		return audioSource;
	}
}
