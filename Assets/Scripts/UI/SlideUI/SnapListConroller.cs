using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEditor;
using UnityEngine;

public class SnapListConroller : MonoBehaviour
{
    private SlideListSnapPoseDelegate listSnapPoseDelegate;

    private SnapInteractable snapInteractable;

    public GameObject previewCube;

    public PreviewCube[] previewCubeList;
    
    // Start is called before the first frame update
    void Start()
    {
        listSnapPoseDelegate = GetComponentInChildren<SlideListSnapPoseDelegate>();
        snapInteractable = GetComponentInChildren<SnapInteractable>();

        for (int i = 0; i < 7; i++)
        {
            GameObject go = PrefabUtility.InstantiatePrefab(previewCube) as GameObject;
            go.transform.SetParent(transform.parent);
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;

            PreviewCube cube = go.GetComponent<PreviewCube>();
            listSnapPoseDelegate.TrackElement(cube.snapInteractor.Identifier, new Pose(Vector3.zero, Quaternion.identity));
            
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
