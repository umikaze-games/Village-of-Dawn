using UnityEngine;

public class BlockPushOnContact : MonoBehaviour
{
    public float pushForce = 4f; // Force applied when pushing
    public float friction = 3f;  // Factor to slow down the block over time

    private Rigidbody2D rb;
    private bool isPushed = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Gradually reduce the velocity over time (simulate friction)
        if (rb.linearVelocity.magnitude > 0)
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, friction * Time.deltaTime);
        }
        else
        {
            isPushed = false;
        }
    }

    // This method checks for player collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isPushed)
        {
            Vector2 direction = collision.GetContact(0).normal; // Get the contact point normal (opposite of collision direction)
            rb.AddForce(-direction * pushForce, ForceMode2D.Impulse); // Apply force in the opposite direction
            isPushed = true;
        }
    }
}