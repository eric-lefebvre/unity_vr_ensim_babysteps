using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BabyparkLogic : MonoBehaviour
{
    public GameObject Poignee;
    public GameEvent PlayerOutBabyPark;

    [SerializeField]
    private GameObject vrController;

    [SerializeField]
    private AudioClip MayaYapSession1;
    [SerializeField]
    private AudioClip MayaYapSession2;

    private void Awake()
    {
        // D�placement de la poignee au milieu
        ConfigurableJoint joint = Poignee.GetComponent<ConfigurableJoint>();
        Poignee.transform.localPosition = Poignee.transform.localPosition + joint.axis.normalized * 2 * joint.linearLimit.limit;
        Poignee.GetComponent<XRGrabInteractable>().selectEntered.AddListener(OnPoigneeGrabbed);
    }

    IEnumerator WaitForMayaStopYapping()
    {
        yield return new WaitForSeconds(MayaYapSession1.length + MayaYapSession2.length);
        Poignee.GetComponent<XRGrabInteractable>().enabled = true;
        vrController.SetActive(true);
        vrController.GetComponent<Animator>().SetBool("PlayPressLeftButton", true);
    }

    void OnPoigneeGrabbed(SelectEnterEventArgs args)
    {
        vrController.SetActive(false);
    }

    public void StartWaitForMayaStopYapping()
    {
        Poignee.GetComponent<XRGrabInteractable>().enabled = false;
        StartCoroutine(WaitForMayaStopYapping());
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger hit: " + other.name);
        if (other.tag == "Player")
        {
            PlayerOutBabyPark.TriggerEvent();
        }
    }
}
