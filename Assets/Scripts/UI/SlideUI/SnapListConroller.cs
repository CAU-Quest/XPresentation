using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEditor;
using UnityEngine;

public class SnapListConroller : MonoBehaviour
{
    [SerializeField]
    private SlideListSnapPoseDelegate listSnapPoseDelegate;

    private SnapInteractable snapInteractable;

    public GameObject previewCube;

    public List<PreviewCube> previewCubeList = new List<PreviewCube>();
    
    // Start is called before the first frame update
    void Start()
    {
        //listSnapPoseDelegate = GetComponentInChildren<SlideListSnapPoseDelegate>();
        snapInteractable = GetComponentInChildren<SnapInteractable>();
    }

    void Update()
    {
        previewCubeList.Sort((PreviewCube p1, PreviewCube p2) => p1.transform.localPosition.x.CompareTo(p2.transform.localPosition.x));
        for (int i = 0; i < 7; i++)
        {
            previewCubeList[i].SetNumber(i + 1);
        }
    }
}
