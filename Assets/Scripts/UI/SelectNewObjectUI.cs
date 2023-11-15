using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectNewObjectUI : HoldUI
{
    private enum SelectableAction { Close, Sphere, Cube, Cylinder, Plane, Text, ImportImage, ImportModel }

    protected override void Start()
    {
        base.Start();
        Select(0);
    }

    protected override void ExecuteSelectedAction()
    {
        switch ((SelectableAction)selectionIndex)
        {
            case SelectableAction.Close:
                break;
            case SelectableAction.Sphere:
                break;
            case SelectableAction.Cube:
                break;
            case SelectableAction.Cylinder:
                break;
            case SelectableAction.Plane:
                break;
            case SelectableAction.Text:
                break;
            case SelectableAction.ImportImage:
                break;
            case SelectableAction.ImportModel:
                break;
        }
    }
}
