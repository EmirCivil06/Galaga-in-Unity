using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private ObjectPoolManager objectPoolManager;

    void Awake()
    {
        objectPoolManager = FindFirstObjectByType<ObjectPoolManager>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            objectPoolManager.DeactivateObject(collision.gameObject);
        }
    }
}
