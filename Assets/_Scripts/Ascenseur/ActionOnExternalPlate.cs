using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ActionOnExternalPlate : MonoBehaviour
{
    [SerializeField]
    private Animator elevatorDoorAnimator;

    public void StartTransition()
    {
        AudioSystem.Instance.Play3DSoundRdmPitchVol("bell", transform.position);
        elevatorDoorAnimator.SetTrigger("openDoors");
    }
}
