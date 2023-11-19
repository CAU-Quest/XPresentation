using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Slide : ISlide
{
    private Dictionary<uint, SlideObjectData> slideObjectDatas = new Dictionary<uint, SlideObjectData>(); // id - Transform 딕셔너리, 객체의 ID로부터 Transform 데이터를 받아올 수 있음
    private Dictionary<uint, XRIAnimation> animations = new Dictionary<uint, XRIAnimation>(); // id - animation 딕셔너리
    
    public void CollectAllObject()
    {
        
    }

    public SlideObjectData GetObjectData(uint id)
    {
        if (slideObjectDatas.ContainsKey(id))
            return slideObjectDatas[id];
        return new SlideObjectData();
    }

    public XRIAnimation GetAnimation(uint id)
    {
        if (animations.ContainsKey(id))
            return animations[id];
        else
            return null;
    }

    public void AddObjectData(uint id, SlideObjectData transform)
    {
        if (!slideObjectDatas.ContainsKey(id))
        {
            slideObjectDatas.Add(id, transform);
        }
        else
        {
            slideObjectDatas[id] = transform;
        }
    }
    
    public void AddAnimation(uint id, XRIAnimation animation)
    {
        if (!animations.ContainsKey(id))
        {
            animations.Add(id, animation);
        }
        else
        {
            animations[id] = animation;
        }
    }
}
