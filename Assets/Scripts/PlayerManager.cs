using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    
    public Transform player, eye, leftHandAnchor, rightHandAnchor, leftTip, rightTip;
    public GrabInteractor leftGrabInteractor, rightGrabInteractor;
    public RayInteractor leftRayInteractor, rightRayInteractor;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }
    
    public IEnumerator VibrateController(float waitTime, float frequency, float amplitude, OVRInput.Controller controller)
    {
        OVRInput.SetControllerVibration(frequency, amplitude, controller);
        yield return new WaitForSeconds(waitTime);
        OVRInput.SetControllerVibration(0, 0, controller);

    }
}
