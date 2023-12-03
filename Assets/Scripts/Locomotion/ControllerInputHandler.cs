using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class ControllerInputHandler : MonoBehaviour
{
    //Locomotion
    private ControllerMovement _movement;
    private ControllerTurn _turn;
    private bool _lastActiveL, _lastActiveR;
    private GrabInteractor _leftGrabInteractor, _rightGrabInteractor;
    [SerializeField] private bool _isOnSelect;
    private PresentationObject _selectedObject;
    
    //Switch Slider
    [SerializeField] private SnapListController snapListController;
    private bool _isReadyToSwitch;
        
    private void Awake()
    {
        _movement = GetComponent<ControllerMovement>();
        _turn = GetComponent<ControllerTurn>();
    }

    private void Start()
    {
        _leftGrabInteractor = PlayerManager.Instance.leftGrabInteractor;
        _rightGrabInteractor = PlayerManager.Instance.rightGrabInteractor;
        XRSelector.Instance.selectUI.onOpen += EnableSwitchSlide;
        XRSelector.Instance.selectUI.onClose += DisableSwitchSlide;
    }

    private void EnableSwitchSlide(PresentationObject selectedObject)
    {
        _isOnSelect = true;
        _selectedObject = selectedObject;
    }
    
    private void DisableSwitchSlide()
    {
        _isOnSelect = false;
        _selectedObject = null;
    }


    private void Update()
    {
        Locomotion();
        SwitchSlide();
    }

    private void Locomotion() //move & turn
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

    private void SwitchSlide()
    {
        var axis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        
        if (axis == new Vector2())
        {
            _isReadyToSwitch = true;
            return;
        }
        if (!_isReadyToSwitch) return;

        if (_isOnSelect)
        {
            
            var index = MainSystem.Instance.currentSlideNum;
            if (Vector2.Dot(axis.normalized, Vector2.left) > 0.5f)
            {
                if(index - 1 < 0) return;
                if (!_selectedObject.animationList[index].previousData.isVisible) return;
                snapListController.SwipeToLeft();
                _isReadyToSwitch = false;
            }
            else if (Vector2.Dot(axis.normalized, Vector2.right) > 0.5f)
            {
                if(index + 1 >= MainSystem.Instance.GetSlideCount()) return;
                if (!_selectedObject.animationList[index].nextData.isVisible) return;
                snapListController.SwipeToRight();
                _isReadyToSwitch = false;
            }
            
        }
        else
        {
            if (Vector2.Dot(axis.normalized, Vector2.left) > 0.5f)
            {
                snapListController.SwipeToLeft();
                _isReadyToSwitch = false;
            }
            else if (Vector2.Dot(axis.normalized, Vector2.right) > 0.5f)
            {
                snapListController.SwipeToRight();
                _isReadyToSwitch = false;
            }
        }
    }
}
