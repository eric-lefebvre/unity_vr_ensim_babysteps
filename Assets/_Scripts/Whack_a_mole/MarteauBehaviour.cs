using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class MarteauBehaviour : StaticInstance<MarteauBehaviour>
{
    [SerializeField] private GameEvent marteauPickedUp;
    private XRGrabInteractable xrGrabInteractable;

    [Header("Haptic Feedback")]
    [Range(0f, 1f)]
    public float hapticFeedbackIntensity;
    [Range(0f, 1f)]
    public float hapticFeedbackDuration;
    [ReadOnly]
    public XRBaseController ControllerHoldingMarteau;

    override protected void Awake()
    {
        base.Awake();
        xrGrabInteractable = GetComponent<XRGrabInteractable>();
        xrGrabInteractable.selectEntered.AddListener(OnSelectEnter);
        xrGrabInteractable.selectExited.AddListener(OnSelectExit);
    }

    public void OnSelectEnter(BaseInteractionEventArgs eventArgs)
    {
        if (eventArgs.interactorObject is XRBaseControllerInteractor controllerInteractor)
        {
            OnSelectEnter(controllerInteractor.xrController);
        }
    }

    public void OnSelectEnter(XRBaseController controller)
    {
        marteauPickedUp.TriggerEvent();
        ControllerHoldingMarteau = controller;
    }

    public void OnSelectExit(BaseInteractionEventArgs eventArgs)
    {
        if (eventArgs.interactorObject is XRBaseControllerInteractor controllerInteractor)
        {
            OnSelectExit(controllerInteractor.xrController);
        }
    }

    public void OnSelectExit(XRBaseController controller)
    {
        ControllerHoldingMarteau = null;
    }
}