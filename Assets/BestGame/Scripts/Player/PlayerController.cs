using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class PlayerController : SingletonMonoBehaviour<PlayerController>
{
	[SerializeField]
	private float playerMoveSpeed=50;

	private PlayerInputActions playerInputAction;
	private Rigidbody2D rb;
	protected override void Awake()
	{
		base.Awake();
		playerInputAction = new PlayerInputActions();
		rb=GetComponent<Rigidbody2D>();
	}
	
	void Update()
	{
	
		PlayerMove();
	
	}
	private void PlayerMove()
	{
		Vector2 moveValue = playerInputAction.PlayerControls.Move.ReadValue<Vector2>();
		Vector2 newPosition = rb.position + moveValue.normalized * playerMoveSpeed * Time.deltaTime;
		rb.MovePosition(newPosition);
	}

	private void OnEnable()
	{
		playerInputAction.Enable();
	}

	private void OnDisable()
	{
		playerInputAction.Disable();
	}

}
