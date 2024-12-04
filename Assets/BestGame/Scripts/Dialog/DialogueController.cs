using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueController : MonoBehaviour
{
	public UnityEvent OnFinishDialogue;
	public List<Dialogue> dialogueList;

	public bool isTalking = false;
	public bool canShowDialogue = false;
	private Queue<Dialogue> dialogueQueue = new Queue<Dialogue>();
	private NPCAction nPCAction;
	public GameObject interactiveButton;

	private void Awake()
	{
		nPCAction = GetComponent<NPCAction>();
	}

	private void Start()
	{
		ResetDialogueQueue();
	}

	private void Update()
	{
		// Check if the player can show dialogue and presses the space key
		if (canShowDialogue && Input.GetKeyDown(KeyCode.Space) && !isTalking)
		{
			StartCoroutine(ShowDialogue());
		}
	}

	// Handle the player staying in the dialogue trigger area
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			interactiveButton.SetActive(true);
			canShowDialogue = true;
			nPCAction.canMove = false;
		}
	}

	// Handle the player exiting the dialogue trigger area
	private void OnTriggerExit2D(Collider2D collision)
	{
		interactiveButton.SetActive(false);
		nPCAction.canMove = true;
		canShowDialogue = false;
	}

	// Coroutine to show the dialogue
	public IEnumerator ShowDialogue()
	{
		EventHandler.CallGamePaueseEvent(true);
		if (dialogueQueue.Count > 0)
		{
			Dialogue dialogue = dialogueQueue.Dequeue();
			EventHandler.CallShowDialogueEvent(dialogue);
			yield return new WaitUntil(() => dialogue.isDone);
			isTalking = false;
		}
		else
		{
			isTalking = false;
			EventHandler.CallEndDialogueEvent();
			ResetDialogueQueue();
			EventHandler.CallGamePaueseEvent(false);
			OnFinishDialogue?.Invoke();
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
