using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerTurn : MonoBehaviour
{
    private Transform _leftHandAnchor, _rightHandAnchor, _player;
    private Vector3 _lastLeftHandPos, _lastRightHandPos;
    private bool _isFirst;

    private void Start()
    {
        _leftHandAnchor = PlayerManager.Instance.leftHandAnchor;
        _rightHandAnchor = PlayerManager.Instance.rightHandAnchor;
        _player = PlayerManager.Instance.player;
    }

    public void UpdateTurn()
    {
        if (_isFirst)
        {
            _lastLeftHandPos = Vector3.zero;
            _lastRightHandPos = Vector3.zero;
            _isFirst = false;
            return;
        }

        var leftHandPos = _leftHandAnchor.localPosition;
        var rightHandPos = _rightHandAnchor.localPosition;
        var middlePos = (leftHandPos + rightHandPos) * 0.5f;
        
        var lastHandV = _lastRightHandPos - _lastLeftHandPos;
        lastHandV = new Vector3(lastHandV.x, 0f, lastHandV.z);
        var handV = rightHandPos - leftHandPos;
        handV = new Vector3(handV.x, 0f, handV.z);

        var angle = Vector3.Angle(handV, lastHandV);
        var sign = (leftHandPos.z > _lastLeftHandPos.z) ? -1 : 1;
        
        _player.RotateAround(middlePos, Vector3.up, angle * sign);

        _lastLeftHandPos = leftHandPos;
        _lastRightHandPos = rightHandPos;
    }
    
    public void ResetProperty()
    {
        _isFirst = true;
    }
}
