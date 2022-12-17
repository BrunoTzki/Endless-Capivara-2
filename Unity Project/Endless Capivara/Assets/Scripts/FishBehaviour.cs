using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBehaviour : MonoBehaviour
{
    public float fishSpeed;
    private Rigidbody fishrb;

    private void Start()
    {
        fishrb = GetComponent<Rigidbody>();

    }
    private void FixedUpdate()
    {
        fishrb.velocity = Vector3.back * fishSpeed;
    }
}
