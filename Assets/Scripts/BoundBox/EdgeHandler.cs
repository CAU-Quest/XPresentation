using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class EdgeHandler : MonoBehaviour
{
    [SerializeField]
    private Grabbable grabbable;
    [SerializeField]
    private OneGrabRotateTransformer oneGrabRotateTransformer;

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

    public void EdgeSelect()
    {
        XRSelector.Instance.edgeSelected = true;
    }
    public void EdgeUnselect()
    {
        XRSelector.Instance.edgeSelected = false;
    }
}
