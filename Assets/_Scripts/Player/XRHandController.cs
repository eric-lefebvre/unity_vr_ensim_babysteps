using UnityEngine;
using UnityEngine.InputSystem;

public class XRHandController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator handAnimator;
    
    [Header("Manual Input Actions")]
    [SerializeField] private InputAction gripAction;
    [SerializeField] private InputAction triggerAction;

    private void OnEnable()
    {
        gripAction.Enable();
        triggerAction.Enable();
    }

    private void OnDisable()
    {
        gripAction.Disable();
        triggerAction.Disable();
    }

    private void Start()
    {
        if (handAnimator == null)
        {
            handAnimator = GetComponentInChildren<Animator>();
        }
    }

    private void Update()
    {
        if (handAnimator == null) return;

        var grip = gripAction.ReadValue<float>();
        var trigger = triggerAction.ReadValue<float>();

        handAnimator.SetFloat("Grip", grip);
        handAnimator.SetFloat("Trigger", trigger);
    }
}
