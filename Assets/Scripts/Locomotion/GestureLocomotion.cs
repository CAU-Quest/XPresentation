using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(GestureMovement))]
[RequireComponent(typeof(GestureSnapTurn))]
public class GestureLocomotion : MonoBehaviour
{
    private GestureMovement _movement;
    private GestureSnapTurn _snapTurn;

    [SerializeField] private bool _isLeftActivated;
    [SerializeField] private bool _isRightActivated;

    private void Awake()
    {
        _movement = GetComponent<GestureMovement>();
        _snapTurn = GetComponent<GestureSnapTurn>();
    }

    private void Update()
    {
        if (_isLeftActivated && _isRightActivated) _snapTurn.SnapTurn();
        else if (_isRightActivated) _movement.Move();
        //else if (_isLeftActivated) _movement.Teleport();
    }

    public void OnLeftActivate()
    {
        _isLeftActivated = true;
        _movement.SetProperty(false);
    }

    public void OnLeftDeactivate()
    {
        _isLeftActivated = false;
        _snapTurn.ResetProperty();
    }
    
    public void OnRightActivate()
    {
        _isRightActivated = true;
        _movement.SetProperty(true);
    }

    public void OnRightDeactivate()
    {
        _isRightActivated = false;
        _snapTurn.ResetProperty();
    }
}
