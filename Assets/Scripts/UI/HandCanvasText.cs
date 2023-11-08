using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HandCanvasText : MonoBehaviour, ISystemObserver
{
    public TextMeshProUGUI tmpSlideNum;
    public TextMeshProUGUI tmpMode;

    public GameObject[] icons;

    public void ObserverUpdateMode(int mode)// 0 - Edit Mode, 1 - Deploy Mode, 2 - Slide Mode, 3 - Animation Mode
    {
        tmpMode.text = "";
        switch (mode)
        {
            case 0:
                tmpMode.text += "Edit Mode";
                break;
            case 1:
                tmpMode.text += "Deploy Mode";
                break;
            case 2:
                tmpMode.text += "Slide Mode";
                break;
            case 3:
                tmpMode.text += "Animation Mode";
                break;
        }

        for (int i = 0; i < 4; i++)
        {
            icons[i].SetActive(mode == i);
        }
    }
    
    public void ObserverUpdateSlide(int slide)
    {
        tmpSlideNum.text = "Slide : " + slide;
    }

    public void ObserverRemoveSlide(int index)
    {
    }

    public void ObserverAddSlide()
    {
        
    }

    public void ObserverMoveSlides(int moved, int count, int into)
    {
        
    }

    public void Start()
    {
        MainSystem.Instance.RegisterObserver(this);
    }

}
