using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectableAction { Close, Sphere, Cube, Cylinder, Plane, Text, ImportImage, ImportModel }
public class SelectNewObjectUI : HoldUI
{

    protected override void Start()
    {
        base.Start();
        Select(0);
    }

    protected override void ExecuteSelectedAction()
    {
        ObjectCreator.Instance.CreateObject((SelectableAction)selectionIndex);
    }
}
