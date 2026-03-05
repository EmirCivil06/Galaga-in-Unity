using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>(); 
    public int currentWave, currentWaveValue;
    private int waveValue;
    public List<GameObject> enemiesToSpawn = new();
    private List<GameObject> activeEnemies = new();

    public Transform spawnPos;
    public float waitingTime;
    private float spawnInterval, waitingTimer;
    private float spawnTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateWave();
    }

    void FixedUpdate()
    {
        currentWaveValue = 16 + (int)(currentWave * 1.25f);
        ManageWave();
        if (spawnTimer <= 0)
        {
            if (enemiesToSpawn.Count > 0)
            {
                GameObject spawned = Instantiate(enemiesToSpawn[0], spawnPos.position, Quaternion.identity);
                enemiesToSpawn.RemoveAt(0);
                activeEnemies.Add(spawned);
                spawnTimer = spawnInterval;
            }
            
        } else
        {
            spawnTimer -= Time.deltaTime;
        }
    }

    public void GenerateWave()
    {
        waveValue = 16 + (int)(currentWave * 1.25f);
        GenerateEnemies();  

        spawnInterval = 1.5f;
    }

    public void GenerateEnemies()
    {
        List<GameObject> generatedEnemies = new List<GameObject>();
        while (waveValue > 0)
        {
            int randEnemyId = Random.Range(0, enemies.Count);
            int randEnemyCost = enemies[randEnemyId].cost;

            if (waveValue - randEnemyCost >= 0)
            {
                generatedEnemies.Add(enemies[randEnemyId].enemyPrefab);
                waveValue -= randEnemyCost;
            } else if (waveValue <= 0)
            {
                break;
            }        
        }
        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
    }

    public void ManageWave()
    {
        activeEnemies.RemoveAll(enemy => enemy == null);

        if (enemiesToSpawn.Count == 0 && activeEnemies.Count == 0)
        {
            waitingTimer += Time.deltaTime;
            if (waitingTimer >= waitingTime)
            {
                GenerateWave();
                currentWave++;
            }
        } else
        {
            waitingTimer = 0;
        }
    }

    public void RemoveEnemy(GameObject obj)
    {
        activeEnemies.Remove(obj);
    }

}
[System.Serializable]
public class Enemy
{
    public GameObject enemyPrefab;
    public int cost;
}