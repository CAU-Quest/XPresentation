using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideUICollider : MonoBehaviour
{
    public bool isLeft;
    public SnapListController snapListController;
    public float cooldown = 0f;
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("STAY");
        if(snapListController.selectingPreviewCube == null) return;
        if (other.gameObject.name == "PreviewCube")
        {
            cooldown += Time.deltaTime;
            if (cooldown > 0.5f)
            {
                cooldown = 0f;
                if (isLeft)
                {
                    snapListController.SwipeToLeft();
                }
                else
                {
                    snapListController.SwipeToRight();
                }

            }
        }
    }
}
