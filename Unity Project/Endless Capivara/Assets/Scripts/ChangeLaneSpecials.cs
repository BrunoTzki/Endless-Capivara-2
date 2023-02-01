using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLaneSpecials : MonoBehaviour
{
    public void PositionLaneSpecials()
    {
        int randomLaneX = Random.Range(-1, 2);
        int randomLaneY = Random.Range(-1, 2);
        transform.position = new Vector3(randomLaneX, randomLaneY*2.2f, transform.position.z);
    }
}
