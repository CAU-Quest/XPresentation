using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class EasingButton : MyButton
{
    [SerializeField] private Transform cubeTransform;
    [SerializeField] private MeshRenderer cubeRenderer;
    [SerializeField] private Ease ease;
    [SerializeField] private Color startColor, endColor;
    private Vector3 _localPosition;
    private Material _material;
    private Sequence _sequence;

    private void Awake()
    {
        _localPosition = cubeTransform.localPosition;
        _material = cubeRenderer.material;
        _sequence = DOTween.Sequence().SetAutoKill(false)
            .OnStart(() =>
            {
                cubeTransform.localPosition = _localPosition;
                _material.color = startColor;
            }).Append(cubeTransform.DOLocalMoveX(130f, 1f).SetEase(ease))
            .Join(_material.DOColor(endColor, 1f).SetEase(ease))
            .AppendInterval(0.3f)
            .SetLoops(-1, LoopType.Restart);
    }

    private void OnEnable()
    {
        _sequence.Play();
    }

    private void OnDisable()
    {
        _sequence.Kill();
    }

    public override void OnSelect()
    {
        base.OnSelect();
        
    }
}
