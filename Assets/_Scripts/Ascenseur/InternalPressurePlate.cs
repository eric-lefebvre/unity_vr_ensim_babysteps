using UnityEngine;

public class InternalPressurePlate : MonoBehaviour
{
    [SerializeField]
    private GameEvent onPlayerPressure;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[PlaqueInterne] Contact avec {other.name} (tag {other.tag})");
        if (other.CompareTag("Player"))
        {
            onPlayerPressure.TriggerEvent();
            gameObject.SetActive(false);
            Debug.Log("Fermeture de l'ascenseur");
        }
    }
}
