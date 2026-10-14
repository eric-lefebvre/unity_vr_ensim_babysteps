using UnityEngine;

public class ExternalPressurePlate : MonoBehaviour
{
    [SerializeField]
    private GameEvent onPlayerPressure;

    // NOTE(Eric-Nicolas): PlaquePressionExterne ne touche volontairement pas le sol,
    // sinon cette méthode se déclenche car en contact avec le tapis.
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[PlaqueExterne] Contact avec {other.name} (tag {other.tag})");
        if (other.CompareTag("Player"))
        {
            if (!WhackAMoleManager.IsFinished)
            {
                Debug.Log("Whack-a-mole non terminé: la plaque de pression ne se déclenche pas.");
                return;
            }

            onPlayerPressure.TriggerEvent();
            gameObject.SetActive(false);
            Debug.Log("Ouverture de l'ascenseur");
        }
    }
}
