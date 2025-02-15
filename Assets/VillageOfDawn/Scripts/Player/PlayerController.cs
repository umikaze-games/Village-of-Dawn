using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.InputSystem;
using UnityEditor;
using System.Collections.Generic;

public class PlayerController : SingletonMonoBehaviour<PlayerController>, ISaveable
{
	[SerializeField]
	private float playerMoveSpeed = 5;

	private PlayerInputActions playerInputAction;
	private Rigidbody2D rb;

	private float inputX;
	private float inputY;
	private bool isRunning;
	private Animator[] animators;

	private float mouseX;
	private float mouseY;
	private bool useTool = false;
	public bool inputDisable = false;

	[SerializeField]
	private bool canMove = true;

	public string GUID => GetComponent<DataGUID>().guid;

	protected override void Awake()
	{
		base.Awake();
		playerInputAction = new PlayerInputActions();

		animators = GetComponentsInChildren<Animator>();
		rb = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		ISaveable saveable = this;
		saveable.RegisterSaveable();
	}

	private void FixedUpdate()
	{
		if (canMove && !inputDisable && !useTool)
		{
			PlayerMove();
		}
		
	}
	void Update()
	{

		SwitchAnimation();
	}

	private void PlayerMove()
	{
		// Read movement input from player
		Vector2 readValue = playerInputAction.PlayerControls.Move.ReadValue<Vector2>();
		inputX = readValue.normalized.x;
		inputY = readValue.normalized.y;
		Vector2 moveDir = new Vector2(inputX, inputY).normalized;

		// If no input, set running state to false and return
		if (readValue == Vector2.zero)
		{
			isRunning = false;
			return;
		}

		// Calculate new position and move the player
		Vector2 newPosition = rb.position + moveDir * playerMoveSpeed * Time.deltaTime;
		rb.MovePosition(newPosition);
		isRunning = true;
	}

	private void OnEnable()
	{
		playerInputAction.Enable();
		EventHandler.MouseClickedEvent += OnMouseClickEvent;
		EventHandler.GamePauseEvent += OnGamePaueseEvent;
		EventHandler.StartNewGameEvent += OnStartNewGameEvent;
		EventHandler.EndGameEvent += OnEndGameEvent;
	}

	private void OnDisable()
	{
		playerInputAction.Disable();
		EventHandler.MouseClickedEvent -= OnMouseClickEvent;
		EventHandler.GamePauseEvent -= OnGamePaueseEvent;
		EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
		EventHandler.EndGameEvent -= OnEndGameEvent;
	}

	private void OnEndGameEvent()
	{
		inputDisable = true;
	}

	private void OnStartNewGameEvent(int obj)
	{
		// Enable input and reset player position when a new game starts
		inputDisable = false;
		transform.position = Settings.playerInitialPosition;
	}

	private void OnGamePaueseEvent(bool gamePause)
	{
		canMove = !gamePause;
	}

	public void SwitchAnimation()
	{
		// Update animation parameters for all animators based on player movement
		foreach (var animator in animators)
		{
			animator.SetFloat("MouseX", mouseX);
			animator.SetFloat("MouseY", mouseY);
			if (isRunning)
			{
				animator.SetBool("isRunning", true);
				animator.SetFloat("InputX", inputX);
				animator.SetFloat("InputY", inputY);
			}
			else
			{
				animator.SetBool("isRunning", false);
			}
		}
	}

	private void OnMouseClickEvent(Vector3 mouseWorldPos, ItemDetails itemDetails)
	{
		if (inputDisable) return;

		// Handle different item types for mouse click
		if (itemDetails.itemType != ItemType.Seed && itemDetails.itemType != ItemType.Product && itemDetails.itemType != ItemType.Furniture)
		{
			inputDisable = true;
			mouseX = mouseWorldPos.x - transform.position.x;
			mouseY = mouseWorldPos.y - (transform.position.y + 0.85f); // 0.85f is a height compensation value for the character

			// Determine which axis has the larger movement
			if (Mathf.Abs(mouseX) > Mathf.Abs(mouseY))
				mouseY = 0;
			else
				mouseX = 0;

			StartCoroutine(UseToolRoutine(mouseWorldPos, itemDetails));
		}
		else
		{
			EventHandler.CallExecuteActionAfterAnimation(mouseWorldPos, itemDetails);
		}
	}

	private IEnumerator UseToolRoutine(Vector3 mouseWorldPos, ItemDetails itemDetails)
	{
		useTool = true;
		inputDisable = true;
		yield return null;

		// Set trigger for using the tool in all animators
		foreach (var anim in animators)
		{
			anim.SetTrigger("UseTool");
			anim.SetFloat("InputX", mouseX);
			anim.SetFloat("InputY", mouseY);
		}

		// Wait for tool usage animation to complete
		yield return new WaitForSeconds(0.6f);
		EventHandler.CallExecuteActionAfterAnimation(mouseWorldPos, itemDetails);
		yield return new WaitForSeconds(0.5f);

		useTool = false;
		inputDisable = false;
	}

	public GameSaveData GenerateSaveData()
	{
		// Create and return save data containing player's position
		GameSaveData gameSaveData = new GameSaveData();
		gameSaveData.characterPosDict = new Dictionary<string, SerializableVector3>();
		gameSaveData.characterPosDict.Add(this.name, new SerializableVector3(transform.position));
		return gameSaveData;
	}

	public void RestoreData(GameSaveData saveData)
	{
		// Restore player's position from save data
		Vector3 targetPosition = saveData.characterPosDict[this.name].ToVector3();
		transform.position = targetPosition;
		inputDisable = false;
	}
}
