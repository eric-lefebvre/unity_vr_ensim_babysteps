using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Menu : MonoBehaviour
{
    [Header("Menu demarrage (statique)")]
    [SerializeField] private GameObject menuPrincipal;

    [SerializeField] private GameObject boutonJouer;
    private XRSimpleInteractable jouerInteractable;

    [SerializeField] private GameObject boutonQuitter;
    private XRSimpleInteractable quitterInteractable;


    [Header("Menu bouton main")]
    [SerializeField] private GameObject menuMain;

    private InputAction boutonA;
    private InputAction triggerAction;

    [SerializeField] private GameObject boutonReprendreMain;
    private XRSimpleInteractable reprendreInteractableMain;

    [SerializeField] private GameObject boutonQuitterMain;
    private XRSimpleInteractable quitterInteractableMain;


    [Header("GameObjects pour le Menu Demarrage")]
    [SerializeField] private GameObject poigneeBarriere;
    private XRGrabInteractable poigneeBarriereInteractable;


    [Header("Fog VR")]
    [SerializeField] private GameObject fogSphere;

    private Material fogMaterial;

    private enum FogState
    {
        Idle,
        Appearing,
        Disappearing
    }

    private FogState currentFogState = FogState.Idle;

    private float currentFogAlpha = 0f;

    [SerializeField] private float fogMaxAlpha = 0.75f;
    [SerializeField] private float fogSpeed = 2f;


    [Header("Cardboards physiques")]
    [SerializeField] private Transform spawnCardboardParent;

    [SerializeField] private float cardboardThrowForce = 2f;
    [SerializeField] private float cardboardTorqueForce = 5f;


    [Header("GameObjects communs")]
    [SerializeField] private GameObject deplacement;

    [SerializeField] private GameObject rotation;

    [SerializeField] private XRRayInteractor rayInteractorGauche;
    [SerializeField] private XRRayInteractor rayInteractorDroite;


    [Header("Debug")]
    [SerializeField] private bool alwaysEnableRays = false;


    [Header("GameEvent")]
    [SerializeField] private GameEvent gameStarted;


    void Awake()
    {
        setRays(true);

        // Trigger
        triggerAction = new InputAction("Trigger", InputActionType.Button);

        triggerAction.AddBinding("<XRController>/triggerPressed");
        triggerAction.performed += OnTriggerPressed;
        triggerAction.Enable();

        // Bouton pause
        boutonA = new InputAction(
            "BoutonA",
            InputActionType.Button,
            "<XRController>{RightHand}/primaryButton"
        );

        boutonA.performed += OnBoutonAPressed;
        boutonA.Enable();

        // Bloquer les deplacements et la rotation du joueur
        deplacement.SetActive(false);
        rotation.SetActive(false);

        // Pas d'interactions avec la barrière du babypark
        poigneeBarriereInteractable = poigneeBarriere.GetComponent<XRGrabInteractable>();
        poigneeBarriereInteractable.enabled = false;

        // Setup fog
        fogMaterial = fogSphere.GetComponent<Renderer>().material;

        currentFogAlpha = fogMaxAlpha;

        SetFogAlpha(currentFogAlpha);

        fogSphere.SetActive(true);

        // Bouton Jouer
        jouerInteractable = boutonJouer.GetComponent<XRSimpleInteractable>();
        jouerInteractable?.activated.AddListener(OnPressedJouer);

        // Bouton Quitter
        quitterInteractable = boutonQuitter.GetComponent<XRSimpleInteractable>();
        quitterInteractable?.activated.AddListener(OnPressedQuitter);

        // Bouton Reprendre
        reprendreInteractableMain = boutonReprendreMain.GetComponent<XRSimpleInteractable>();
        reprendreInteractableMain?.activated.AddListener(OnPressedReprendre);

        // Bouton Quitter menu pause
        quitterInteractableMain = boutonQuitterMain.GetComponent<XRSimpleInteractable>();
        quitterInteractableMain?.activated.AddListener(OnPressedQuitter);
    }

    void Update()
    {
        switch (currentFogState)
        {
            case FogState.Appearing:
                UpdateFogAppearing();
                break;

            case FogState.Disappearing:
                UpdateFogDisappearing();
                break;
        }
    }

    private void SetFogAlpha(float alpha)
    {
        Color c = fogMaterial.color;
        c.a = alpha;
        fogMaterial.color = c;
    }

    void UpdateFogAppearing()
    {
        if (!fogSphere.activeSelf)
        {
            fogSphere.SetActive(true);
        }

        currentFogAlpha += Time.deltaTime * fogSpeed;

        currentFogAlpha = Mathf.Min(currentFogAlpha, fogMaxAlpha);

        SetFogAlpha(currentFogAlpha);

        if (currentFogAlpha >= fogMaxAlpha)
        {
            currentFogState = FogState.Idle;
        }
    }

    void UpdateFogDisappearing()
    {
        currentFogAlpha -= Time.deltaTime * fogSpeed;

        currentFogAlpha = Mathf.Max(currentFogAlpha, 0f);

        SetFogAlpha(currentFogAlpha);

        if (currentFogAlpha <= 0f)
        {
            fogSphere.SetActive(false);
            currentFogState = FogState.Idle;
        }
    }

    private void OnTriggerPressed(InputAction.CallbackContext ctx)
    {
        CheckRay(rayInteractorGauche);
        CheckRay(rayInteractorDroite);
    }

    void CheckRay(XRRayInteractor ray)
    {
        if (ray.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            var interactable = hit.collider.GetComponentInParent<XRSimpleInteractable>();

            if (interactable == jouerInteractable)
                OnPressedJouer(null);

            if (interactable == quitterInteractable || interactable == quitterInteractableMain)
                OnPressedQuitter(null);

            if (interactable == reprendreInteractableMain)
                OnPressedReprendre(null);
        }
    }

    void OnPressedJouer(ActivateEventArgs args)
    {
        Debug.Log("Le bouton Jouer a ete presse !");

        // Spawn versions physiques
        SpawnPhysicalCardboard(boutonJouer);
        SpawnPhysicalCardboard(boutonQuitter);

        boutonJouer.SetActive(false);
        boutonQuitter.SetActive(false);

        menuPrincipal.SetActive(false);

        poigneeBarriereInteractable.enabled = true;

        deplacement.SetActive(true);
        rotation.SetActive(true);

        currentFogState = FogState.Disappearing;

        setRays(false);

        // J'active l'event de début
        gameStarted.TriggerEvent();
    }

    void OnPressedQuitter(ActivateEventArgs args)
    {
        Debug.Log("Le jeu se ferme !!!!!");
        Application.Quit();
    }

    void OnPressedReprendre(ActivateEventArgs args)
    {
        Debug.Log("Le bouton Reprendre a ete presse !");

        // Spawn versions physiques
        SpawnPhysicalCardboard(boutonReprendreMain);
        SpawnPhysicalCardboard(boutonQuitterMain);

        deplacement.SetActive(true);
        rotation.SetActive(true);

        menuMain.SetActive(false);

        currentFogState = FogState.Disappearing;

        setRays(false);
    }

    private void OnBoutonAPressed(InputAction.CallbackContext ctx)
    {
        Debug.Log("Bouton pause presse");

        // Empêche la pause tant que le menu principal est affiché
        if (menuPrincipal.activeSelf)
            return;

        bool isOpening = !menuMain.activeSelf;

        if (isOpening)
        {
            // Pause
            deplacement.SetActive(false);
            rotation.SetActive(false);

            fogSphere.SetActive(true);

            currentFogState = FogState.Appearing;

            setRays(true);
        }
        else
        {
            // Reprendre
            deplacement.SetActive(true);
            rotation.SetActive(true);

            currentFogState = FogState.Disappearing;

            setRays(false);
        }

        menuMain.SetActive(isOpening);
    }

    private void setRays(bool active)
    {
        // Mode debug : garde toujours les raycasts actifs
        if (alwaysEnableRays)
        {
            rayInteractorDroite.enabled = true;
            rayInteractorGauche.enabled = true;
            return;
        }

        rayInteractorDroite.enabled = active;
        rayInteractorGauche.enabled = active;
    }

    private void SpawnPhysicalCardboard(GameObject original)
    {
        // Clone
        GameObject clone = Instantiate(
            original,
            original.transform.position,
            original.transform.rotation,
            spawnCardboardParent
        );

        clone.name = original.name + "_Physical";

        clone.SetActive(true);

        // Désactiver tous les layers UI/raycast éventuels
        clone.layer = LayerMask.NameToLayer("Default");

        // Supprimer TOUS les XRSimpleInteractable du clone
        XRSimpleInteractable[] interactables =
            clone.GetComponentsInChildren<XRSimpleInteractable>(true);

        foreach (XRSimpleInteractable interactable in interactables)
        {
            Destroy(interactable);
        }

        MenuButtonHelper[] helpers =
            clone.GetComponentsInChildren<MenuButtonHelper>(true);

        foreach (MenuButtonHelper helper in helpers)
        {
            Destroy(helper);
        }

        // Supprimer interaction UI
        /*XRSimpleInteractable simpleInteractable =
            clone.GetComponent<XRSimpleInteractable>();

        if (simpleInteractable != null)
        {
            Destroy(simpleInteractable);
        }*/

        // Rigidbody
        Rigidbody rb = clone.GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb = clone.AddComponent<Rigidbody>();
        }

        rb.useGravity = true;
        rb.isKinematic = false;

        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // Collider
        Collider col = clone.GetComponent<Collider>();

        if (col == null)
        {
            BoxCollider box = clone.AddComponent<BoxCollider>();

            Renderer renderer = clone.GetComponentInChildren<Renderer>();

            if (renderer != null)
            {
                box.size = renderer.bounds.size;
            }
        }

        // Grab interactable
        XRGrabInteractable grab = clone.GetComponent<XRGrabInteractable>();

        if (grab == null)
        {
            grab = clone.AddComponent<XRGrabInteractable>();
        }

        grab.useDynamicAttach = true;
        grab.matchAttachPosition = true;
        grab.matchAttachRotation = true;

        // Petite impulsion
        Vector3 randomDirection =
            UnityEngine.Random.insideUnitSphere * 0.5f + Vector3.up;

        rb.AddForce(
            randomDirection * cardboardThrowForce,
            ForceMode.Impulse
        );

        rb.AddTorque(
            UnityEngine.Random.insideUnitSphere * cardboardTorqueForce,
            ForceMode.Impulse
        );
    }

    private void OnDestroy()
    {
        triggerAction?.Disable();
        triggerAction?.Dispose();

        boutonA?.Disable();
        boutonA?.Dispose();
    }
}