using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.XR.Interaction.Toolkit;

public class Taupe : MonoBehaviour
{
    private Animator animator;
    private Collider col;
    private WhackAMoleManager manager;
    private bool canBeHit = true;
    public bool isUp { get; private set; }

    void Awake()
    {
        animator = GetComponent<Animator>();
        manager = FindObjectOfType<WhackAMoleManager>();
        col = GetComponent<Collider>();
    }

    public void GoUp()
    {
        canBeHit = true;
        isUp = true;
        animator.SetTrigger("up");
    }

    public void GoDown()
    {
        // laugh seulement si taupe ratee
        if (canBeHit)
        {
            AudioSystem.Instance.Play3DSoundRdmPitchVol("taupe laugh", transform.position);
        }

        canBeHit = false;
        isUp = false;
        animator.SetTrigger("down");
    }

    public void DisableTaupe()
    {
        canBeHit = false;
        isUp = false;
        col.enabled = false;
        animator.ResetTrigger("up");
        animator.ResetTrigger("down");
    }

    public void ShowUpFinal()
    {
        animator.Play("up");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canBeHit) return;
        if (!isUp || WhackAMoleManager.Score >= WhackAMoleManager.ScoreLimit) return;

        XRGrabInteractable grab = other.GetComponentInParent<XRGrabInteractable>();
        if (other.CompareTag("Marteau") && grab != null && grab.isSelected)
        {
            canBeHit = false;
            manager.OnTaupeHit(this);
            AudioSystem.Instance.Play3DSoundRdmPitchVol("taupe hit", transform.position);

            // Haptic Feedback
            MarteauBehaviour marteau = MarteauBehaviour.Instance;
            if (marteau.ControllerHoldingMarteau != null)
            {
                marteau.ControllerHoldingMarteau.SendHapticImpulse(marteau.hapticFeedbackIntensity, marteau.hapticFeedbackDuration);
            }
        }
    }

    /*
    old version
    private void OnTriggerEnter(Collider other)
    {
        if (!canBeHit) return;
        if (!isUp || WhackAMoleManager.Score >= 5) return;

        XRGrabInteractable grab = other.GetComponent<XRGrabInteractable>();
        if (other.CompareTag("Marteau") && grab != null && grab.isSelected)
        {
            canBeHit = false;
            manager.OnTaupeHit(this);
            AudioSystem.Instance.Play3DSoundRdmPitchVol("Taupe Bonked", transform.position);
        }
    }
    */
}