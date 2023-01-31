using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderWaterObject : MonoBehaviour
{
    public float UWValue = -2.5f;
    public void UWPosition()
    {
        transform.position = new Vector3(transform.position.x, UWValue, transform.position.z);
    }

}
