using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Oculus.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectUI : MonoBehaviour
{
    private enum Mode { Transform = 1, Color, Animation, Etc }
    private Mode _mode;

    [SerializeField] private GameObject[] canvases;
    [SerializeField] private Image[] buttons;
    [SerializeField] private Image[] labelPanels;
    [SerializeField] private TextMeshProUGUI[] labelTexts;
    [SerializeField] private Color defaultColor, selectColor;
        
    public ObjectSelectionProperty selectedProperty;
    public Action initUI;

    void Start()
    {
        PlayerManager.Instance.leftGrabInteractor.onSelect += GetSelectedObjectInfo;
        PlayerManager.Instance.rightGrabInteractor.onSelect += GetSelectedObjectInfo;
        PlayerManager.Instance.leftRayInteractor.onSelect += GetSelectedObjectInfo;
        PlayerManager.Instance.rightRayInteractor.onSelect += GetSelectedObjectInfo;
        
        CloseUI();
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
        canvases[0].SetActive(true);
        Select(Mode.Transform);
        initUI.Invoke();
    }

    private void CloseUI()
    {
        for (int i = 0; i < canvases.Length; i++)
        {
            canvases[i].SetActive(false);
        }
        
        selectedProperty = null;
    }

    public void ChangeMode(int index)
    {
        Select((Mode)index);
    }

    private void Select(Mode mode)
    {
        _mode = mode;
        var modeIndex = (int)mode - 1;
        for (int i = 0; i < buttons.Length; i++)
        {
            canvases[i + 1].SetActive(i == modeIndex);
            buttons[i].transform.DOScale((i == modeIndex) ? 1f : 0.8f, 0.3f);
            buttons[i].DOColor((i == modeIndex) ? selectColor : defaultColor, 0.3f);
            labelPanels[i].DOFade((i == modeIndex) ? 1f : 0f, 0.3f);
            labelTexts[i].DOFade((i == modeIndex) ? 1f : 0f, 0.3f);
        }
    }
}
