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
    [SerializeField] private GrabInteractable handle;
    [SerializeField] private Image graduationImage, valuePanelImage, handleValuePanelImage;
    [SerializeField] private TextMeshProUGUI valueText, handleValueText;

    [SerializeField] private GrabInteractor _activeHandGrabInteractor;

    private bool _turnOff, _isGrabbing;
    
    private void Start()
    {
        ReleaseHandle();
        TurnOffHandle();
    }

    public void TurnOnHandle() //onHover
    {
        handle.gameObject.SetActive(true);
    }
    
    public void TurnOffHandle() //onUnhover
    {
        if (_isGrabbing) _turnOff = true;
        else handle.gameObject.SetActive(false);
    }
    
    public void GrabHandle() //onSelect
    {
        SetActiveGrabInteractor();
        //if (_activeHandGrabInteractor == null) return; 
        
        _activeHandGrabInteractor.ForceSelect(handle);
        _isGrabbing = true;
        
        DOKill();
        graduationImage.DOFade(1f, 0.2f).SetEase(Ease.OutCirc);
        handleValuePanelImage.DOFade(1f, 0.2f).SetEase(Ease.OutCirc);
        handleValueText.DOFade(1f, 0.2f).SetEase(Ease.OutCirc);
        
        valuePanelImage.DOFade(0f, 0.2f).SetEase(Ease.InCirc);
        valueText.DOFade(0f, 0.2f).SetEase(Ease.InCirc);
    }

    public void ReleaseHandle() //onUnselect
    {
        if(_activeHandGrabInteractor != null) _activeHandGrabInteractor.ForceRelease();
            
        _activeHandGrabInteractor = null;
        _isGrabbing = false;
        if(_turnOff) handle.gameObject.SetActive(false);
        handle.transform.localPosition = new Vector3(0f, 0f, handle.transform.localPosition.z);
        
        DOKill();
        graduationImage.DOFade(0f, 0.2f).SetEase(Ease.InCirc);
        handleValuePanelImage.DOFade(0f, 0.2f).SetEase(Ease.InCirc);
        handleValueText.DOFade(0f, 0.2f).SetEase(Ease.InCirc);
        
        valuePanelImage.DOFade(1f, 0.2f).SetEase(Ease.OutCirc);
        valueText.DOFade(1f, 0.2f).SetEase(Ease.OutCirc);
    }

    private void DOKill()
    {
        graduationImage.DOKill();
        handleValuePanelImage.DOKill();
        handleValueText.DOKill();
        valuePanelImage.DOKill();
        valueText.DOKill();
    }

    private void SetActiveGrabInteractor()
    {
        var left = OVRInput.Get(OVRInput.RawButton.LIndexTrigger);
        var right = OVRInput.Get(OVRInput.RawButton.RIndexTrigger);
        if (left && right) //Both
        {
            var handlePos = handle.transform.position;
            var distanceL = Vector3.Distance(handlePos, PlayerManager.Instance.leftTip.position);
            var distanceR = Vector3.Distance(handlePos, PlayerManager.Instance.rightTip.position);
            _activeHandGrabInteractor = (distanceL < distanceR)? PlayerManager.Instance.leftGrabInteractor : PlayerManager.Instance.rightGrabInteractor;
        }
        else if (left)
        {
            _activeHandGrabInteractor = PlayerManager.Instance.leftGrabInteractor;
        }
        else if (right)
        {
            _activeHandGrabInteractor = PlayerManager.Instance.rightGrabInteractor;
        }
    }
}
