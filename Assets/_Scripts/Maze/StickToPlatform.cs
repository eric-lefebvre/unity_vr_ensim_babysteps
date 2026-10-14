using UnityEngine;

public class StickToPlatform : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // We check if it's the player's camera/body
        // Or you can check by Tag if your XR Origin is tagged "Player"
        if (other.gameObject.CompareTag("MainCamera") || other.gameObject.name.Contains("Origin"))
        {
            // This makes the player a "child" of the elevator
            // so they move exactly with it
            other.transform.root.SetParent(transform);
            Debug.Log("Player is now on the elevator!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera") || other.gameObject.name.Contains("Origin"))
        {
            // This un-glues the player when they step off
            other.transform.root.SetParent(null);
            Debug.Log("Player left the elevator.");
        }
    }
}