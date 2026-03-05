using System.Collections;
using UnityEngine.Pool;
using UnityEngine;
using System.Collections.Generic;
using System;

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] private bool _addToDontDestroyOnLoad = false;
    private GameObject _emptyHolder;

    private static GameObject _particleSytemsEmpty;
    private static GameObject _gameObjectsEmpty;
    private static GameObject _soundFXEmpty;

    private static Dictionary<GameObject, ObjectPool<GameObject>> _objectPools;
    private static Dictionary<GameObject, GameObject> _cloneToPrefabMap;

    public enum PoolType
    {
        GameObjects,
        SoundFX,
        ParticleSytems
    }

    public static PoolType PoolingType;

    void Awake()
    {
        _objectPools = new Dictionary<GameObject, ObjectPool<GameObject>>();
        _cloneToPrefabMap = new Dictionary<GameObject, GameObject>();
        SetupEmpties();
    }

    void SetupEmpties()
    {
        _emptyHolder = new GameObject("Bütün Havuzlar");

        _particleSytemsEmpty = new GameObject("Özel Efekt Havuzu");
        _particleSytemsEmpty.transform.SetParent(_emptyHolder.transform);

        _gameObjectsEmpty = new GameObject("Obje Havuzu");
        _gameObjectsEmpty.transform.SetParent(_emptyHolder.transform);

        _soundFXEmpty = new GameObject("Ses Efekti Havuzu");
        _soundFXEmpty.transform.SetParent(_emptyHolder.transform);

        if (_addToDontDestroyOnLoad) DontDestroyOnLoad(_particleSytemsEmpty);
    }

    private static void CreatePool(GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.GameObjects)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () => CreateObject(prefab, pos, rot, poolType),
            actionOnGet: OnGetObject,
            actionOnRelease: OnReleaseObject,
            actionOnDestroy: OnDestroyObject
            );
        _objectPools.Add(prefab, pool);    
    }

    private static GameObject CreateObject(GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.GameObjects)
    {
        prefab.SetActive(false);
        GameObject obj = Instantiate(prefab, pos, rot);
        prefab.SetActive(true);

        GameObject parentObject = SetParentObject(poolType);
        obj.transform.SetParent(parentObject.transform);
        return obj;
    }

    private static GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.GameObjects:
                return _gameObjectsEmpty;
            case PoolType.SoundFX:
                return _soundFXEmpty;
            case PoolType.ParticleSytems:
                return _particleSytemsEmpty;
            default:
                return null;
        }
    }

    private static void OnGetObject(GameObject gameObject)
    {
        // Opsiyonel mantık
    }
    private static void OnReleaseObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
    private static void OnDestroyObject(GameObject gameObject)
    {
        if (_cloneToPrefabMap.ContainsKey(gameObject))
        {
            _cloneToPrefabMap.Remove(gameObject);
        }
    }

    private static T SpawnObject<T>(GameObject objectToSpawn, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.GameObjects) where T : UnityEngine.Object
    {
        if (!_objectPools.ContainsKey(objectToSpawn))
        {
            CreatePool(objectToSpawn, pos, rot, poolType);
        }

        GameObject obj = _objectPools[objectToSpawn].Get();

        if (obj != null)
        {
            if (!_cloneToPrefabMap.ContainsKey(obj))
            {
                _cloneToPrefabMap.Add(obj, objectToSpawn);
            }

            obj.transform.position = pos;
            obj.transform.rotation = rot;
            obj.SetActive(true);

            if (typeof(T) == typeof(GameObject))
            {
                return obj as T;
            }
            T component = obj.GetComponent<T>();
            if (component == null)
            {
                Debug.LogError($"{objectToSpawn.name} Objesinin {typeof(T)} tipinde bir komponenti yok.");
                return null;
            }
            return component;
        }
        return null;
    }

    public static T SpawnObject<T>(T typePrefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.GameObjects) where T : Component
    {
        return SpawnObject<T>(typePrefab.gameObject, pos, rot, poolType);
    }

    public static GameObject SpawnObject(GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.GameObjects) 
    {
        return SpawnObject<GameObject>(prefab, pos, rot, poolType);
    }

    public static void ReleaseObject(GameObject obj, PoolType poolType = PoolType.GameObjects)
    {
        if (_cloneToPrefabMap.TryGetValue(obj, out GameObject prefab))
        {
            GameObject parentObject = SetParentObject(poolType);
            if (obj.transform.parent != parentObject.transform)
            {
                obj.transform.SetParent(parentObject.transform);
            }

            if (_objectPools.TryGetValue(prefab, out ObjectPool<GameObject> pool))
            {
                pool.Release(obj);
            }
        } else
        {
            Debug.LogWarning("Havuzu olmayan bir obje geri döndürülmeye çalışılıyor: " + obj.name);
        }
    }
}
