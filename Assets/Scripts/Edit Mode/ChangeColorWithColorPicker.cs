using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorWithColorPicker : MonoBehaviour
{
    private Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }

}
