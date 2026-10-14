using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voiture : MonoBehaviour
{
    public void Klaxon()
    {
        AudioSystem.Instance.Play3DSoundRdmPitchVol("klaxon", transform.position);
    }
}
