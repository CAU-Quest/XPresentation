using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelectionProperty : MonoBehaviour
{
    public Transform Transform => transform;

    public Material Material => _renderer.material;

    //animation ...
    public bool grabbableInPresentation;

    private MeshRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }
}
