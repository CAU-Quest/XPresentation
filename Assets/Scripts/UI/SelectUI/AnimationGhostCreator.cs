using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationGhostCreator : MonoBehaviour, ISelectedObjectModifierInitializer
{


    public void InitProperty(PresentationObject selectedObject)
    { 
        Debug.Log("Animation Ghost Creator Init");
        XRSelector.Instance.EnableAnimationGhost();


        if (selectedObject is PresentationObject)
        {
            List<SlideObjectData> slideObjectDatas = ((PresentationObject)selectedObject).slideData;
            int index = MainSystem.Instance.currentSlideNum;
            if(index - 1 >= 0 && index - 1 < MainSystem.Instance.GetSlideCount())
                XRSelector.Instance.beforeAnimationGhost.ApplySlideObjectData(slideObjectDatas[MainSystem.Instance.currentSlideNum - 1]);
            else
                XRSelector.Instance.beforeAnimationGhost.SetInvisible();
            if(index + 1 >= 0 && index + 1 < MainSystem.Instance.GetSlideCount())
                XRSelector.Instance.afterAnimationGhost.ApplySlideObjectData(slideObjectDatas[MainSystem.Instance.currentSlideNum + 1]);
            else
                XRSelector.Instance.afterAnimationGhost.SetInvisible();
        }
    }
}
