using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCAction : MonoBehaviour
{
	public Vector3 nPCPosition;
	public string nPCInScene;

	private SpriteRenderer nPCSpriteRenderer;
	private BoxCollider2D boxCollider;
	private Rigidbody2D rb;
	private Animator animator;

	public bool canInteractive=true;
	private bool canMove = true;
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

	public void StartInteraction()
	{
		canInteractive = false;
		isMoving = false;
		animator.SetBool("isMoving", false); 							
	}

	public void EndInteraction()
	{
		canInteractive = true;
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
	}

	private void OnDisable()
	{
		EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoadEvent;
	}

	private void OnAfterSceneLoadEvent()
	{
		CheckNPCVisable();
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

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player")) // Assumes player has "Player" tag
		{
			StartInteraction(); // Start conversation when player is nearby
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			EndInteraction(); // End conversation when player leaves
		}
	}
}
