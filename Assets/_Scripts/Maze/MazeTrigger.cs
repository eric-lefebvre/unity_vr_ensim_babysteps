using UnityEngine;

public class MazeFinishTrigger : MonoBehaviour
{
    public GameEvent finishEvent; // Drag your MazeDone event file here

    private void OnTriggerEnter(Collider other)
    {
        // VR rigs can be named anything, so we trigger on any contact
        if (finishEvent != null)
        {
            Debug.Log("Maze Finished! Sending Event...");
            finishEvent.TriggerEvent(); // This matches your script's method name

            // Optional: disable the wall so it doesn't trigger twice
            //gameObject.SetActive(false);
        }
    }
}