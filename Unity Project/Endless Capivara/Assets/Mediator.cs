using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mediator : MonoBehaviour
{

        private GameManager gameManager;

        private void Start()
        {
            gameManager = GameManager.gm;
        }

}
