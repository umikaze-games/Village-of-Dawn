using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	public static PlayerController instance { get; private set; }
	private PlayerInputActions playerInputAction;
	private void Awake()
	{
		if (instance == null)instance = this;
		else Destroy(this.gameObject);
		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{

	}

	private void OnEnable()
	{
		playerInputAction.PlayerControls.Move.performed += OnMove;
		playerInputAction.Enable();

	}

	private void OnDisable()
	{
		playerInputAction.Disable();
	}

	private void OnMove(InputAction.CallbackContext context)
	{
		
	}
}
