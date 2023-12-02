using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BTypeButton : MonoBehaviour
{
    [SerializeField] protected Image buttonImage;

    public virtual void OnHover()
    {
        buttonImage.DOColor(ColorManager.ToggleUnselectedHover, 0.3f);
    }

    public virtual void OnUnhover()
    {
        buttonImage.DOColor(ColorManager.ToggleUnselected, 0.3f);
    }

    public virtual void OnSelect()
    {
        buttonImage.DOColor(ColorManager.Select, 0.3f);
    }
}
