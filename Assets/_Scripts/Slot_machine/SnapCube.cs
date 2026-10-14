using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SnapCube : MonoBehaviour
{
    private static bool activeHologram;
    GameObject hologram;

    bool placed = false;
    bool inTheTrigger = false;
    public GameObject snappedObject;

    private void OnTriggerStay(Collider other)
    {
        if (placed) return;

        XRGrabInteractable interactable = other.GetComponent<XRGrabInteractable>();
        if (interactable != null && interactable.isSelected && hologram == null && activeHologram == false)
        {
            activeHologram = true;
            hologram = Instantiate(other.gameObject);

            hologram.GetComponent<Rigidbody>().isKinematic = true;

            foreach (var col in hologram.GetComponentsInChildren<Collider>()) col.enabled = false;

            hologram.transform.position = this.transform.position;
            hologram.transform.rotation = this.transform.rotation;

            hologram.SetActive(true);
            inTheTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (placed) return;

        if (hologram != null && activeHologram == true)
        {
            Destroy(hologram);
            activeHologram = false;
            MyAsyncTask();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (placed || !inTheTrigger) return;

        XRGrabInteractable grab = collision.gameObject.GetComponent<XRGrabInteractable>();
        if (grab == null) return;

        snappedObject = collision.gameObject;

        string name = snappedObject.name;

        char letter = name[name.Length - 1];

        int slotIndex = (int)char.GetNumericValue(this.gameObject.name[this.gameObject.name.Length - 1]);

        GameObject machine = GameObject.Find("machine");
        if (machine != null)
        {
            machine.GetComponent<WheelRotation>().CheckIfWon(letter, slotIndex, false);
        }

        // --- Logique de snap ---
        grab.selectExited.AddListener(OnGrabAgain);
        placed = true;
        activeHologram = false;
        inTheTrigger = false;
        if (hologram != null) Destroy(hologram);

        snappedObject.GetComponent<Rigidbody>().isKinematic = true;
        snappedObject.transform.position = this.transform.position;
        snappedObject.transform.rotation = this.transform.rotation;

        foreach (var col in GetComponents<BoxCollider>()) col.enabled = false;
        this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    private void OnGrabAgain(SelectExitEventArgs args)
    {
        if (snappedObject == null) return;

        placed = false;

        Rigidbody rb = snappedObject.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        string cubeName = snappedObject.name;
        char letter = cubeName[cubeName.Length - 1];
    
        int slotIndex = (int)char.GetNumericValue(this.gameObject.name[this.gameObject.name.Length - 1]);

        GameObject machine = GameObject.Find("machine");
        if (machine != null)
        {
            machine.GetComponent<WheelRotation>().CheckIfWon(letter, slotIndex, true);
        }

        args.interactableObject.transform.GetComponent<XRGrabInteractable>().selectExited.RemoveListener(OnGrabAgain);

        snappedObject = null;
        foreach (var col in GetComponents<BoxCollider>()) col.enabled = true;
        this.gameObject.layer = LayerMask.NameToLayer("Socle");
    }

    async void MyAsyncTask()
    {
        await Task.Delay(100);
        inTheTrigger = false;
    }
}
