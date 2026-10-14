using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSpin : MonoBehaviour
{
    public bool isClockwise;
    public float speed;

    private void Update()
    {
        float direction = isClockwise ? -1f : 1f;

        transform.Rotate(0f, 0f, direction * speed * Time.deltaTime);
    }
}
