using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuaranaCollider : MonoBehaviour
{
    public float moveDuration = 1f;
    public float speed = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            StartCoroutine(MoveCoinTowardsGuaranaCollider(other.gameObject));
        }
    }

    private IEnumerator MoveCoinTowardsGuaranaCollider(GameObject coin)
    {
        Vector3 startPos = coin.transform.position;
        Vector3 endPos = transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            coin.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / moveDuration);
            yield return null;
        }
    }
}

