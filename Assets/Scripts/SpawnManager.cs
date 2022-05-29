using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject[] enemyType;
    public GameObject[] powerups;
    public GameObject enemyContainer;
    public GameObject powerupContainer;

    private int[] powerUpSpawnChance =
    {
        10,
        10,
        10,
        40,
        5,
        10,
        10,
    };
    void Start()
    {

    }

    void Update()
    {
        
    }
    public void StartTheGame()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerup());
    }
    public IEnumerator SpawnEnemy()
    {
        while (Player != null)
        {
            Vector3 enemySpawnPosition = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(enemyType[Random.Range(0, 4)], enemySpawnPosition, Quaternion.Euler(0, 0, 0));
            newEnemy.transform.parent = enemyContainer.transform;
            yield return new WaitForSeconds(2f);
        }
    }
    public IEnumerator SpawnPowerup()
    {
        while (Player != null)
        {
            Vector3 poweruppostospawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerup = randomTable();
            GameObject newPowerup = Instantiate(powerups[randomPowerup], poweruppostospawn, Quaternion.identity);
            newPowerup.transform.parent = powerupContainer.transform;
            yield return new WaitForSeconds(Random.Range(2,6));
        }
    }
    int randomTable()
    {
        int rng = Random.Range(1, 101);
        for (int i = 0; i < powerUpSpawnChance.Length; i++)
        {
            if (rng <= powerUpSpawnChance[i])
            {
                return i;
            }
            else
            {
                rng -= powerUpSpawnChance[i];
            }
        }
        return 0;
    }
}