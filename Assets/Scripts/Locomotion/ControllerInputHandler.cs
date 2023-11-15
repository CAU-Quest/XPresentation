using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class ControllerInputHandler : MonoBehaviour
{
    private ControllerMovement _movement;
    private ControllerTurn _turn;
    private bool _lastActiveL, _lastActiveR;
    private GrabInteractor _leftGrabInteractor, _rightGrabInteractor;
    private void Awake()
    {
        _movement = GetComponent<ControllerMovement>();
        _turn = GetComponent<ControllerTurn>();
    }

    private void Start()
    {
        _leftGrabInteractor = PlayerManager.Instance.leftGrabInteractor;
        _rightGrabInteractor = PlayerManager.Instance.rightGrabInteractor;
    }

    private void Update()
    {
        var isActiveL = OVRInput.Get(OVRInput.RawButton.LHandTrigger) && !_leftGrabInteractor.isGrabbing;
        var isActiveR = OVRInput.Get(OVRInput.RawButton.RHandTrigger) && !_rightGrabInteractor.isGrabbing;
        
        if(_lastActiveL && _lastActiveR && !(isActiveL && isActiveR)) _turn.ResetProperty();
        if(!_lastActiveL && isActiveL) _movement.SetPropertyL();
        if(!_lastActiveR && isActiveR) _movement.SetPropertyR();
        
        if (isActiveL && isActiveR) _turn.UpdateTurn();
        else if (isActiveL || isActiveR) _movement.UpdateMovement();

        _lastActiveL = isActiveL;
        _lastActiveR = isActiveR;
    }
}
