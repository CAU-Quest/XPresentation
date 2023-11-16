using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Oculus.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderButton : MonoBehaviour
{
    [SerializeField] private Transform handle;
    [SerializeField] private Image graduationImage, graduationShadowImage, valuePanelImage, handleValuePanelImage;
    [SerializeField] private TextMeshProUGUI valueText, handleValueText;

    private Transform _activeTipTransform;
    private bool _turnOff, _isSelectingHandle;
    private Vector3 _handleInitialLocalPos;
    private const float SwipeLength = 0.2f;
    
    private void Start()
    {
        _handleInitialLocalPos = handle.localPosition;
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
        var initialHandlePos = transform.TransformPoint(_handleInitialLocalPos);
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

    private void UpdateValue(float value)
    {
        
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
        SetActiveTipTransform();
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
        _isSelectingHandle = false;
        _activeTipTransform = null;
        if(_turnOff) handle.gameObject.SetActive(false);
        handle.transform.localPosition = _handleInitialLocalPos;
        
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

    private void DOKill()
    {
        graduationImage.DOKill();
        graduationShadowImage.DOKill();
        handleValuePanelImage.DOKill();
        handleValueText.DOKill();
        valuePanelImage.DOKill();
        valueText.DOKill();
    }
}