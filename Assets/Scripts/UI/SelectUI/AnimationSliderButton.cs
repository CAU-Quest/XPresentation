using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AnimationSliderButton : SliderButton
{
    private enum Usage
    {
        PositionX, PositionY, PositionZ, RotationX, RotationY, RotationZ, ScaleX, ScaleY, ScaleZ,
    };
    
    [SerializeField] private Usage usage;
    [SerializeField] private Transform sliderLocal;
    
    private const float SwipeLength = 500f;
    private SlideObjectData _selectedSlideObjectData;
    private Transform _activeTipTransform;

    public int currentSlideNum;
    public bool canUse = true;

    public void SetSlideObjectDataWithIndex(SlideObjectData slideObjectData, int index)
    {
        _selectedSlideObjectData = slideObjectData;
        currentSlideNum = index;
    }
    
    public void SetSlideObjectData(SlideObjectData slideObjectData)
    {
        _selectedSlideObjectData = slideObjectData;
    }
    
    public override void InitProperty(PresentationObject selectedObject)
    {
        SelectedObject = selectedObject;
        base.InitProperty(selectedObject);
    }

    protected override void SetInitialValue()
    {
        switch (usage)
        {
            case Usage.PositionX:
                initialValue = _selectedSlideObjectData.position.x;
                break;
            case Usage.PositionY:
                initialValue = _selectedSlideObjectData.position.y;
                break;
            case Usage.PositionZ:
                initialValue = _selectedSlideObjectData.position.z;
                break;
            case Usage.RotationX:
                initialValue = _selectedSlideObjectData.rotation.x;
                break;
            case Usage.RotationY:
                initialValue = _selectedSlideObjectData.rotation.y;
                break;
            case Usage.RotationZ:
                initialValue = _selectedSlideObjectData.rotation.z;
                break;
            case Usage.ScaleX:
                initialValue = _selectedSlideObjectData.scale.x;
                break;
            case Usage.ScaleY:
                initialValue = _selectedSlideObjectData.scale.y;
                break;
            case Usage.ScaleZ:
                initialValue = _selectedSlideObjectData.scale.z;
                break;
        }
    }

    public override void OnHoverButton() //onHover
    {
        if (!canUse) return;
        SetHoverColor();
        base.OnHoverButton();
    }
    
    public override void OnUnhoverButton() //onUnhover
    {
        if (!canUse) return;
        SetDefaultColor();
        base.OnUnhoverButton();
    }

    public override void OnSelectButton() //Handle_onSelect
    {
        if (!canUse) return;
        SetActiveTipTransform();
        base.OnSelectButton();
        
        SetSelectColor();
        DOKill();
        SetGraduation(true);
        SetHandleValuePanel(true);
        SetValuePanel(false);
    }
    
    public override void OnUnselectButton() //Handle_onUnselect
    {
        if (!canUse) return;
        ResetHandlePosition();
        _activeTipTransform = null;
        base.OnUnselectButton();
        
        SetHoverColor();
        DOKill();
        SetGraduation(false);
        SetHandleValuePanel(false);
        SetValuePanel(true);
    }
    
    private void ResetHandlePosition()
    {
        handle.transform.localPosition = initialHandleLocalPos;
    }
    
    protected virtual void Update()
    {
        if (!isSelectingHandle) return;

        var value = UpdateHandleTransform();
        UpdateValue(value);
    }

    private float UpdateHandleTransform()
    {
        var tipLocalPos = sliderLocal.InverseTransformPoint(_activeTipTransform.position);
        var tipLocalPosX = tipLocalPos.x;

        if (tipLocalPosX - initialHandleLocalPos.x > SwipeLength)
        {
            Debug.Log("A");
            handle.localPosition = new Vector3(initialHandleLocalPos.x + SwipeLength, initialHandleLocalPos.y, initialHandleLocalPos.z);
        }
        else if(tipLocalPosX - initialHandleLocalPos.x < -SwipeLength)
        {
            Debug.Log("B");
            handle.localPosition = new Vector3(initialHandleLocalPos.x - SwipeLength, initialHandleLocalPos.y, initialHandleLocalPos.z);
        }
        else
        {
            Debug.Log("C");
            handle.localPosition = new Vector3(tipLocalPosX, initialHandleLocalPos.y, initialHandleLocalPos.z);
        }
        return handle.localPosition.x;
    }
    
    protected override void UpdateValue(float value) //value = -500 ~ 500
    {
        value /= 500f; //-1 ~ 1
        value *= maxOffset;
        value += initialValue;
            
        var position = _selectedSlideObjectData.position;
        var rotation = _selectedSlideObjectData.rotation.eulerAngles;
        var scale = _selectedSlideObjectData.scale;
        var newPosition = _selectedSlideObjectData.position; 
        var newRotation = _selectedSlideObjectData.rotation;
        var newScale = _selectedSlideObjectData.scale;
        
        switch (usage)
        {
            case Usage.PositionX:
                newPosition = new Vector3(value, position.y ,position.z);
                _selectedSlideObjectData.position = newPosition;
                break;
            case Usage.PositionY:
                newPosition = new Vector3(position.x, value, position.z);
                _selectedSlideObjectData.position = newPosition;
                break;
            case Usage.PositionZ:
                newPosition = new Vector3(position.x, value, position.z);
                _selectedSlideObjectData.position = newPosition;
                break;
            case Usage.RotationX:
                newRotation = Quaternion.Euler(value, rotation.y ,rotation.z);
                _selectedSlideObjectData.rotation = newRotation;
                break;
            case Usage.RotationY:
                newRotation = Quaternion.Euler(rotation.x, value, rotation.z);
                _selectedSlideObjectData.rotation = newRotation;
                break;
            case Usage.RotationZ:
                newRotation = Quaternion.Euler(rotation.x, rotation.y, value);
                _selectedSlideObjectData.rotation = newRotation;
                break;
            case Usage.ScaleX:
                newScale = new Vector3(value, scale.y ,scale.z);
                _selectedSlideObjectData.scale = newScale;
                break;
            case Usage.ScaleY:
                newScale = new Vector3(scale.x, value, scale.z);
                _selectedSlideObjectData.scale = newScale;
                break;
            case Usage.ScaleZ:
                newScale = new Vector3(scale.x, scale.y, value);
                _selectedSlideObjectData.scale = newScale;
                break;
        }

        NewSlideObjectData = new SlideObjectData(_selectedSlideObjectData, newPosition, newRotation, newScale);
        SelectedObject.ApplyDataToSlideWithIndex(NewSlideObjectData, currentSlideNum);
        if(currentSlideNum == MainSystem.Instance.currentSlideNum)
            SelectedObject.ApplyDataToObject(NewSlideObjectData);
        _selectedSlideObjectData = NewSlideObjectData;
        XRSelector.Instance.NotifySlideObjectDataChangeToObservers();
        base.UpdateValue(value);
    }
    
    private void SetActiveTipTransform()
    {
        var left = OVRInput.Get(OVRInput.RawButton.LIndexTrigger);
        var right = OVRInput.Get(OVRInput.RawButton.RIndexTrigger);
        if (left && right) //Both
        {
            var handlePos = handle.transform.position;
            var distanceL = Vector3.Distance(handlePos, PlayerManager.Instance.leftTip.position);
            var distanceR = Vector3.Distance(handlePos, PlayerManager.Instance.rightTip.position);
            _activeTipTransform = (distanceL < distanceR)? PlayerManager.Instance.leftTip : PlayerManager.Instance.rightTip;
        }
        else if (left)
        {
            _activeTipTransform = PlayerManager.Instance.leftTip;
        }
        else if (right)
        {
            _activeTipTransform = PlayerManager.Instance.rightTip;
        }
    }
}
