using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

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
	void Update()
	{

		PlayerMove();
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
	}

	private void OnDisable()
	{
		playerInputAction.Disable();
		EventHandler.MouseClickedEvent += OnMouseClickEvent;
	}

	public void SwitchAnimation()
	{
		foreach (var animator in animators)
		{
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
	private void OnMouseClickEvent(Vector3 mouseWorldPos, ItemDetails itemdetails)
	{
		//TODO animation
		Debug.Log("OnMouseClickEvent");
		EventHandler.CallExecuteActionAfterAnimation(mouseWorldPos, itemdetails);
	}


}
