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
        var leftHandPos = _leftHandAnchor.localPosition;
        var rightHandPos = _rightHandAnchor.localPosition;
        
        if (_isFirst)
        {
            _lastLeftHandPos = leftHandPos;
            _lastRightHandPos = rightHandPos;
            _isFirst = false;
            return;
        }
        
        var lastHandV = _lastRightHandPos - _lastLeftHandPos;
        lastHandV = new Vector3(lastHandV.x, 0f, lastHandV.z);
        var handV = rightHandPos - leftHandPos;
        handV = new Vector3(handV.x, 0f, handV.z);
        
        var angle = Vector3.Angle(handV, lastHandV);
        var forwardV = Quaternion.Euler(0f, -90f, 0f) * lastHandV;
        var sign = (Vector3.Dot(handV, forwardV) > 0f) ? 1f : -1f;
        
        var middlePos = (_leftHandAnchor.position + _rightHandAnchor.position) * 0.5f;
        _player.RotateAround(middlePos, Vector3.up, angle * sign);

        _lastLeftHandPos = leftHandPos;
        _lastRightHandPos = rightHandPos;
    }
    public void ResetProperty()
    {
        _isFirst = true;
    }
}
