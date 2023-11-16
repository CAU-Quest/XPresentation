using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GrabInteractable a;

    public void Select()
    {
        PlayerManager.Instance.leftGrabInteractor.ForceSelect(a);
    }
    
    public void Release()
    {
        PlayerManager.Instance.leftGrabInteractor.ForceRelease();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)) ;
    }
}
