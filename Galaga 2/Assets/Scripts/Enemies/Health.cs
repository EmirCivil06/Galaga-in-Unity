using UnityEngine;

// Bütün düşman objeleri için baz sınıf
public class Health : MonoBehaviour
{
    private ObjectPoolManager objectPoolManager;
    public int health;
    [SerializeField] private PlayerData playerData;
    // Girilen fieldlar yoluyla düşman karakterin sağlık değeri güncellenir
    void Awake()
    {
        objectPoolManager = FindFirstObjectByType<ObjectPoolManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            Damage(collision, playerData.damage);
        }
    }

    private void Damage(Collider2D collision, int damage)
    {
        health -= damage;
        objectPoolManager.DeactivateObject(collision.gameObject);
        if (health <= 0)
        {
            Debug.Log("Dead");
            objectPoolManager.DeactivateObject(gameObject);
        }
    }
}
