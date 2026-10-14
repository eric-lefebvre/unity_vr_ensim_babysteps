using UnityEngine;

public class IntersectionHighlighter : MonoBehaviour
{
    [Header("Lights")]
    public Light correctPathLight;
    public Light[] wrongPathLights;

    [Header("Detection Settings")]
    public float revealDistance = 8f; // How close you need to be to see the lights
    public Transform playerCamera;   // Drag your Main Camera here

    [Header("Visual Settings")]
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;

    void Start()
    {
        // Set colors at start
        if (correctPathLight != null) correctPathLight.color = correctColor;
        foreach (Light l in wrongPathLights) if(l != null) l.color = wrongColor;
        
        // Find player camera automatically if not assigned
        if (playerCamera == null) playerCamera = Camera.main.transform;
    }

    void Update()
    {
        if (playerCamera == null) return;

        // Calculate how far the player is from this intersection
        float distance = Vector3.Distance(transform.position, playerCamera.position);

        // If close enough, turn lights on. If too far, turn them off to save performance.
        bool isNear = distance <= revealDistance;

        if (correctPathLight != null) correctPathLight.enabled = isNear;
        
        foreach (Light l in wrongPathLights)
        {
            if (l != null) l.enabled = isNear;
        }
    }
}