using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
	public GameObject dialoguePanel;
	public TextMeshProUGUI dialogueText;
	public Image nPCFaceImage;
	public Image playerFaceImage;
	public TextMeshProUGUI playerFaceText;
	public TextMeshProUGUI nPCFaceText;
	public GameObject continuneBox;

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
	}

	
	private void OnDisable()
	{
		EventHandler.ShowDialogueEvent -= OnShowDialogueEvent;
	}
	private void OnShowDialogueEvent(Dialogue dialogue)
	{
		dialoguePanel.SetActive(true);
		if (dialogue.onLeft)
		{
			nPCFaceImage.sprite = dialogue.faceImage;

		}
		else
		{ 
			playerFaceImage.sprite = dialogue.faceImage;
		}
		dialogueText.text = dialogue.dialogueText;
		if (dialogue.needSpaceContinue)
		{
			continuneBox.SetActive(true);
		}
		dialogue.isDone = true;
		
	}

}
