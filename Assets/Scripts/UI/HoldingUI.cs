using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public abstract class HoldingUI : MonoBehaviour
{
    [SerializeField] private OVRInput.Button triggerButton;
    [SerializeField] private float rotationAngle;

    [SerializeField] private Transform uiCanvas, progressCanvas;
    [SerializeField] protected Transform sector;
    
    [SerializeField] private Transform closeButton;
    [SerializeField] protected Transform[] sectorButtons;
    [SerializeField] protected TextMeshProUGUI[] sectorButtonTexts;
    
    [SerializeField] private Slider slider;
    [SerializeField] protected Color defaultColor;
    [SerializeField] protected Color selectedColor;
    
    private float _holdTime;
    [SerializeField] private bool _isUIOpened, _isStartToOpen;
    private Image _closeButtonImage;
    private WaitForSeconds _waitForSeconds;
    
    protected int selectionIndex;
    
    private void Awake()
    {
        _waitForSeconds = new WaitForSeconds(0.3f);
        _closeButtonImage = closeButton.GetComponent<Image>();
        _holdTime = 0f;
        
        StartCoroutine(CloseUI());
    }

    private void Update()
    {
        if (!_isUIOpened && OVRInput.GetDown(triggerButton))
        {
            transform.position = PlayerManager.Instance.leftTip.position;
            transform.LookAt(transform.position * 2 - PlayerManager.Instance.eye.position);
            progressCanvas.gameObject.SetActive(true);
            StartCoroutine(PlayerManager.Instance.VibrateController(0.2f, 1f, 1f, OVRInput.Controller.LTouch));
            _isStartToOpen = true;
        }
        if (_isStartToOpen && !_isUIOpened && OVRInput.Get(triggerButton))
        {
            if (_holdTime > 0.5f)
            {
                OpenUI();
                _isUIOpened = true;
            }
            slider.value = _holdTime / 0.5f;
            _holdTime += Time.deltaTime;
        }
        if (OVRInput.GetUp(triggerButton))
        {
            if (_isUIOpened)
            {
                ExecuteSelectedAction();
                StartCoroutine(CloseUI());
            }
            else
            {
                _isStartToOpen = false;
            }
            _holdTime = 0f;
            slider.value = 0f;
        }
    }

    protected virtual void OpenUI()
    {
        uiCanvas.DOKill();
        uiCanvas.localScale = new Vector3();
        uiCanvas.DOScale(0.0003f, 0.3f).SetEase(Ease.OutCirc);
        
        progressCanvas.gameObject.SetActive(false);
        uiCanvas.gameObject.SetActive(true);
        slider.value = 0f;
    }
    
    private IEnumerator CloseUI()
    {
        selectionIndex = 0;
        uiCanvas.DOKill();
        uiCanvas.DOScale(0f, 0.3f).SetEase(Ease.InCirc);
        
        yield return _waitForSeconds;
        
        uiCanvas.gameObject.SetActive(false);
        progressCanvas.gameObject.SetActive(false);
        _isUIOpened = false;
        _isStartToOpen = false;
    }

    protected abstract void ExecuteSelectedAction();
    
    public void Select(int index)
    {
        if (selectionIndex == index) return;
        
        selectionIndex = index;
        UnselectExcept(selectionIndex);

        if (selectionIndex == 0) SelectButton(true);
        else
        {
            for (int i = 1; i <= sectorButtons.Length; i++)
            {
                if (selectionIndex == i) SelectSector(i, true);
            }
        }
    }
    
    private void UnselectExcept(int index)
    {
        if (index != 0) SelectButton(false);
        for (int i = 1; i <= sectorButtons.Length; i++)
        {
            if (index != i) SelectSector(i, false);
        }
    }
    
    protected void SelectSector(int index, bool isTrue)
    {
        if(isTrue) sector.DOLocalRotate(new Vector3(0f, 0f,  -rotationAngle * (index - 1)), 0.3f).SetEase(Ease.OutBack);
        sectorButtons[index - 1].DOScale(isTrue ? 1.1f : 0.9f, 0.3f);
        sectorButtonTexts[index - 1].DOFade(isTrue ? 1f : 0f, 0.3f);
    }

    protected virtual void SelectButton(bool isTrue) //true = select, false = unselect
    {
        _closeButtonImage.DOColor(isTrue ? selectedColor : defaultColor, 0.1f);
    }
}
