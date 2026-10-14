using UnityEngine;

public class TableTrigger : MonoBehaviour
{
    public GameObject startButton;
    private bool gameActive = false;

    private void OnTriggerEnter(Collider other)
    {
        // Only show the button if a game isn't already running
        if (!gameActive && (other.CompareTag("MainCamera") || other.CompareTag("Player")))
        {
            startButton.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // We add a tiny delay or check to prevent "flickering"
        if (other.CompareTag("MainCamera") || other.CompareTag("Player"))
        {
            // Instead of hiding instantly, we only hide if the game hasn't started
            if (!gameActive) Invoke(nameof(HideButton), 0.5f);
        }
    }

    void HideButton()
    {
        // Only hide if we aren't currently touching the trigger
        startButton.SetActive(false);
    }

    // Call this from your BlackjackManager.StartNewGame to stop the button from coming back
    public void LockTrigger()
    {
        gameActive = true;
        startButton.SetActive(false);
    }

    // Add this to allow the game to be restarted
    public void UnlockTrigger()
    {
        gameActive = false;
    }
}