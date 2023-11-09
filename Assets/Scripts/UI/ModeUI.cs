using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using DG.Tweening;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class ModeUI : MonoBehaviour
{
    [SerializeField] private Transform canvas;
    [SerializeField] private Transform sector;
    [SerializeField] private Transform returnButton;
    [SerializeField] private Transform closeButton;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color selectedColor;
    
    [SerializeField] private float _holdTime;
    private bool _isUIOpened;
    private WaitForSeconds _waitForSeconds;

    private int _sectorIndex;
    private Image _returnButtonImage, _closeButtonImage, _sectorImage;

    public enum SelectableAction { CLOSE, DEPLOY, ANIMATION, PREVIEW, RETURN }
    private SelectableAction selectedAction;
    
    private void Awake()
    {
        _waitForSeconds = new WaitForSeconds(0.3f);
        _returnButtonImage = returnButton.GetComponent<Image>();
        _closeButtonImage = closeButton.GetComponent<Image>();
        _sectorImage = sector.GetComponent<Image>();
        _holdTime = 0f;
        StartCoroutine(CloseUI());
    }

    private void Update()
    {
        if (!_isUIOpened && OVRInput.Get(OVRInput.RawButton.Start))
        {
            if (_holdTime > 0.5f)
            {
                OpenUI();
                _isUIOpened = true;
            }
            _holdTime += Time.deltaTime;
        }
        else if (OVRInput.GetUp(OVRInput.RawButton.Start))
        {
            if (_isUIOpened)
            {
                ExecuteSelectedAction();
                StartCoroutine(CloseUI());
                _isUIOpened = false;
            }

            _holdTime = 0f;
        }
    }

    private void ExecuteSelectedAction()
    {
        switch (selectedAction)
        {
            case SelectableAction.DEPLOY:
                break;
            case SelectableAction.PREVIEW:
                break;
            case SelectableAction.ANIMATION:
                break;
            case SelectableAction.RETURN:
                break;
            case SelectableAction.CLOSE:
                break;
        }
    }

    private void OpenUI()
    {
        canvas.gameObject.SetActive(true);
        returnButton.gameObject.SetActive(MainSystem.Instance.mode != MainSystem.Mode.MAIN);
        transform.localScale = new Vector3();
        transform.DOScale(1f, 0.3f);
    }
    
    private IEnumerator CloseUI()
    {
        _sectorIndex = 0;
        transform.DOScale(0f, 0.3f);
        yield return _waitForSeconds;
        canvas.gameObject.SetActive(false);
    }

    public void Select(int action)
    {
        SetColor(selectedAction, false);
        selectedAction = (SelectableAction)action;
        
        switch ((SelectableAction)action)
        {
            case SelectableAction.DEPLOY:
                _sectorIndex = 1;
                break;
            case SelectableAction.ANIMATION:
                _sectorIndex = 2;
                break;
            case SelectableAction.PREVIEW:
                _sectorIndex = 3;
                break;
            case SelectableAction.RETURN:
                _sectorIndex = 0;
                break;
            case SelectableAction.CLOSE:
                _sectorIndex = 0;
                break;
        }
        SetColor((SelectableAction)action, true);
        SetSector(_sectorIndex);
    }

    private void SetColor(SelectableAction action, bool isSelected)
    {
        switch (action)
        {
            case SelectableAction.RETURN:
                _returnButtonImage.DOColor(isSelected ? selectedColor : defaultColor, 0.1f);
                break;
            case SelectableAction.CLOSE:
                _closeButtonImage.DOColor(isSelected ? selectedColor : defaultColor, 0.1f);
                break;
            default:
                _returnButtonImage.DOColor(defaultColor, 0.1f);
                _closeButtonImage.DOColor(defaultColor, 0.1f);
                break;
        }
    }

    private void SetSector(int index)
    {
        var duration = (_sectorImage.color.a == 0f) ? 0f : 0.5f;
        
        if(index == 0) _sectorImage.DOFade(0f, duration);
        else
        {
            sector.DORotate(new Vector3(0f, 0f, 45f - 45f * (index - 1)), duration);
            if(duration == 0) _sectorImage.DOFade(1f, 0.5f);
        }
    }
}