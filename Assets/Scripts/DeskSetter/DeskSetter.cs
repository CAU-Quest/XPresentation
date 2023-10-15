using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskSetter : MonoBehaviour
{
    [SerializeField] private Transform grabPoint1, grabPoint2, plane;

    private float _planeOffsetX;

    private void Awake()
    {
        _planeOffsetX = plane.localPosition.x;
    }

    public void Update()
    {
        var grab1Pos = grabPoint1.position;
        plane.position = new Vector3(grab1Pos.x + _planeOffsetX, grab1Pos.y,
            (grabPoint2.position.z + grab1Pos.z) * 0.5f);
    }

    public void DisableThis()
    {
        this.enabled = false;
    }
}
