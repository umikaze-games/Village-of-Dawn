using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
	public GameObject dialoguePanel;
	public TextMeshProUGUI dialogueText;
	public GameObject nPCFaceGameObject;
	public GameObject playerFaceGameObject;
	public TextMeshProUGUI playerFaceText;
	public TextMeshProUGUI nPCFaceText;
	public GameObject continuneBox;
	private Image dialogueLeftImage;
	private Image dialogueRightImage;
	public GameObject dialogueLeftName;
	public GameObject dialoguerRightName;
	private void Awake()
	{
		continuneBox.gameObject.SetActive(false);
	}

	private void Update()
	{

	}
	private void OnEnable()
	{
		EventHandler.ShowDialogueEvent += OnShowDialogueEvent;
		EventHandler.EndDialogueEvent += OnEndDialogueEvent;
	}

	
	private void OnDisable()
	{
		EventHandler.ShowDialogueEvent -= OnShowDialogueEvent;
		EventHandler.EndDialogueEvent -= OnEndDialogueEvent;
	}

	private void OnEndDialogueEvent()
	{
		dialogueLeftName.SetActive(false);
		dialoguerRightName.SetActive(false);
		dialoguePanel.SetActive(false);
		dialogueLeftImage.enabled = false;
		dialogueRightImage.enabled = false;
	}

	private void OnShowDialogueEvent(Dialogue dialogue)
	{
		dialoguePanel.SetActive(true);
		if (dialogue.onLeft)
		{
			UpdateLeftDialogue(dialogue);

		}
		else
		{
			UpdateRightDialogue(dialogue);
		}

		dialogueText.text = dialogue.dialogueText;

		//if (dialogue.needSpaceContinue)
		//{
		//	continuneBox.SetActive(true);
		//}
		dialogue.isDone = true;
		
	}

	private void UpdateLeftDialogue(Dialogue dialogue) 
	{
		dialogueLeftImage = nPCFaceGameObject.GetComponentInChildren<Image>();
		dialogueLeftImage.sprite = dialogue.faceImage;
		dialogueLeftName.SetActive(true);
		dialogueLeftName.GetComponentInChildren<TextMeshProUGUI>().text = dialogue.characterName;
		dialogueLeftImage.enabled = true;

	}

	private void UpdateRightDialogue(Dialogue dialogue)
	{
		dialogueRightImage = playerFaceGameObject.GetComponent<Image>();
		dialogueRightImage.sprite = dialogue.faceImage;
		dialoguerRightName.SetActive(true);
		dialoguerRightName.GetComponentInChildren<TextMeshProUGUI>().text = dialogue.characterName;
		dialogueRightImage.enabled = true;
	}
}
