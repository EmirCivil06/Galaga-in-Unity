using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private ObjectPoolManager objectPoolManager;
    public int health;
    [SerializeField] private PlayerData player;
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
            health -= player.damage;
            objectPoolManager.DeactivateObject(collision.gameObject);
            if (health <= 0)
            {
                Debug.Log("Dead!");
            }
        }
    }
}
