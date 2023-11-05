using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMovement : MonoBehaviour
{
    [SerializeField, Range(0f, 5f)] private float moveSpeed;
    private Transform _leftHandAnchor, _rightHandAnchor, _player;
    private Transform _activeHandAnchor;
    private Vector3 _lastPos = Vector3.zero;
    
    private void Start()
    {
        _leftHandAnchor = PlayerManager.Instance.leftHandAnchor;
        _rightHandAnchor = PlayerManager.Instance.rightHandAnchor;
        _player = PlayerManager.Instance.player;
    }
    
    public void UpdateMovement()
    {
        if (_lastPos != Vector3.zero)
        {
            var moveV = (_activeHandAnchor.position - _lastPos);
            //moveV = new Vector3(moveV.x, 0f, moveV.z);
            _player.position -= moveV * moveSpeed;
        }

        _lastPos = _activeHandAnchor.position;
    }

    public void SetPropertyL()
    {
        _activeHandAnchor = _leftHandAnchor; 
        _lastPos = Vector3.zero;
    }
    
    public void SetPropertyR()
    {
        _activeHandAnchor = _rightHandAnchor; 
        _lastPos = Vector3.zero;
    }
}
