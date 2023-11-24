using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformSliderButton : SliderButton
{
    private enum Usage
    {
        PositionX, PositionY, PositionZ, RotationX, RotationY, RotationZ, ScaleX, ScaleY, ScaleZ,
    };
    
    [SerializeField] private Usage usage;
    private Transform _selectedTransform;
    private Transform _activeTipTransform;
    
    protected override void InitProperty()
    {
        _selectedTransform = selectUI.selectedProperty.Transform;
        base.InitProperty();
    }
    
    protected override void SetInitialValue()
    {
        switch (usage)
        {
            case Usage.PositionX:
                initialValue = _selectedTransform.position.x;
                break;
            case Usage.PositionY:
                initialValue = _selectedTransform.position.y;
                break;
            case Usage.PositionZ:
                initialValue = _selectedTransform.position.z;
                break;
            case Usage.RotationX:
                initialValue = _selectedTransform.rotation.x;
                break;
            case Usage.RotationY:
                initialValue = _selectedTransform.rotation.y;
                break;
            case Usage.RotationZ:
                initialValue = _selectedTransform.rotation.z;
                break;
            case Usage.ScaleX:
                initialValue = _selectedTransform.localScale.x;
                break;
            case Usage.ScaleY:
                initialValue = _selectedTransform.localScale.y;
                break;
            case Usage.ScaleZ:
                initialValue = _selectedTransform.localScale.z;
                break;
        }
    }

    public override void OnHoverButton() //onHover
    {
        SetHoverColor();
        base.OnHoverButton();
    }
    
    public override void OnUnhoverButton() //onUnhover
    {
        SetDefaultColor();
        base.OnUnhoverButton();
    }

    public override void OnSelectButton() //Handle_onSelect
    {
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
        var tipPosX = _activeTipTransform.position.x;
        var initialHandlePos = transform.TransformPoint(initialHandleLocalPos);
        if (tipPosX - initialHandlePos.x > SwipeLength)
        {
            handle.position = new Vector3(initialHandlePos.x + SwipeLength, initialHandlePos.y, initialHandlePos.z);
        }
        else if(tipPosX - initialHandlePos.x < -SwipeLength)
        {
            handle.position = new Vector3(initialHandlePos.x - SwipeLength, initialHandlePos.y, initialHandlePos.z);
        }
        else
        {
            handle.position = new Vector3(tipPosX, initialHandlePos.y, initialHandlePos.z);
        }
        return handle.localPosition.x;
    }
    
    protected override void UpdateValue(float value) //value = -500 ~ 500
    {
        value /= 500f; //-1 ~ 1
        value *= maxOffset;
        value += initialValue;
            
        var position = _selectedTransform.position;
        var rotation = _selectedTransform.rotation.eulerAngles;
        var scale = _selectedTransform.localScale;
        
        switch (usage)
        {
            case Usage.PositionX:
                _selectedTransform.position = new Vector3(value, position.y ,position.z);
                break;
            case Usage.PositionY:
                _selectedTransform.position = new Vector3(position.x, value, position.z);
                break;
            case Usage.PositionZ:
                _selectedTransform.position = new Vector3(position.x, position.y, value);
                break;
            case Usage.RotationX:
                _selectedTransform.rotation = Quaternion.Euler(value, rotation.y ,rotation.z);
                break;
            case Usage.RotationY:
                _selectedTransform.rotation = Quaternion.Euler(rotation.x, value, rotation.z);
                break;
            case Usage.RotationZ:
                _selectedTransform.rotation = Quaternion.Euler(rotation.x, rotation.y, value);
                break;
            case Usage.ScaleX:
                _selectedTransform.localScale = new Vector3(value, scale.y ,scale.z);
                break;
            case Usage.ScaleY:
                _selectedTransform.localScale = new Vector3(scale.x, value, scale.z);
                break;
            case Usage.ScaleZ:
                _selectedTransform.localScale = new Vector3(scale.x, scale.y, value);
                break;
        }
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
