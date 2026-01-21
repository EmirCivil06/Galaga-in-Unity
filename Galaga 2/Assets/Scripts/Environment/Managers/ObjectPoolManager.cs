using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

    [SerializeField]private List<GameObject> pooledObjects = new List<GameObject>();

    private void Awake()
    {
        foreach (var item in pooledObjects)
        {
            switch (item.tag)
            {
                case "Projectile":
                    CreatePool(item, 12);
                    break;
                case "Enemy":
                    CreatePool(item, 10);
                    break;
                default:
                    CreatePool(item, 5);
                    break;
           }  
        }
    }

    private void CreatePool(GameObject prefab, int poolSize)
    {
        Queue<GameObject> newPool = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.GetComponent<PrefabIdentifier>().SetPrefab(prefab);
            obj.SetActive(false);
            newPool.Enqueue(obj);
        }
        poolDictionary[prefab] = newPool;
    }

    public void ActivateObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        Queue<GameObject> pool = poolDictionary[prefab];
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);
        }
    }

    public void DeactivateObject(GameObject obj)
    {
        obj.SetActive(false);
        poolDictionary[obj.GetComponent<PrefabIdentifier>().prefab].Enqueue(obj);
    }
}
