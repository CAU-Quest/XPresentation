using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Oculus.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SelectUI : MonoBehaviour
{
    public enum Mode { Transform = 1, Color, Animation, Etc, Close }
    private Mode _mode;

    [SerializeField] private GameObject[] canvases;
    [SerializeField] private Image[] buttons;
    [SerializeField] private Image[] labelPanels;
    [SerializeField] private TextMeshProUGUI[] labelTexts;
    
    public PresentationObject selectedObject;
    private ISelectedObjectModifierInitializer[][] _initializers;
    private ISelectedObjectModifier[][] _modifiers;
    private bool _isOpened;
    public Action<PresentationObject> onOpen;
    public Action onClose;
    
    private void Awake()
    {
        _initializers = new ISelectedObjectModifierInitializer[canvases.Length][];
        _modifiers = new ISelectedObjectModifier[canvases.Length][];
        
        for (int i = 0; i < canvases.Length; i++)
        {
            _initializers[i] = canvases[i].GetComponentsInChildren<ISelectedObjectModifierInitializer>(true);
            
            _modifiers[i] = canvases[i].GetComponentsInChildren<ISelectedObjectModifier>(true);
            foreach (var modifier in _modifiers[i])
            {
                modifier.WhenHasModification += modifier.UpdateSelectedObjectData;
            }
        }
    }

    void Start()
    {
        PlayerManager.Instance.leftGrabInteractor.onSelect += GetSelectedObjectInfo;
        PlayerManager.Instance.rightGrabInteractor.onSelect += GetSelectedObjectInfo;
        PlayerManager.Instance.leftRayInteractor.onSelect += GetSelectedObjectInfo;
        PlayerManager.Instance.rightRayInteractor.onSelect += GetSelectedObjectInfo;
        onClose += XRSelector.Instance.DeactivateBoundBox;
        
        CloseUI();
    }

    private void GetSelectedObjectInfo(GrabInteractable interactable)
    {
        var parent = interactable.transform;
        while (parent != null)
        {
            if (parent.TryGetComponent(out SelectObject select))
            {
                selectedObject = (PresentationObject) select.presentationObject;
                OpenUI();
                break;
            }
            parent = parent.parent;
        }
    }
    
    private void GetSelectedObjectInfo(RayInteractable interactable)
    {
        var parent = interactable.transform;
        while (parent != null)
        {
            if (parent.TryGetComponent(out SelectObject select))
            {
                selectedObject = (PresentationObject)select.presentationObject;
                OpenUI();
                break;
            }

            parent = parent.parent;
        }
    }
    
    private void OpenUI()
    {
        canvases[0].SetActive(true);
        Select(Mode.Transform);

        foreach (var component in _initializers[(int)Mode.Transform])
        {
            component.InitializeProperty(selectedObject);
        }

        _isOpened = true;
        onOpen?.Invoke(selectedObject);
    }

    public void CloseUI()
    {
        for (int i = 0; i < canvases.Length; i++)
        {
            canvases[i].SetActive(false);
        }

        selectedObject = null;
        _isOpened = false;
        onClose?.Invoke();
    }

    public void OnHover(int index)
    {
        if(index == (int)_mode) return;
        
        index--;
        buttons[index].DOColor(ColorManager.Hover, 0.3f);
        labelPanels[index].DOColor(ColorManager.Hover, 0.3f);
        labelPanels[index].DOFade(1f, 0.3f);
        labelTexts[index].DOFade(1f, 0.3f);
    }

    public void OnUnhover(int index)
    {
        if(index == (int)_mode) return;
        
        index--;
        buttons[index].DOColor(ColorManager.Default, 0.3f);
        labelPanels[index].DOColor(ColorManager.Default, 0.3f);
        labelPanels[index].DOFade(0f, 0.3f);
        labelTexts[index].DOFade(0f, 0.3f);
    }

    public void ChangeMode(int index)
    {
        Select((Mode)index);
    }

    private void Select(Mode mode)
    {
        foreach (var component in _initializers[(int)_mode])
        {
            component.FinalizeProperty();
        }
        
        _mode = mode;
        var modeIndex = (int)mode - 1;
        
        for (int i = 1; i < canvases.Length; i++)
        { 
            canvases[i].SetActive(i - 1 == modeIndex);
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].transform.DOScale((i == modeIndex) ? 1f : 0.8f, 0.3f);
            buttons[i].DOColor((i == modeIndex) ? ColorManager.Select : ColorManager.Default, 0.3f);
            labelPanels[i].DOColor((i == modeIndex) ? ColorManager.Select : ColorManager.Default, 0.3f);
            labelPanels[i].DOFade((i == modeIndex) ? 1f : 0f, 0.3f);
            labelTexts[i].DOFade((i == modeIndex) ? 1f : 0f, 0.3f);
        }

        foreach (var component in _initializers[(int)mode])
        {
            component.InitializeProperty(selectedObject);
        }
    }
}
