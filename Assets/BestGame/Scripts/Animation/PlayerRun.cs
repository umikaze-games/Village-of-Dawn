using UnityEngine;

public class PlayerRun : MonoBehaviour
{
	public AudioSource audioSource;

	public void PlayStepAudio()
	{ 
		audioSource.Play();
	}
}
