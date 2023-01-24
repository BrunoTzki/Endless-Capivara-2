using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    public float distanceMinima;
    public int maxTries;
    public GameObject[] Obstacles;
    public GameObject[] uwObstacles;
    public Vector2 numberOfObstacles;
    public Vector2 numberOfUWObstacles;
    // public Transform ObstacleHolder;

    public GameObject coin;
    public Vector2 numberOfCoins;

    public GameObject[] specials;
    public Vector2 numberOfSpecials;

    public List<GameObject> newObstacles;
    public List<GameObject> newUWObstacles;
    public List<GameObject> newCoins;
    public List<GameObject> newSpecials;

    // Start is called before the first frame update
    void Start()
    {
        int newNumberOfObstacles = (int)Random.Range(numberOfObstacles.x, numberOfObstacles.y);
        int newNumberOfUWObstacles = (int)Random.Range(numberOfUWObstacles.x, numberOfUWObstacles.y);
        int newNumberOfCoins = (int)Random.Range(numberOfCoins.x, numberOfCoins.y);
        int newNumberOfSpecials = (int)Random.Range(numberOfSpecials.x, numberOfSpecials.y);

        //Over Water
        int countObstacle1 = 0;
        for (int i = 0; i < newNumberOfObstacles; i++)
        {
            GameObject obstacle = Obstacles[Random.Range(0, Obstacles.Length)];
            if (obstacle.name == "3troncos" && countObstacle1 <= 1)
            {
                newObstacles.Add(Instantiate(obstacle, transform));
                newObstacles[i].SetActive(false);
                countObstacle1++;
            }
            else if (obstacle.name == "3troncos" && countObstacle1 > 1)
            {
                i--; //decrementa i para não contar como objeto instanciado
            }
            else
            {
                newObstacles.Add(Instantiate(obstacle, transform));
                newObstacles[i].SetActive(false);
            }
        }

        //Under Water
        for (int i = 0; i < newNumberOfUWObstacles; i++)
        {
            newUWObstacles.Add(Instantiate(uwObstacles[Random.Range(0, uwObstacles.Length)], transform));
            newUWObstacles[i].SetActive(false);
        }

        for (int i = 0; i < newNumberOfCoins; i++)
        {
            newCoins.Add(Instantiate(coin, transform));
            newCoins[i].SetActive(false);
        }

        for (int i = 0; i < newNumberOfSpecials; i++)
        {
            newSpecials.Add(Instantiate(specials[Random.Range(0, specials.Length)], transform));
            newSpecials[i].SetActive(false);
        }

        PositionateObstacles();
        PositionateUWObstacles();
        PositionateCoins();
        PositionateSpecials();

    }

    void PositionateObstacles()
    {
        for (int i = 0; i < newObstacles.Count; i++)
        {
            float posZmin = (200f / newObstacles.Count) + (200f / newObstacles.Count) * i;
            float posZmax = (200f / newObstacles.Count) + (200f / newObstacles.Count) * i + 1;
            newObstacles[i].transform.localPosition = new Vector3(0, 0, Random.Range(posZmin, posZmax));
            newObstacles[i].SetActive(true);

            if (newObstacles[i].GetComponent<ChangeLane>() != null)
                newObstacles[i].GetComponent<ChangeLane>().PositionLane();
        }
    }

    void PositionateUWObstacles()
    {
        for (int i = 0; i < newUWObstacles.Count; i++)
        {
            float UWposZmin = (200f / newUWObstacles.Count) + (200f / newUWObstacles.Count) * i;
            float UWposZmax = (200f / newUWObstacles.Count) + (200f / newUWObstacles.Count) * i + 1;
            newUWObstacles[i].transform.localPosition = new Vector3(0, 0, Random.Range(UWposZmin, UWposZmax));
            newUWObstacles[i].SetActive(true);

            if (newUWObstacles[i].GetComponent<UnderWaterObject>() != null)
                newUWObstacles[i].GetComponent<UnderWaterObject>().UWPosition();

            if (newUWObstacles[i].GetComponent<ChangeLane>() != null)
                newUWObstacles[i].GetComponent<ChangeLane>().PositionLane();

        }
    }

    void PositionateCoins()
    {
        float minZpos = 13f;

        for (int i = 0; i < newCoins.Count; i++)
        {
            int randomLane = Random.Range(-1, 2);
            int randomLaneY = Random.Range(-1, 1);
            float maxZpos = minZpos + 13f;
            float randomZpos = Random.Range(minZpos, maxZpos);
            Vector3 newPos = new Vector3(randomLane, randomLaneY*2.2f, randomZpos);
            Collider[] colliders;
            int cont = 0;
            do
            {
                colliders = Physics.OverlapSphere(newPos, distanceMinima);
                if (colliders.Length > 0)
                {
                    Debug.Log("Colidiu");
                    randomLane = Random.Range(-1, 2);
                    randomLaneY = Random.Range(-1, 1);
                    newPos = new Vector3(randomLane, randomLaneY*2.2f, randomZpos);
                    newCoins[i].transform.localPosition = newPos;
                    cont++;
                    Debug.Log("Mudou de posição x" + cont);

                }
            } while (colliders.Length > 0 && cont <= maxTries);

            if (cont > maxTries)
            {
                Debug.Log("Desistiu de procurar um novo lugar");
                continue;
                // aqui você pode tratar o caso de não conseguir gerar uma posição válida
            }
            newCoins[i].transform.localPosition = newPos;
            minZpos = randomZpos + 1;
            newCoins[i].SetActive(true);
        }
    }
    void PositionateSpecials()
    {
        float minZpos = 20f;

        for (int i = 0; i < newSpecials.Count; i++)
        {
            float maxZpos = minZpos + 50f;
            float randomZpos = Random.Range(minZpos, maxZpos);
            newSpecials[i].transform.localPosition = new Vector3(transform.position.x, transform.position.y, randomZpos);
            newSpecials[i].SetActive(true);
            newSpecials[i].GetComponent<ChangeLaneSpecials>().PositionLaneSpecials();
            minZpos = randomZpos + 50;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().IncreaseSpeed();
            transform.position = new Vector3(0, 0, transform.position.z + 200 * 2);

            PositionateObstacles();
            PositionateCoins();
            PositionateUWObstacles();
            PositionateSpecials();
        }
    }

}
