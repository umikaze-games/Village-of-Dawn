using UnityEngine;

public class ChestAnimationOnContact : MonoBehaviour
{
    // make sure we only open once
    private bool isOpened = false;

    // Reference to the Animator component
    private Animator animator;

    private void Start()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("No Animator component found on this GameObject.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // open the chest !
        if (!isOpened)
        {
            isOpened = true;
            // Trigger the animation if Animator component is found
            if (animator != null)
            {
                animator.SetTrigger("OpenTrigger");
            }
        }
    }
}