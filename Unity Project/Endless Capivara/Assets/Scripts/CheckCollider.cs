using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollider : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Coin em cima de Obstáculo");
        }
        if (other.CompareTag("Player"))
        {
            Debug.Log("Coin coletada");
        }
    }
}
