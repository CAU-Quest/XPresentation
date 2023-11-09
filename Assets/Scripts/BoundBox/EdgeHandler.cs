using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class EdgeHandler : MonoBehaviour
{
    public GameObject cursor;

    public Vector3 initialLocalPosition;
    public Vector3 initialRotation;

    public Quaternion beforeRotation;  // 이전 회전 상태를 저장하기 위한 변수
    public Quaternion afterRotation;   // 누적 회전값을 저장하기 위한 변수

    public bool isSelected = false;

    // 선택한 축을 나타내는 열거형
    public enum RotationAxis
    {
        X,
        Y,
        Z
    }

    public RotationAxis selectedAxis = RotationAxis.X; // 기본적으로 X 축을 선택

    public void ChangeRotationAxis(RotationAxis rotationAxis)
    {
        selectedAxis = rotationAxis;
    }
    
    private void Start()
    {
        cursor = XRSelector.Instance.Cursor;
        GetComponent<Grabbable>().InjectOptionalTargetTransform(cursor.transform);
        initialLocalPosition = transform.localPosition;
        initialRotation = (transform.position - XRSelector.Instance.transform.position).normalized;
        beforeRotation = XRSelector.Instance.selectedObject.transform.rotation;
        afterRotation = Quaternion.identity;
    }

    public void Select()
    {
        beforeRotation = XRSelector.Instance.selectedObject.transform.rotation;
        cursor = XRSelector.Instance.Cursor;
        cursor.transform.position = transform.position;
        initialLocalPosition = transform.localPosition;
        initialRotation = (cursor.transform.position - XRSelector.Instance.transform.position).normalized;
        isSelected = true;
    }
    public void Unselect()
    {
        beforeRotation = XRSelector.Instance.selectedObject.transform.rotation;
        cursor = XRSelector.Instance.Cursor;
        cursor.transform.position = transform.position;
        initialLocalPosition = transform.localPosition;
        initialRotation = (cursor.transform.position - XRSelector.Instance.transform.position).normalized;
        isSelected = false;
    }

    private void Update()
    {
        if (isSelected)
        {
            // 현재 위치에서 중심까지의 방향을 계산
            Vector3 directionToCenter = (cursor.transform.position - XRSelector.Instance.transform.position).normalized;

            // 회전값 계산
            Quaternion rotation = Quaternion.FromToRotation(initialRotation, directionToCenter);

            afterRotation = rotation * beforeRotation;  // 누적 회전값 업데이트

            if (selectedAxis == RotationAxis.X)
            {
                // X 축 회전만 적용
                XRSelector.Instance.selectedObject.transform.rotation = Quaternion.Euler(afterRotation.eulerAngles.x, beforeRotation.eulerAngles.y, beforeRotation.eulerAngles.z);
            }
            else if (selectedAxis == RotationAxis.Y)
            {
                // Y 축 회전만 적용
                XRSelector.Instance.selectedObject.transform.rotation = Quaternion.Euler(beforeRotation.eulerAngles.x, afterRotation.eulerAngles.y, beforeRotation.eulerAngles.z);
            }
            else if (selectedAxis == RotationAxis.Z)
            {
                // Z 축 회전만 적용
                XRSelector.Instance.selectedObject.transform.rotation = Quaternion.Euler(beforeRotation.eulerAngles.x, beforeRotation.eulerAngles.y, afterRotation.eulerAngles.z);
            }
        }
    }
}
