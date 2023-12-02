using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EasingButton : MonoBehaviour
{
    [SerializeField] private Transform cubeTransform;
    [SerializeField] private MeshRenderer cubeRenderer; 
    public Ease ease;
    [SerializeField] protected Image buttonImage, panelImage;
    [SerializeField] private Color startColor, endColor;
    private Vector3 _localPosition;
    private Material _material;
    private Sequence _sequence;

    public Action<Ease> onSelect;
    public XRAnimation xrAnimation;
    public Transform selectOutline;
    
    private void Awake()
    {
        _localPosition = cubeTransform.localPosition;
        _material = cubeRenderer.material;
        _sequence = DOTween.Sequence().SetAutoKill(false)
            .OnStart(() =>
            {
                cubeTransform.localPosition = _localPosition;
                _material.color = startColor;
            })
            .AppendInterval(0.3f)
            .Append(cubeTransform.DOLocalMoveX(130f, 1.5f).SetEase(ease))
            .Join(_material.DOColor(endColor, 1.5f).SetEase(ease))
            .AppendInterval(0.3f)
            .SetLoops(-1, LoopType.Restart);
    }

    public void InitButton(XRAnimation newXrAnimation)
    {
        xrAnimation = newXrAnimation;
        _sequence.Restart();
    }

    public void CloseButton()
    {
        _sequence.Pause();
    }

    public void OnHover()
    {
        buttonImage.DOColor(ColorManager.SliderHover, 0.3f);
        panelImage.DOColor(ColorManager.PanelHover, 0.3f);
    }

    public void OnUnhover()
    {
        buttonImage.DOColor(ColorManager.SliderDefault, 0.3f);
        panelImage.DOColor(ColorManager.PanelDefault, 0.3f);
    }

    public void OnSelect()
    {
        buttonImage.DOColor(ColorManager.SliderSelect, 0.3f);
        panelImage.DOColor(ColorManager.PanelSelect, 0.3f);
        
        xrAnimation.SetEase(ease);
        onSelect?.Invoke(ease);
        selectOutline.position = transform.position;
    }
}
