using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ActionOnInternalPlate : MonoBehaviour
{
    [SerializeField]
    private float fadeDuration = 1f;

    [SerializeField]
    private string nextSceneName = "Salle2";

    [SerializeField]
    private Animator elevatorDoorAnimator;

    public void StartTransition()
    {

        AudioSystem.Instance.Play3DSoundRdmPitchVol("bell", transform.position);

        elevatorDoorAnimator.ResetTrigger("openDoors");
        elevatorDoorAnimator.SetFloat("speed", -1);
        elevatorDoorAnimator.SetTrigger("openDoors");

        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
        yield return new WaitForSeconds(fadeDuration);
        SceneManager.LoadScene(nextSceneName);
    }
}
