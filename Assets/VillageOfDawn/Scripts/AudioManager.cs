using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
	[SerializeField] private AudioMixer audioMixer;
	[SerializeField] private AudioSource musicSource;
	[SerializeField] private AudioSource sfxSource;
	[SerializeField] private Button bgmDecreaseVolumeBtm;
	[SerializeField] private Button bgmIncreaseVolumeBtm;
	[SerializeField] private Button sfxDecreaseVolumeBtm;
	[SerializeField] private Button sfxIncreaseVolumeBtm;

	[SerializeField] private TextMeshProUGUI musicVolumeText;
	[SerializeField] private TextMeshProUGUI sfxVolumeText;
	public static AudioManager instance;

	private float audioVolume = 5;
	private float musicVolume=5;
	private float sfxVolume=5;
	private void Awake()
	{
		if (instance == null) instance = this;
		else Destroy(this.gameObject);
		DontDestroyOnLoad(this.gameObject);
		bgmDecreaseVolumeBtm.onClick.AddListener(DecreaseMusicVolume);
		bgmIncreaseVolumeBtm.onClick.AddListener(IncreaseMusicVolume);
		sfxDecreaseVolumeBtm.onClick.AddListener(DecreaseSFXVolume);
		sfxIncreaseVolumeBtm.onClick.AddListener(IncreaseSFXVolume);

	}

	private void Start()
	{
	
		ResetAudio();
	}
	public void PlayBackGroundMusic(AudioClip audioClip )
	{
		musicSource.clip = audioClip;
		musicSource.loop = true;
		musicSource.Play();
	}

	public void ResetAudio()
	{
		setAudioVolume(5);
		setMusicVolume(5);
		setSFXVolume(5);
	}
	public void setAudioVolume(float volume)
	{
		audioVolume = volume;
		//float adjustedVolume = Mathf.Sqrt(volume / 10);
		audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume / 10) * 20);
	
	}

	public void setMusicVolume(float volume)
	{
		musicVolume = volume;
		musicVolumeText.text= musicVolume.ToString();
		//float adjustedVolume = Mathf.Sqrt(volume / 10);
		audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume / 10) * 20);
	}

	public void setSFXVolume(float volume)
	{
		sfxVolume = volume;
		sfxVolumeText.text= sfxVolume.ToString();
		//float adjustedVolume = Mathf.Sqrt(volume / 10);
		audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume / 10) * 20);

	}

	public void IncreaseMusicVolume()
	{
		float volume = ++musicVolume;
		if (musicVolume > 10)
		{
			musicVolume = 10;
		}
		else setMusicVolume(volume);
	}
	public void DecreaseMusicVolume()
	{
		float volume = --musicVolume;
		if (musicVolume <0.1)
		{
			musicVolume = 0;
			musicVolumeText.text = "0";
			audioMixer.SetFloat("MusicVolume", -80);
		}
		else setMusicVolume(volume);
	}

	private void IncreaseSFXVolume()
	{
		float volume = ++sfxVolume;
		if (sfxVolume > 10)
		{
			sfxVolume = 10;
		}
		else setSFXVolume(volume);
	}

	private void DecreaseSFXVolume()
	{
		float volume = --sfxVolume;
		if (sfxVolume < 0.1)
		{
			sfxVolume = 0;
			sfxVolumeText.text = "0";
			audioMixer.SetFloat("SFXVolume", -80);
		}
		else setSFXVolume(volume);

	}

}
