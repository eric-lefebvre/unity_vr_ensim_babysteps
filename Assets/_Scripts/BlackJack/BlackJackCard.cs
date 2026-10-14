using UnityEngine;
using System.Collections;

public class BlackjackCard : MonoBehaviour
{
    public int cardValue; // Set this in the Inspector for each prefab (2-11)
    public float flipSpeed = 4f;

    public void Reveal()
    {
        StartCoroutine(FlipRoutine());
    }

    IEnumerator FlipRoutine()
    {
        Quaternion startRot = transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0, 180, 0); 

        float elapsed = 0;
        while (elapsed < 1.0f)
        {
            elapsed += Time.deltaTime * flipSpeed;
            transform.rotation = Quaternion.Slerp(startRot, endRot, elapsed);
            yield return null;
        }
        transform.rotation = endRot;
    }
}