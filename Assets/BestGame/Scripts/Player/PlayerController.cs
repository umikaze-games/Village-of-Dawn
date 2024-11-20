using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.InputSystem;

public class PlayerController : SingletonMonoBehaviour<PlayerController>
{
	[SerializeField]
	private float playerMoveSpeed = 50;

	private PlayerInputActions playerInputAction;
	private Rigidbody2D rb;

	private float inputX;
	private float inputY;
	private bool isRunning;
	private Animator[] animators;

	private float mouseX;
	private float mouseY;
	private bool useTool;
	private bool inputDisable;
	private bool canMove=true;

	protected override void Awake()
	{
		base.Awake();
		playerInputAction = new PlayerInputActions();

		animators = GetComponentsInChildren<Animator>();
		rb = GetComponent<Rigidbody2D>();
	}
	private void Start()
	{

	}
	private void FixedUpdate()
	{
		if (canMove&&!inputDisable)
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
		Vector2 readValue = playerInputAction.PlayerControls.Move.ReadValue<Vector2>();
		inputX = readValue.normalized.x;
		inputY = readValue.normalized.y;
		Vector2 moveDir = new Vector2(inputX, inputY);
		if (readValue == Vector2.zero)
		{
			isRunning = false;
			return;
		}
		Vector2 newPosition = rb.position + moveDir * playerMoveSpeed * Time.deltaTime;
		rb.MovePosition(newPosition);
		isRunning = true;
	}

	private void OnEnable()
	{
		playerInputAction.Enable();
		EventHandler.MouseClickedEvent += OnMouseClickEvent;
		EventHandler.GamePauseEvent += OnGamePaueseEvent;
	}

	private void OnDisable()
	{
		playerInputAction.Disable();
		EventHandler.MouseClickedEvent -= OnMouseClickEvent;
		EventHandler.GamePauseEvent -= OnGamePaueseEvent;
	}

	private void OnGamePaueseEvent(bool gamePause)
	{
		canMove=!gamePause;
	}

	public void SwitchAnimation()
	{
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
		if (itemDetails.itemType != ItemType.Seed && itemDetails.itemType != ItemType.Product && itemDetails.itemType != ItemType.Furniture)
		{
			mouseX = mouseWorldPos.x - transform.position.x;
			mouseY = mouseWorldPos.y - (transform.position.y + 0.85f);// 0.85f is a height compensation value for the character

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
		foreach (var anim in animators)
		{
			anim.SetTrigger("UseTool");
			anim.SetFloat("InputX", mouseX);
			anim.SetFloat("InputY", mouseY);
		}
		yield return new WaitForSeconds(0.45f);
		EventHandler.CallExecuteActionAfterAnimation(mouseWorldPos, itemDetails);
		yield return new WaitForSeconds(0.25f);
	
		useTool = false;
		inputDisable = false;
	}


}
