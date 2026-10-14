using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable]
public class Haptic
{
    [Range(0f, 1f)]
    public float intensity;
    [Range(0f, 1f)]
    public float duration;

    public void TriggerHaptic(BaseInteractionEventArgs eventArgs)
    {
        if (eventArgs.interactorObject is XRBaseControllerInteractor controllerInteractor)
        {
            TriggerHaptic(controllerInteractor.xrController);
        }
    }

    public void TriggerHaptic(XRBaseController controller)
    {
        if (intensity > 0)
        {
            controller.SendHapticImpulse(intensity, duration);
        }
    }
}

[RequireComponent(typeof(XRBaseInteractable))]
public class HapticInteractable : MonoBehaviour
{
    public Haptic hapticOnActivated;
    public Haptic hapticOnHoverEntered;
    public Haptic hapticOnHoverExited;
    public Haptic hapticOnSelectEntered;
    public Haptic hapticOnSelectExited;

    private void Start()
    {
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        interactable.activated.AddListener(hapticOnActivated.TriggerHaptic);
        interactable.hoverEntered.AddListener(hapticOnHoverEntered.TriggerHaptic);
        interactable.hoverExited.AddListener(hapticOnHoverExited.TriggerHaptic);
        interactable.selectEntered.AddListener(hapticOnSelectEntered.TriggerHaptic);
        interactable.selectExited.AddListener(hapticOnSelectExited.TriggerHaptic);
    } 
}
