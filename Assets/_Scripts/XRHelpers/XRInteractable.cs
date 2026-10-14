using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRInteractable : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Material material;
    private Renderer renderer;
    private Color originalEmission;
    private Color hoverEmission = Color.cyan;

    // Pour le respawn quand ca sort des bounds
    private Vector3 boundsOrigin = new Vector3(0, 2.45f, 13);
    private Vector3 boundsSize = new Vector3(19, 9, 40);
    private Bounds bounds;
    private Vector3 spawnPoint;

    private bool selected = false;

    [Header("Pick up event")]
    [SerializeField]
    private bool triggersEvent = true;
    public GameEvent PickedUp;
    [SerializeField]
    private bool triggersSound = true;
    bool wasSelectedLastFrame = false;

    [Header("Son")]
    [SerializeField]
    private float maxVolume;
    private bool soundEnabled = false;
    [SerializeField]
    private string soundName;


    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Recuperation du renderer ou le premier trouv� chez les enfants
        renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            renderer = GetComponentInChildren<Renderer>();
            if (renderer == null)
            {
                Debug.Log("Material non trouv� !!!!!!!!!");
                this.enabled = false;
            }
        }
        material = renderer.material;

        originalEmission = material.GetColor("_Color");

        grabInteractable.hoverEntered.AddListener(OnHoverEnter);
        grabInteractable.hoverExited.AddListener(OnHoverExit);
        grabInteractable.selectEntered.AddListener(OnSelectEnter);
        grabInteractable.selectExited.AddListener(OnSelectExit);

        // Respawn si out of bounds
        bounds = new Bounds(boundsOrigin, boundsSize);
        spawnPoint = transform.position;
    }

    void Update()
    {
        // D�clench� UNIQUEMENT la frame du pickup
        if (!wasSelectedLastFrame && grabInteractable.isSelected)
        {
            if (triggersSound)
                soundEnabled = true;
            if (triggersEvent)
                PickedUp.TriggerEvent();
        }
        wasSelectedLastFrame = grabInteractable.isSelected;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!soundEnabled)
            return;

        float impactSpeed = collision.relativeVelocity.magnitude;

        if (!grabInteractable.isSelected)
        {
            float volume = impactSpeed < 1.5f ? 0f : maxVolume * (1 - Mathf.Exp(-impactSpeed / 8f));
            Debug.Log("Impact reel a vitesse: " + impactSpeed);
            Debug.Log("Son joue au volume: " + volume);
            AudioSystem.Instance.Play3DSoundRdmPitch(soundName, transform.position, volume);
        }
    }

    void OnHoverEnter(HoverEnterEventArgs args)
    {
        if (selected) return;
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", originalEmission * 1.5f);
    }

    void OnHoverExit(HoverExitEventArgs args)
    {
        material.DisableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", originalEmission);
    }

    void OnSelectEnter(SelectEnterEventArgs args)
    {
        material.DisableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", originalEmission);
        selected = true;

        AudioSystem.Instance.Play3DSoundRdmPitchVol("object picked up", transform.position);
    }

    void OnSelectExit(SelectExitEventArgs args)
    {
        selected = false;

        if (!bounds.Contains(transform.position))
        {
            Debug.Log("Objet hors des limites !");
            transform.position = spawnPoint;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawWireCube(boundsOrigin, boundsSize);
    }
}