using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Oculus.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SliderButton : MonoBehaviour
{
    private enum Mode { Transform, Color }
    
    private enum Usage
    {
        PositionX, PositionY, PositionZ, RotationX, RotationY, RotationZ, ScaleX, ScaleY, ScaleZ,
        ColorR, ColorG, ColorB, ColorA
    };
    
    [SerializeField] private Mode mode;
    [SerializeField] private Usage usage;
    [SerializeField]private float maxOffset = 1f;
    
    [SerializeField] private Transform handle;
    [SerializeField] private Image buttonImage, handleImage, graduationImage, graduationShadowImage, valuePanelImage, handleValuePanelImage;
    [SerializeField] private TextMeshProUGUI valueText, handleValueText;

    private const float SwipeLength = 0.2f;
    
    private SelectUI _selectUI;
    private Transform _selectedTransform;
    private Material _selectedMaterial;
    
    private Transform _activeTipTransform;
    private bool _turnOff, _isSelectingHandle;
    
    private Vector3 _initialHandleLocalPos;
    private float _initialValue;
    
    private void Awake()
    {
        _selectUI = GetComponentInParent<SelectUI>();
        _selectUI.initUI += InitProperty;
    }

    private void InitProperty()
    {
        if(mode == Mode.Transform) _selectedTransform = _selectUI.selectedProperty.Transform;
        else _selectedMaterial = _selectUI.selectedProperty.Material;
        
        SetInitialValue();
        valueText.text = _initialValue.ToString("0.0");
        handleValueText.text = _initialValue.ToString("0.0");
    }

    private void Start()
    {
        _initialHandleLocalPos = handle.localPosition;
        UnselectHandle();
        TurnOffHandle();
    }

    private void Update()
    {
        if (!_isSelectingHandle) return;

        var value = UpdateHandleTransform();
        UpdateValue(value);
    }

    private float UpdateHandleTransform()
    {
        var tipPosX = _activeTipTransform.position.x;
        var initialHandlePos = transform.TransformPoint(_initialHandleLocalPos);
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

    private void UpdateValue(float value) //value = -500 ~ 500
    {
        Vector3 position = new Vector3(), scale  = new Vector3(), rotation = new Vector3();
        Color color = new Color();
        
        value /= 500f; //-1 ~ 1
        if (mode == Mode.Transform)
        {
            value *= maxOffset;
            value += _initialValue;
            
            position = _selectedTransform.position;
            rotation = _selectedTransform.rotation.eulerAngles;
            scale = _selectedTransform.localScale;
        }
        else if (mode == Mode.Color)
        {
            color = _selectedMaterial.color;
        }
        
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
            case Usage.ColorR:
                _selectedMaterial.color = new Color(value, color.g, color.b, color.a);
                break;
            case Usage.ColorG:
                _selectedMaterial.color = new Color(color.r, value, color.b, color.a);
                break;
            case Usage.ColorB:
                _selectedMaterial.color = new Color(color.r, color.g, value, color.a);
                break;
            case Usage.ColorA:
                _selectedMaterial.color = new Color(color.r, color.g, color.b, value);
                break;
        }

        valueText.text = value.ToString("0.0");
        handleValueText.text = value.ToString("0.0");
    }

    public void TurnOnHandle() //onHover
    {
        handle.gameObject.SetActive(true);
    }
    
    public void TurnOffHandle() //onUnhover
    {
        if (_isSelectingHandle) _turnOff = true;
        else handle.gameObject.SetActive(false);
    }
    
    public void SelectHandle() //Handle_onSelect
    {
        transform.SetAsLastSibling();
        SetActiveTipTransform();
        SetInitialValue();
        _isSelectingHandle = true;
        
        DOKill();
        graduationImage.DOFade(1f, 0.2f).SetEase(Ease.OutCirc);
        graduationShadowImage.DOFade(1f, 0.2f).SetEase(Ease.OutCirc);
        handleValuePanelImage.DOFade(1f, 0.2f).SetEase(Ease.OutCirc);
        handleValueText.DOFade(1f, 0.2f).SetEase(Ease.OutCirc);
        
        valuePanelImage.DOFade(0f, 0.2f).SetEase(Ease.InCirc);
        valueText.DOFade(0f, 0.2f).SetEase(Ease.InCirc);
    }

    public void UnselectHandle() //Handle_onUnselect
    {
        ResetHandlePosition();
        _isSelectingHandle = false;
        _activeTipTransform = null;
        if (_turnOff) handle.gameObject.SetActive(false);
        
        DOKill();
        graduationImage.DOFade(0f, 0.2f).SetEase(Ease.InCirc);
        graduationShadowImage.DOFade(0f, 0.2f).SetEase(Ease.InCirc);
        handleValuePanelImage.DOFade(0f, 0.2f).SetEase(Ease.InCirc);
        handleValueText.DOFade(0f, 0.2f).SetEase(Ease.InCirc);
        
        valuePanelImage.DOFade(1f, 0.2f).SetEase(Ease.OutCirc);
        valueText.DOFade(1f, 0.2f).SetEase(Ease.OutCirc);
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
    
    private void SetInitialValue()
    {
        switch (usage)
        {
            case Usage.PositionX:
                _initialValue = _selectedTransform.position.x;
                break;
            case Usage.PositionY:
                _initialValue = _selectedTransform.position.y;
                break;
            case Usage.PositionZ:
                _initialValue = _selectedTransform.position.z;
                break;
            case Usage.RotationX:
                _initialValue = _selectedTransform.rotation.x;
                break;
            case Usage.RotationY:
                _initialValue = _selectedTransform.rotation.y;
                break;
            case Usage.RotationZ:
                _initialValue = _selectedTransform.rotation.z;
                break;
            case Usage.ScaleX:
                _initialValue = _selectedTransform.localScale.x;
                break;
            case Usage.ScaleY:
                _initialValue = _selectedTransform.localScale.y;
                break;
            case Usage.ScaleZ:
                _initialValue = _selectedTransform.localScale.z;
                break;
            case Usage.ColorR:
                _initialValue = _selectedMaterial.color.r;
                break;
            case Usage.ColorG:
                _initialValue = _selectedMaterial.color.g;
                break;
            case Usage.ColorB:
                _initialValue = _selectedMaterial.color.b;
                break;
            case Usage.ColorA:
                _initialValue = _selectedMaterial.color.a;
                break;
        }
    }

    private void DOKill()
    {
        graduationImage.DOKill();
        graduationShadowImage.DOKill();
        handleValuePanelImage.DOKill();
        handleValueText.DOKill();
        valuePanelImage.DOKill();
        valueText.DOKill();
    }
    
    private void ResetHandlePosition()
    {
        if(mode == Mode.Transform) handle.transform.localPosition = _initialHandleLocalPos;
        else if (mode == Mode.Color) handle.transform.localPosition = new Vector3(_initialValue * 500f, _initialHandleLocalPos.y, _initialHandleLocalPos.z);
    }

    public void SetHoverColor()
    {
        buttonImage.DOColor(ColorManager.SliderHover, 0.2f);
    }

    public void SetSelectColor()
    {
        buttonImage.DOColor(ColorManager.SliderSelect, 0.2f);
        handleImage.DOColor(ColorManager.Select, 0.2f);
    }

    public void SetDefaultColor()
    {
        buttonImage.DOColor(ColorManager.SliderDefault, 0.2f);
        handleImage.DOColor(ColorManager.Default, 0f);
    }
}