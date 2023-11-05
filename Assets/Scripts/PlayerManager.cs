using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    
    public Transform player;
    public Transform leftHandAnchor, rightHandAnchor;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }
    
    
}
