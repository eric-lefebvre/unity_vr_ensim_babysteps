using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MenuButtonHelper : MonoBehaviour
{
    private Material material;
    private Renderer renderer;
    private Color originalEmission;

    [SerializeField]
    private GameObject vrController;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            renderer = GetComponentInChildren<Renderer>();
            if (renderer == null)
            {
                Debug.Log("Material non trouve !!!!!!!!!");
                this.enabled = false;
            }
        }
        material = renderer.material;

        originalEmission = material.GetColor("_EmissionColor");

        GetComponent<XRSimpleInteractable>().hoverEntered.AddListener(OnHoverEnter);
        GetComponent<XRSimpleInteractable>().hoverExited.AddListener(OnHoverExit);
    }

    void OnHoverEnter(HoverEnterEventArgs args)
    {
        material.SetColor("_EmissionColor", originalEmission * 1.4f);
        vrController.SetActive(true);
        vrController.GetComponent<Animator>().SetBool("PlayPressRightTrigger", true);
    }

    void OnHoverExit(HoverExitEventArgs args)
    {
        material.SetColor("_EmissionColor", originalEmission);
        vrController.SetActive(false);
        vrController.GetComponent<Animator>().SetBool("PlayPressRightTrigger", false);
    }
}
