using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Slide : ISlide
{
    private Dictionary<int, TransformData> transforms = new Dictionary<int, TransformData>(); // id - Transform 딕셔너리, 객체의 ID로부터 Transform 데이터를 받아올 수 있음
    private Dictionary<int, XRIAnimation> animations = new Dictionary<int, XRIAnimation>(); // id - animation 딕셔너리
    
    public void CollectAllObject()
    {
        
    }

    public TransformData GetObjectData(int id)
    {
        if (transforms.ContainsKey(id))
            return transforms[id];
        return new TransformData();
    }

    public XRIAnimation GetAnimation(int id)
    {
        if (animations.ContainsKey(id))
            return animations[id];
        else
            return null;
    }

    public void AddObjectData(int id, TransformData transform)
    {
        if (!transforms.ContainsKey(id))
        {
            transforms.Add(id, transform);
        }
        else
        {
            transforms[id] = transform;
        }
    }
    
    public void AddAnimation(int id, XRIAnimation animation)
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
