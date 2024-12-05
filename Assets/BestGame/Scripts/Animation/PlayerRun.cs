using UnityEngine;

public class PlayerRun : MonoBehaviour
{
	public AudioSource audioSource;

	//use in animation
	public void PlayStepAudio()
	{ 
		audioSource.Play();
	}
}
