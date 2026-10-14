using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class VRControllerBehaviour : MonoBehaviour
{
    [Tooltip("Déplacer le focus point sur l'élément à mettre en valeur et faire pointer son y local vers l'extérieur")]
    public Transform FocusPoint;
    private Transform mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main.transform;
    }

    void Update()
    {
        LookAtCamera();
    }

    void LookAtCamera()
    {
        Vector3 desiredUp = (mainCamera.transform.position - FocusPoint.position).normalized;

        // Rotation à appliquer pour que focusPoint.up => desiredUp
        Quaternion correction = Quaternion.FromToRotation(FocusPoint.up, desiredUp);
        transform.rotation = correction * transform.rotation;
    }
}
