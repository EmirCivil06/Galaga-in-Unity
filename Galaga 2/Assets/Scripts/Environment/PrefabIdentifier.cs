using UnityEngine;

public class PrefabIdentifier : MonoBehaviour
{
    [HideInInspector] public GameObject prefab;

    public void SetPrefab(GameObject prefab)
    {
        this.prefab = prefab;
    }
}
