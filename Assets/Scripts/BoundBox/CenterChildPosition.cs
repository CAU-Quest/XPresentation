using UnityEngine;

public class CenterChildPosition : MonoBehaviour
{
    public Vector3[] childTransforms;

    private void Start()
    {
        // 자식 객체들의 Transform 컴포넌트를 가져옵니다.
        SetChildTransform();

        // 모든 자식의 위치를 중심 위치로 이동시킵니다.
        CenterChildPositions();
    }

    private void SetChildTransform()
    {
        // 자식 객체들의 Transform 컴포넌트를 가져옵니다.
        childTransforms = new Vector3[transform.childCount - 12];
        for (int i = 0; i < transform.childCount - 12; i++)
        {
            childTransforms[i] = transform.GetChild(i).position;
        }
    }

    public void CenterChildPositions()
    {
        Vector3 centerPosition = Vector3.zero;

        // 모든 자식 객체의 위치를 합산합니다.
        foreach (Vector3 childTransform in childTransforms)
        {
            centerPosition += childTransform;
        }

        // 중심 위치로 이동시킵니다.
        centerPosition /= childTransforms.Length;
        transform.position = centerPosition;

        // 자식 객체들의 로컬 위치를 중심 위치에 따라 조정합니다.
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).position = childTransforms[i];
        }
        
        SetChildTransform();
    }

    public void EditorCenterChildPositions()
    {
        SetChildTransform();
        CenterChildPositions();
    }
    
}
