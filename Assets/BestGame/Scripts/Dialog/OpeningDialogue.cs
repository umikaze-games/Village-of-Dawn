using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OpeningDialogue : MonoBehaviour
{
	public List<Dialogue> dialogueList;
	public GameObject interactiveButton;
	public bool canShowDialogue = false;
	private Queue<Dialogue> dialogueQueue = new Queue<Dialogue>();

	private void OnEnable()
	{
		EventHandler.StartNewGameEvent += OnStartNewGameEvent;
	}

	private void OnDisable()
	{
		EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
	}

	private void Start()
	{
		ResetDialogueQueue();
	}

	private void Update()
	{
		// Check if player can show dialogue and presses the space key
		if (canShowDialogue && Input.GetKeyDown(KeyCode.Space))
		{
			StartCoroutine(ShowDialogue());
		}
	}

	// Handles the start of a new game event to trigger the dialogue
	private void OnStartNewGameEvent(int obj)
	{
		StartCoroutine(ShowDialogue());
	}

	// Coroutine to show the dialogue sequence
	public IEnumerator ShowDialogue()
	{
		EventHandler.CallGamePaueseEvent(true);
		if (dialogueQueue.Count > 0)
		{
			interactiveButton.gameObject.SetActive(true);
			canShowDialogue = true;
			Dialogue dialogue = dialogueQueue.Dequeue();
			EventHandler.CallShowDialogueEvent(dialogue);
			yield return new WaitUntil(() => dialogue.isDone);
		}
		else
		{
			canShowDialogue = false;
			EventHandler.CallEndDialogueEvent();
			EventHandler.CallGamePaueseEvent(false);
			interactiveButton.gameObject.SetActive(false);
		}
	}

	// Enqueue all dialogues from the given list
	private void GetDialogDetail(List<Dialogue> dialogueList)
	{
		foreach (var Dialogue in dialogueList)
		{
			dialogueQueue.Enqueue(Dialogue);
		}
	}

	// Reset the dialogue queue
	private void ResetDialogueQueue()
	{
		if (dialogueQueue.Count > 0)
		{
			dialogueQueue.Clear();
			Debug.Log("dialogueQueueClear");
		}

		GetDialogDetail(dialogueList);
	}
}
