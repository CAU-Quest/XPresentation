using System;
using System.Collections.Generic;
using UnityEngine;

public class CenterChildPosition : MonoBehaviour
{

    public struct ChildT
    {
        public Vector3 position;
        public int index;

        public ChildT(Vector3 pos, int idx)
        {
            this.position = pos;
            this.index = idx;
        }
        
        public override bool Equals(object obj)
        {
            if (!(obj is ChildT))
            {
                return false;
            }

            ChildT other = (ChildT)obj;

            return index == other.index && position == other.position;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(index, position);
        }
    };
    
    public List<ChildT> childTransforms;

    private void Start()
    {
        SetChildTransform();
        CenterChildPositions();
    }

    private void SetChildTransform()
    {
        childTransforms = new List<ChildT>();
        for (int i = 0; i < transform.childCount; i++)
        {
            ChildT element = new ChildT(transform.GetChild(i).position, i);
            if (transform.GetChild(i).gameObject.layer == 7 && !childTransforms.Contains(element))
            {
                childTransforms.Add(element);
            }
        }
    }

    public void CenterChildPositions()
    {
        if (childTransforms.Count == 0) return;
        Vector3 centerPosition = Vector3.zero;

        // 모든 자식 객체의 위치를 합산합니다.
        for(int i = 0; i < childTransforms.Count; i++)
        {
            centerPosition += childTransforms[i].position;
        }

        // 중심 위치로 이동시킵니다.
        centerPosition /= childTransforms.Count;
        transform.position = centerPosition;

        // 자식 객체들의 로컬 위치를 중심 위치에 따라 조정합니다.
        for(int i = 0; i < childTransforms.Count; i++)
        {
            transform.GetChild(childTransforms[i].index).position = childTransforms[i].position;
        }
        
        SetChildTransform();
    }

    public void EditorCenterChildPositions()
    {
        SetChildTransform();
        CenterChildPositions();
    }
    
}
