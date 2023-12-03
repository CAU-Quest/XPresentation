using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BTypeToggleButton : MonoBehaviour
{
    [SerializeField] protected Image buttonImage;
    [SerializeField] private Image icon;
    [SerializeField] private Sprite whenOn, whenOff;
    public GameObject canvas;
    
    protected bool isOn;

    public virtual void OnHover()
    {
        buttonImage.DOColor(ColorManager.Hover, 0.3f);
    }

    public virtual void OnUnhover()
    {
        buttonImage.DOColor((isOn) ? ColorManager.ToggleSelected : ColorManager.Default, 0.3f);
    }
    
    public virtual void OnSelect()
    {
        buttonImage.DOColor(ColorManager.ToggleSelect, 0.3f);
        isOn = !isOn;
        canvas.SetActive(isOn);
        ToggleSprite();
    }

    protected void ToggleSprite()
    {
        icon.sprite = (isOn) ? whenOn : whenOff;
    }

    public void SetOff()
    {
        isOn = false;
        ToggleSprite();
        OnUnhover();
    }
}
