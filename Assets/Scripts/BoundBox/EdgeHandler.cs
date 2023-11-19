using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Oculus.Interaction;
using UnityEngine;

public class EdgeHandler : MonoBehaviour
{
    [SerializeField]
    private Grabbable grabbable;
    [SerializeField]
    private OneGrabRotateTransformer oneGrabRotateTransformer;

    private MeshRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        Start();
    }

    private void OnValidate()
    {
        Start();
    }

    public void Start()
    {
        if(grabbable == null) grabbable = GetComponent<Grabbable>();
        if(oneGrabRotateTransformer == null) oneGrabRotateTransformer = GetComponent<OneGrabRotateTransformer>();
    }

    public void Init(OneGrabRotateTransformer.Axis axis, Transform center)
    {
        grabbable.InjectOptionalTargetTransform(center);
        oneGrabRotateTransformer.SetRotationAxis(axis);
    }

    public void OnHover()
    {
        _renderer.material.DOKill();
        _renderer.material.DOColor(ColorManager.BoundBoxEdgeHover, 0.2f);
    }

    public void OnUnhover()
    {
        _renderer.material.DOKill();
        _renderer.material.DOColor(ColorManager.BoundBoxEdgeDefault, 0.2f);
    }

    public void OnSelect()
    {
        XRSelector.Instance.edgeSelected = true;
        _renderer.material.DOKill();
        _renderer.material.DOColor(ColorManager.BoundBoxEdgeSelect, 0.2f);
    }
    public void OnUnselect()
    {
        XRSelector.Instance.edgeSelected = false;
        XRSelector.Instance.presentationObject.SaveTransformToSlide();
        _renderer.material.DOKill();
        _renderer.material.DOColor(ColorManager.BoundBoxEdgeHover, 0.2f);
    }
}
