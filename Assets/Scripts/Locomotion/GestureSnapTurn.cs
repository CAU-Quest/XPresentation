using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureSnapTurn : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform leftHandAnchor, rightHandAnchor;
    [SerializeField] private OVRCameraRig cameraRig;
    [SerializeField] private int handRotationAngle = 30;
    [SerializeField] private int rotationAngle = 45;

    private bool _isFirst;
    private Vector3 _initialLeftHandPos, _initialRightHandPos;
    private int _lastAngleState = 0;
    private Vector2 _lastPair;

    public void SnapTurn()
    {
        if (_isFirst) SetInitialHandPosition();
        
        var angle = CalculateAngle();
        var angleState = (int)angle / handRotationAngle;
        
        if (_lastAngleState != angleState 
            && _lastPair != new Vector2(_lastAngleState, angleState)
            && _lastPair != new Vector2(angleState, _lastAngleState))
        {
            //Debug.Log("currentAngleQ : " + _lastAngleState +", angleQ : " +angleState+", angle : " +angle);
            var isClockwise = _lastAngleState < angleState;
            //Turn(isClockwise);

            _lastPair = new Vector2(_lastAngleState, angleState);
        }
        
        _lastAngleState = angleState;
    }

    private void SetInitialHandPosition()
    {
        _initialLeftHandPos = leftHandAnchor.localPosition;
        _initialRightHandPos = rightHandAnchor.localPosition;
        _isFirst = false;
    }

    private float CalculateAngle()
    {
        var initialV = _initialRightHandPos - _initialLeftHandPos;
        var currentV = rightHandAnchor.localPosition - leftHandAnchor.localPosition;

        var sign = (Vector3.Dot(currentV - initialV, player.forward) > 0) ? 1 : -1;
        Debug.Log("Sign :" +sign);
        return sign * Vector3.Angle(initialV, currentV);
    }

    private void Turn(bool isClockwise)
    {
        var angle = (isClockwise) ? rotationAngle : -rotationAngle;
        player.RotateAround(cameraRig.centerEyeAnchor.position, Vector3.up, angle);
    }

    public void ResetProperty()
    {
        _isFirst = true;
        _initialLeftHandPos = Vector3.zero;
        _initialRightHandPos = Vector3.zero;
        _lastAngleState = 0;
        _lastPair = Vector2.zero;
    }
}
