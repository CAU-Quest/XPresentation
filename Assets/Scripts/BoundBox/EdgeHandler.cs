using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeHandler : MonoBehaviour
{
    public GameObject cursor;
    
    public Vector3 initialLocalPosition;
    public Vector3 initialRotation;

    public bool isSelected = false;
    private void Start()
    {
        cursor = XRSelector.Instance.Cursor;
        initialLocalPosition = transform.localPosition;
        initialRotation = (transform.position - XRSelector.Instance.transform.position).normalized;
    }

    public void Select()
    {
        cursor = XRSelector.Instance.Cursor;
        cursor.transform.position = transform.position;
        initialLocalPosition = transform.localPosition;
        initialRotation = (cursor.transform.position - XRSelector.Instance.transform.position).normalized;
        isSelected = !isSelected;
    }
    
    
    private void Update()
    {
        if (isSelected)
        {
            // 현재 위치에서 중심까지의 방향을 계산
            Vector3 directionToCenter = (cursor.transform.position - XRSelector.Instance.transform.position).normalized;

            // 회전값 계산
            Quaternion rotation = Quaternion.FromToRotation(initialRotation, directionToCenter);

            XRSelector.Instance.selectedObject.transform.rotation = rotation;
            XRSelector.Instance.transform.rotation = rotation;
        }
    }
}
