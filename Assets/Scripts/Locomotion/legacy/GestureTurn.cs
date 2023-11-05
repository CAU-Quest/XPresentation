using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureTurn : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform leftHandAnchor, rightHandAnchor;

    private Vector3 _lastLeftHandPos, _lastRightHandPos;
    private bool _isFirst;
    
    public void UpdateTurn()
    {
        if (_isFirst)
        {
            _lastLeftHandPos = Vector3.zero;
            _lastRightHandPos = Vector3.zero;
            _isFirst = false;
            return;
        }

        var leftHandPos = leftHandAnchor.localPosition;
        var rightHandPos = rightHandAnchor.localPosition;
        var middlePos = (leftHandPos + rightHandPos) * 0.5f;
        
        var lastHandV = _lastRightHandPos - _lastLeftHandPos;
        lastHandV = new Vector3(lastHandV.x, 0f, lastHandV.z);
        var handV = rightHandPos - leftHandPos;
        handV = new Vector3(handV.x, 0f, handV.z);

        var angle = Vector3.Angle(handV, lastHandV);
        var sign = (leftHandPos.z > _lastLeftHandPos.z) ? -1 : 1;
        
        player.RotateAround(middlePos, Vector3.up, angle * sign);

        _lastLeftHandPos = leftHandPos;
        _lastRightHandPos = rightHandPos;
    }
    
    public void ResetProperty()
    {
        _isFirst = true;
    }
}
