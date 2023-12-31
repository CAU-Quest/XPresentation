using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class XRUIManager : MonoBehaviour
{
    public static XRUIManager Instance = null;


    public float distance = 0.5f;
    public float verticalOffset = -0.5f;

    [SerializeField] public RayInteractor leftRayInteractor;
    [SerializeField] public RayInteractor rightRayInteractor;

    public GameObject fileBrowser;
    
    public Transform positionSource;
    void Start()
    {
        if (null == Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public Pose GetPlayerSightPose()
    {
        Vector3 direction = positionSource.forward;
        direction.y = 0;
        direction.Normalize();

        Vector3 targetPosition = positionSource.position + direction * distance + Vector3.up * verticalOffset;
        Quaternion rotation = Quaternion.Euler(direction);

        return new Pose(targetPosition, rotation);
    }

}
