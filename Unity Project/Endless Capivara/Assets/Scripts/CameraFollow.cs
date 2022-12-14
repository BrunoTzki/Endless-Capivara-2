using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public bool submerged = true;
    public float submergedCam;
    public float notSubmergedCam;
    public int smothness;


    private Transform player;
    private Vector3 offset;
    private Vector3 verticalTargetPosition;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        offset = transform.position - player.position;
    }

    public void CameraSubmerge()
    {

    }

    void LateUpdate()
    {


        if (submerged)
        {
            float currentYposition = transform.position.y;
            verticalTargetPosition.y = Mathf.MoveTowards(currentYposition, submergedCam,  smothness * Time.deltaTime);
            Vector3 newPosition = new Vector3(transform.position.x, verticalTargetPosition.y, player.position.z + offset.z);
            transform.position = newPosition;
        }
        else
        {
            float currentYposition = transform.position.y;
            verticalTargetPosition.y = Mathf.MoveTowards(currentYposition, notSubmergedCam, smothness * Time.deltaTime);
            Vector3 newPosition = new Vector3(transform.position.x, verticalTargetPosition.y, player.position.z + offset.z);
            transform.position = newPosition;
        }

    }
}
