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
		continuneBox.gameObject.SetActive(false); // Hide the continue box on awake
	}

	private void Update()
	{
		// Empty update method, can be used for future logic if needed
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

	// Handles ending the dialogue
	private void OnEndDialogueEvent()
	{
		dialogueLeftName.SetActive(false);
		dialoguerRightName.SetActive(false);
		dialoguePanel.SetActive(false);
		dialogueLeftImage.enabled = false;
		dialogueRightImage.enabled = false;
	}

	// Handles showing the dialogue
	private void OnShowDialogueEvent(Dialogue dialogue)
	{
		dialoguePanel.SetActive(true);
		if (dialogue.onLeft)
		{
			UpdateLeftDialogue(dialogue); // Update the dialogue UI for NPC (left side)
		}
		else
		{
			UpdateRightDialogue(dialogue); // Update the dialogue UI for player (right side)
		}

		dialogueText.text = dialogue.dialogueText;

		// Uncomment if need to show continue prompt
		// if (dialogue.needSpaceContinue)
		// {
		//     continuneBox.SetActive(true);
		// }

		dialogue.isDone = true; // Mark the dialogue as done
	}

	// Updates the left side dialogue (NPC side)
	private void UpdateLeftDialogue(Dialogue dialogue)
	{
		dialogueLeftImage = nPCFaceGameObject.GetComponentInChildren<Image>();
		dialogueLeftImage.sprite = dialogue.faceImage;
		dialogueLeftName.SetActive(true);
		dialogueLeftName.GetComponentInChildren<TextMeshProUGUI>().text = dialogue.characterName;
		dialogueLeftImage.enabled = true;
	}

	// Updates the right side dialogue (Player side)
	private void UpdateRightDialogue(Dialogue dialogue)
	{
		dialogueRightImage = playerFaceGameObject.GetComponent<Image>();
		dialogueRightImage.sprite = dialogue.faceImage;
		dialoguerRightName.SetActive(true);
		dialoguerRightName.GetComponentInChildren<TextMeshProUGUI>().text = dialogue.characterName;
		dialogueRightImage.enabled = true;
	}
}
