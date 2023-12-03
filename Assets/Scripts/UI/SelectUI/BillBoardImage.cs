using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoardImage : MonoBehaviour
{
    private Transform player;

    void Start()
    {
        player = PlayerManager.Instance.eye;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation = Quaternion.LookRotation(player.position - transform.position);
    }
}
