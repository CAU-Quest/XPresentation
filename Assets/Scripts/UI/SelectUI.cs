using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class SelectUI : MonoBehaviour
{
    private enum Mode { Transform = 1, Color, Animation, Etc }
    private Mode _mode;
        
    public ObjectSelectionProperty selectedProperty;
    public Action initUI;
    void Start()
    {
        PlayerManager.Instance.leftGrabInteractor.onSelect += GetSelectedObjectInfo;
        PlayerManager.Instance.rightGrabInteractor.onSelect += GetSelectedObjectInfo;
        PlayerManager.Instance.leftRayInteractor.onSelect += GetSelectedObjectInfo;
        PlayerManager.Instance.rightRayInteractor.onSelect += GetSelectedObjectInfo;
    }

    private void GetSelectedObjectInfo(GrabInteractable interactable)
    {
        if (interactable.TryGetComponent(out ObjectSelectionProperty property))
        {
            selectedProperty = property;
            OpenUI();
        }
    }
    
    private void GetSelectedObjectInfo(RayInteractable interactable)
    {
        if (interactable.TryGetComponent(out ObjectSelectionProperty property))
        {
            selectedProperty = property;
            OpenUI();
        }
    }

    private void OpenUI()
    {
        _mode = Mode.Transform;
        initUI.Invoke();
    }

    private void CloseUI()
    {
        selectedProperty = null;
    }

    public void ChangeMode(int index)
    {
        _mode = (Mode)index;
    }
}
