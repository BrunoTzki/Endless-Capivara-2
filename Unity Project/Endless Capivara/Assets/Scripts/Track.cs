using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    public GameObject[] Obstacles;
    public GameObject[] uwObstacles;
    public Vector2 numberOfObstacles;
    public Vector2 numberOfUWObstacles;
    public Transform ObstacleHolder;

    public GameObject coin;
    public Vector2 numberOfCoins;

    public List<GameObject> newObstacles;
    public List<GameObject> newUWObstacles;
    public List<GameObject> newCoins;
    // Start is called before the first frame update
    void Start()
    {
        int newNumberOfObstacles = (int)Random.Range(numberOfObstacles.x, numberOfObstacles.y);
        int newNumberOfUWObstacles = (int)Random.Range(numberOfUWObstacles.x, numberOfUWObstacles.y);
        int newNumberOfCoins = (int)Random.Range(numberOfCoins.x, numberOfCoins.y);

        //Over Water
        for (int i = 0; i < newNumberOfObstacles; i++)
        {
            newObstacles.Add(Instantiate(Obstacles[Random.Range(0, Obstacles.Length)], transform));
            newObstacles[i].SetActive(false);
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

        PositionateObstacles();
        PositionateCoins();
        PositionateUWObstacles();

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
            float minZpos = 10f;

        for (int i = 0; i < newCoins.Count; i++)
        {
            float maxZpos = minZpos + 15f;
            float randomZpos = Random.Range(minZpos, maxZpos);
            newCoins[i].transform.localPosition = new Vector3(transform.position.x, transform.position.y, randomZpos);
            newCoins[i].SetActive(true);
            newCoins[i].GetComponent<ChangeLane>().PositionLane();
            minZpos = randomZpos + 1;
        }
    }
    void RepositionateCoins(int i, float minZpos)
    {
        float maxZpos = minZpos + 15f;
        float randomZpos = Random.Range(minZpos, maxZpos);
        newCoins[i].transform.localPosition = new Vector3(transform.position.x, transform.position.y, randomZpos);
        newCoins[i].GetComponent<ChangeLane>().PositionLane();
        minZpos = randomZpos + 1;
        //   if ()
        //    {
        //        RepositionateCoins(i, minZpos);
        //    }
        //}
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
        }
    }

}
