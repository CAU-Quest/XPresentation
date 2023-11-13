using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSetter : MonoBehaviour
{
    public GameObject player;
    
    public Transform stageCenter;
    public Transform audienceCenter;

    private OVRCameraRig ovrCameraRig;

    void Start()
    {
        ovrCameraRig = player.GetComponentInChildren<OVRCameraRig>();
    }
    
    public void GoToAudience()
    {
        player.transform.position = audienceCenter.position;
        ovrCameraRig.transform.rotation = audienceCenter.rotation;
    }
    
    public void GoToStage()
    {
        player.transform.position = stageCenter.position;
        ovrCameraRig.transform.rotation = stageCenter.rotation;
    }

}
