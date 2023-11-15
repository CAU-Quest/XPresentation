using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GrabInteractable a;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space)) PlayerManager.Instance.leftGrabInteractor.ForceSelect(a);
    }
}
