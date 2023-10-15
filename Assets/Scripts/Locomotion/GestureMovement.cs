using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GestureMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform leftHandAnchor, rightHandAnchor;
    [SerializeField, Range(0f, 5f)] private float moveSpeed;
    
    private Transform _activeHandAnchor;
    private Vector3 _lastHandPos = Vector3.zero;
    
    public void UpdateMovement()
    {
        if (_lastHandPos != Vector3.zero)
        {
            var moveV = (_activeHandAnchor.position - _lastHandPos);
            //moveV = new Vector3(moveV.x, 0f, moveV.z);
            player.position -= moveV * moveSpeed;
        }

        _lastHandPos = _activeHandAnchor.position;
    }

    public void SetProperty(bool isRight)
    {
        _activeHandAnchor = (isRight) ? rightHandAnchor : leftHandAnchor; 
        _lastHandPos = Vector3.zero;
    }
}
