using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using DG.Tweening;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class ModeUI : HoldUI
{
    private enum SelectableAction { Close, Edit, Animation, Preview }

    protected override void Start()
    {
        base.Start();
        Select((int)MainSystem.Instance.mode);
    }

    protected override void ExecuteSelectedAction()
    {
        switch ((SelectableAction)selectionIndex)
        {
            case SelectableAction.Close:
                break;
            case SelectableAction.Edit:
                break;
            case SelectableAction.Preview:
                break;
            case SelectableAction.Animation:
                break;
        }
    }

    protected override void SelectButton(bool isTrue) //true = select, false = unselect
    {
        base.SelectButton(isTrue);
        SelectSector((int)MainSystem.Instance.mode, true);
    }
}