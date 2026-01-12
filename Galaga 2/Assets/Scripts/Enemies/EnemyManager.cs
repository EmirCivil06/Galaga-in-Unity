using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private ObjectPoolManager objectPoolManager;
    public List<GameObject> enemies;
    public Transform spawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objectPoolManager = FindFirstObjectByType<ObjectPoolManager>();
        foreach (var item in enemies)
        {
            objectPoolManager.ActivateAll(item, spawnPoint.position, spawnPoint.rotation);
        }
    }

}
