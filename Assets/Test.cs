using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class Test : MonoBehaviour
{
    public A[] list1;
    public B[] list2;

    private void Start()
    {
        list1 = GetComponentsInChildren<A>();
        list2 = GetComponentsInChildren<B>();

        Debug.Log("LIST1 : "+list1.Length);
        Debug.Log("LIST2 : "+list2.Length);
    }
}

public interface A
{
    void Hi();
}

public interface B : A
{
    void Bye();
}
