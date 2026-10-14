using UnityEngine;

public class LeverCollisionFix : MonoBehaviour
{
    void Start()
    {
        GameObject rig = GameObject.Find("XR Origin (XR Rig)");

        if (rig != null)
        {
            Collider[] bodyColliders = rig.GetComponentsInChildren<Collider>();
            Collider leverCollider = GetComponent<Collider>();

            foreach (var col in bodyColliders)
            {
                Physics.IgnoreCollision(leverCollider, col);
            }
        }
    }
}
