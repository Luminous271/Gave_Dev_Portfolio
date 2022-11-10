using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowDaPlayer : MonoBehaviour
{

    public GameObject player;
    public float interpSpeed;
    private Vector3 targetPos;
    public float targetBack;
    public float verticalOffest;
    void LateUpdate()
    {
        targetPos = new Vector3(player.transform.position.x, player.transform.position.y+verticalOffest, -targetBack);
        transform.position = Vector3.Lerp(transform.position, targetPos, interpSpeed);   
    }                               
}
