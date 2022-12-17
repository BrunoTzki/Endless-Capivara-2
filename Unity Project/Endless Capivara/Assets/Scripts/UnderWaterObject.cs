using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderWaterObject : MonoBehaviour
{
    public void UWPosition()
    {
        transform.position = new Vector3(transform.position.x, -2.5f, transform.position.z);
    }

}
