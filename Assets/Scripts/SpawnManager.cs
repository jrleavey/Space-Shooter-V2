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

    public bool isWaiting = false;
    private bool canSpawnMore = true;
    private bool hasTheBossSpawned = false;

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

    public int _WaveID;
    public int enemiesSpawned;
    public int _enemyDeathcount;
    public int[] _WaveCount;
    public IEnumerator SpawnEnemy()
    {
        while (Player != null)
        {
           if (canSpawnMore == true && _WaveID != 10)
            {
                Vector3 enemySpawnPosition = new Vector3(Random.Range(-8f, 8f), 7, 0);
                GameObject newEnemy = Instantiate(enemyType[Random.Range(0, 4)], enemySpawnPosition, Quaternion.Euler(0, 0, 0));
                enemiesSpawned++;
                newEnemy.transform.parent = enemyContainer.transform;
            }
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
            yield return new WaitForSeconds(Random.Range(2, 6));
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

    public IEnumerator WaveSystem()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerup());
        while (true)
        {
            yield return null;
            switch (_WaveID)
            {
                case 0:
                    CallTheWave();
                    break;
                case 1:
                    CallTheWave();
                    break;
                case 2:
                    CallTheWave();
                    break;
                case 3:
                    CallTheWave();
                    break;
                case 4:
                    CallTheWave();
                    break;
                case 5:
                    CallTheWave();
                    break;
                case 6:
                    CallTheWave();
                    break;
                case 7:
                    CallTheWave();
                    break;
                case 8:
                    CallTheWave();
                    break;
                case 9:
                    CallTheBoss();
                    break;


            }
        }

    }

    IEnumerator WaitForNextWave()
    {
        isWaiting = true;
        yield return new WaitForSeconds(3f);
        canSpawnMore = true;
        _WaveID++;
        _enemyDeathcount = 0;
        isWaiting = false;
    }

    public void CallTheWave()
    {
        if (enemiesSpawned >= 10)
        {
            canSpawnMore = false;
        }

        if (isWaiting == false &&_enemyDeathcount >= 10)
        {
            enemiesSpawned = 0;

            StartCoroutine(WaitForNextWave());
        }
    }
    public void CallTheBoss()
    {
        if (hasTheBossSpawned == false)
        {
            hasTheBossSpawned = true;
            Vector3 enemySpawnPosition = new Vector3(0, 10, 0);
            GameObject newEnemy = Instantiate(enemyType[4], enemySpawnPosition, Quaternion.Euler(0, 0, 0));
            newEnemy.transform.parent = enemyContainer.transform;
            canSpawnMore = false;
            StopCoroutine(SpawnEnemy());
        }
    }

    public void StartWaveSystem()
    {
        StartCoroutine(WaveSystem());
    }
}