using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueController : MonoBehaviour
{
	public UnityEvent OnFinishDialogue;
	public List<Dialogue> dialogueList;

	public bool isTalking=false;
	public bool canShowDialogue=false;
	private Queue<Dialogue> dialogueQueue=new Queue<Dialogue>();
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
		if (canShowDialogue&&Input.GetKeyDown(KeyCode.Space) && !isTalking)
		{
			StartCoroutine(ShowDialogue());
		}
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!nPCAction.isMoving&&collision.gameObject.CompareTag("Player"))
		{
			interactiveButton.SetActive(true);
			canShowDialogue =true;
		}

	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		interactiveButton.SetActive(false);
		nPCAction.canMove = true;
		canShowDialogue =false;

	}

	public IEnumerator ShowDialogue()
	{
		EventHandler.CallGamePaueseEvent(true);
		if (dialogueQueue.Count>0)
		{
			Dialogue dialogue = dialogueQueue.Dequeue();
			EventHandler.CallShowDialogueEvent(dialogue);
			yield return new WaitUntil(() => dialogue.isDone);
			//Debug.Log("isdone");
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

	private void GetDialogDetail(List<Dialogue> dialogueList)
	{
		foreach (var Dialogue in dialogueList)
		{
			dialogueQueue.Enqueue(Dialogue);
		}

	}

	private void ResetDialogueQueue()
	{
		if (dialogueQueue.Count>0)
		{
			dialogueQueue.Clear();
			Debug.Log("dialogueQueueClear");
		}
	
		GetDialogDetail(dialogueList);
	}

}
