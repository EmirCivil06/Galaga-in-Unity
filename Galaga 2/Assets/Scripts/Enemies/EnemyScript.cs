using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private ObjectPoolManager objectPoolManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        objectPoolManager = FindFirstObjectByType<ObjectPoolManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            objectPoolManager.DeactivateObject(collision.gameObject); 
        }
    }
}
