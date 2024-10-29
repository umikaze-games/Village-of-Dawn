using UnityEngine;

public class TallGrassAnimationOnContact : MonoBehaviour
{
    // Reference to the Front and Back gameobjects
    private Animator animatorFront;
    private Animator animatorBack;

    private void Start()
    {
        // Get the Animator component attached to each child
        animatorFront = transform.Find("front").gameObject.GetComponent<Animator>();
        animatorBack = transform.Find("back").gameObject.GetComponent<Animator>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Trigger the animation if Animator component is found
        if (animatorFront != null && animatorBack != null)
        {
            animatorFront.SetTrigger("WaveTrigger");
            animatorBack.SetTrigger("WaveTrigger");
        }
    }
}