using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBonus : MonoBehaviour
{
    private const int NUMBER_OBJECT_TO_SPAWN = 3; 

    private GameObject[] spawnPoint;
    private List<bool> isNotEmpty;

    private GameObject[] bonus;

    private void InitListSpawn()
    {
        for(int i = 0; i < spawnPoint.Length; i++)
        {
            isNotEmpty.Add(false);
        }
    }

    private void SpawnObject()
    {
        int random_object = (int)Random.Range(0.0f, (float)bonus.Length);

        //Instantiate(bonus[0], spawnPoint[i].transform.position, spawnPoint[i].transform.rotation);
    }

    void Start()
    {
        isNotEmpty = new List<bool>();

        spawnPoint = GameObject.FindGameObjectsWithTag("SpawnObject");
        bonus = GameObject.FindGameObjectsWithTag("Bonus");

        InitListSpawn();

        StartCoroutine(SpawnBonusCoroutine());
    }

    void Update()
    {
        
    }

    IEnumerator SpawnBonusCoroutine()
    {
        while(true)
        {
            //resetSpawner();

            for(int i = 0; i < NUMBER_OBJECT_TO_SPAWN; i++)
            {
                int random_position = (int)Random.Range(0.0f, (float)spawnPoint.Length);

                SpawnObject();

                if(isNotEmpty[i] == false)
                {
                    SpawnObject();
                }
            }

            yield return new WaitForSeconds(5);
        }
    }
}
