using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueController : MonoBehaviour
{
	public UnityEvent OnFinishDialogue;
	public List<Dialogue> dialogueList;

	private bool isTalking=false;
	private bool canShowDialogue=false;
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
		if (canShowDialogue&&Input.GetKeyDown(KeyCode.Space)&&!isTalking)
		{
			StartCoroutine(ShowDialogue());
		}
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!nPCAction.isMoving&&collision.gameObject.CompareTag("Player"))
		{
			interactiveButton.SetActive(true);
			canShowDialogue=true;
		}

	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		interactiveButton.SetActive(false);
		canShowDialogue=false;

	}

	private IEnumerator ShowDialogue()
	{
		if (dialogueQueue.Count>0)
		{
			Dialogue dialogue = dialogueQueue.Dequeue();
			EventHandler.CallShowDialogueEvent(dialogue);
			Debug.Log("CallShowDialogueEvent");
			yield return new WaitUntil(() => dialogue.isDone);
			Debug.Log("isdone");
			isTalking = false;
		}
		else
		{
			isTalking = false;
			ResetDialogueQueue();
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
