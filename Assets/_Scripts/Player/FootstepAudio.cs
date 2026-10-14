using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    [Header("Footstep Settings")]
    public float distanceThreshold = 1.5f; // Distance avant de jouer le bruit de pas

    private CharacterController characterController;
    private Vector2 lastPosition2D;        // position XZ
    private float distanceTraveled = 0f;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        Vector3 startPos = transform.position;
        lastPosition2D = new Vector2(startPos.x, startPos.z);
    }

    void Update()
    {
        // Position actuelle en XZ
        Vector3 currentPos3D = transform.position;
        Vector2 currentPos2D = new Vector2(currentPos3D.x, currentPos3D.z);

        // Calcul du déplacement horizontal
        Vector2 displacement2D = currentPos2D - lastPosition2D;

        // Si le joueur ne bouge pas, reset distance
        if (displacement2D.sqrMagnitude < 0.0001f)
        {
            distanceTraveled = 0f;
            lastPosition2D = currentPos2D;
            return;
        }

        // Accumule la distance
        distanceTraveled += displacement2D.magnitude;

        // Jouer le bruit de pas si seuil atteint
        if (distanceTraveled >= distanceThreshold)
        {
            AudioSystem.Instance.PlayFootstep(transform.position);

            distanceTraveled = 0f; // reset
        }

        lastPosition2D = currentPos2D;
    }
}