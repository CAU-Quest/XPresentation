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

    public void ObserverUpdateMode(MainSystem.Mode mode)// 0 - Edit Mode, 1 - Deploy Mode, 2 - Slide Mode, 3 - Animation Mode
    {
        tmpMode.text = "";
        switch (mode)
        {
            case MainSystem.Mode.MAIN:
                tmpMode.text += "Main Mode";
                break;
            case MainSystem.Mode.DEPLOY:
                tmpMode.text += "Deploy Mode";
                break;
            case MainSystem.Mode.PREVIEW:
                tmpMode.text += "PREVIEW Mode";
                break;
            case MainSystem.Mode.ANIMATION:
                tmpMode.text += "Animation Mode";
                break;
        }

        for (int i = 0; i < 4; i++)
        {
            icons[i].SetActive((int)mode == i);
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
