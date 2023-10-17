using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(GestureMovement))]
[RequireComponent(typeof(GestureTurn))]
public class GestureLocomotion : MonoBehaviour
{
    private GestureMovement _movement;
    private GestureTurn _turn; 
    private bool _isLeftActivated, _isRightActivated;

    private void Awake()
    {
        _movement = GetComponent<GestureMovement>();
        _turn = GetComponent<GestureTurn>();
    }

    private void Update()
    {
        if (_isLeftActivated && _isRightActivated) _turn.UpdateTurn();
        else if (_isLeftActivated || _isRightActivated) _movement.UpdateMovement();
    }

    public void OnActivate(bool isRight)
    {
        if (isRight) _isRightActivated = true;
        else _isLeftActivated = true;
        _movement.SetProperty(isRight);
    }

    public void OnDeactivate(bool isRight)
    {
        if (isRight) _isRightActivated = false;
        else _isLeftActivated = false;
        
        _turn.ResetProperty();
    }
}
