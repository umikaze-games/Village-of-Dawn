using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCAction : MonoBehaviour
{
	//NPC Position
	public Vector3 nPCPosition;
	public string nPCInScene;

	//NPC Shop
	public InventoryBag_SO nPCShop;
	private bool shopIsOpen;

	private SpriteRenderer nPCSpriteRenderer;
	private BoxCollider2D boxCollider;
	private Rigidbody2D rb;
	private Animator animator;
	public DialogueController dialogueController;

	public bool canInteractive=true;
	public bool canMove = true;
	public bool isMoving = false;

	[SerializeField]
	private Vector3 targetPosition;

	[SerializeField]
	private float moveSpeed;

	[SerializeField]
	private Vector2 moveRange;

	private float idleTime;
	private float idleTimer = 0f;

	private void Awake()
	{
		idleTime = Random.Range(2, 5);
		boxCollider = GetComponent<BoxCollider2D>();
		nPCSpriteRenderer = GetComponent<SpriteRenderer>();
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

	private void Start()
	{
		transform.position = nPCPosition;
		CheckNPCVisable();
		GenerateTargetPosition();
	}

	private void Update()
	{
		// NPC moves only when not interacting and allowed to move
		if (canMove && !isMoving)
		{
			idleTimer += Time.deltaTime;

			if (idleTimer >= idleTime)
			{
				idleTime = Random.Range(2, 5);
				idleTimer = 0f;
				StartMoving();
			}
		}

		if (isMoving)
		{
			MoveTowardsTarget();
		}
	}
	public void OpenShop()
	{ 
		if(shopIsOpen)return;
		shopIsOpen = true;
		dialogueController.isTalking = true;
		EventHandler.CallBagOpenEvent(SlotType.Shop,nPCShop);
		EventHandler.CallGamePaueseEvent(true);

	}

	public void CloseShop()
	{ 
		shopIsOpen=false;
		dialogueController.isTalking = false;
		EventHandler.CallBagCloseEvent(SlotType.Shop,nPCShop);
		EventHandler.CallGamePaueseEvent(false);
	}
	public void SetNPCVisable()
	{
		boxCollider.enabled = true;
		nPCSpriteRenderer.enabled = true;
	}

	public void SetNPCInVisable()
	{
		boxCollider.enabled = false;
		nPCSpriteRenderer.enabled = false;
	}

	private void OnEnable()
	{
		EventHandler.AfterSceneLoadEvent += OnAfterSceneLoadEvent;
		EventHandler.GamePauseEvent += OnGamePauseEvent;
	}
	private void OnDisable()
	{
		EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
		EventHandler.GamePauseEvent -= OnGamePauseEvent;
	}
	private void OnAfterSceneLoadEvent()
	{
		CheckNPCVisable();
	}
	private void OnGamePauseEvent(bool gamePause)
	{
		canMove=!gamePause;
	}
	public void CheckNPCVisable()
	{
		if (nPCInScene == SceneManager.GetActiveScene().name)
		{
			SetNPCVisable();
		}
		else
		{
			SetNPCInVisable();
		}
	}
	public void GenerateTargetPosition()
	{
		targetPosition = new Vector3(
			Random.Range(nPCPosition.x - moveRange.x, nPCPosition.x + moveRange.x),
			Random.Range(nPCPosition.y - moveRange.y, nPCPosition.y + moveRange.y),
			0
		);
	}

	private void StartMoving()
	{
		isMoving = true;
		canInteractive=false;
		GenerateTargetPosition();
	}

	private void MoveTowardsTarget()
	{
		if (Vector3.Distance(transform.position, targetPosition) > 0.2f)
		{
			Vector2 dir = (targetPosition - transform.position).normalized;

			// Set animation parameters based on direction
			if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y) && dir.x > 0)
			{
				animator.SetFloat("DirX", 1);
				animator.SetFloat("DirY", 0);
			}
			else if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y) && dir.x < 0)
			{
				animator.SetFloat("DirX", -1);
				animator.SetFloat("DirY", 0);
			}
			else if (Mathf.Abs(dir.x) < Mathf.Abs(dir.y) && dir.y > 0)
			{
				animator.SetFloat("DirX", 0);
				animator.SetFloat("DirY", 1);
			}
			else if (Mathf.Abs(dir.x) < Mathf.Abs(dir.y) && dir.y < 0)
			{
				animator.SetFloat("DirX", 0);
				animator.SetFloat("DirY", -1);
			}

			animator.SetBool("isMoving", true);

			// Move NPC towards target position
			rb.MovePosition((Vector2)transform.position + dir * moveSpeed * Time.deltaTime);
		}
		else
		{
			// Stop moving when close enough to the target
			animator.SetBool("isMoving", false);
			isMoving = false;
			canInteractive = true;
		}
	}
}
