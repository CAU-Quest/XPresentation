using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DeployType
{
    Sphere = 1, 
    Cube = 2, 
    Cylinder = 3, 
    Plane = 4, 
    Text = 5, 
    ImportImage = 6, 
    ImportModel = 7
}
public class SelectNewObjectUI : HoldUI
{

    protected override void Start()
    {
        base.Start();
        Select(0);
    }

    protected override void ExecuteSelectedAction()
    {
        ObjectCreator.Instance.CreateObject((DeployType)selectionIndex);
    }
}
