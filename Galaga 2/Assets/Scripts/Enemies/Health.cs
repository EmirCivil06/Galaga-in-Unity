
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// Bütün düşman objeleri için baz sınıf
public class Health : MonoBehaviour
{
    public UnityEvent OnDeath;
    public EnemyManager enemyManager;
    private Color flashColor = Color.white;
    private Material _material;
    private SpriteRenderer _renderer;
    public int health;
    public float damageTimer;
    public bool isDying = false;
    public SoundType deathSound;
    [Range(0f, 1f)]
    public float volume;
    // Girilen fieldlar yoluyla düşman karakterin sağlık değeri güncellenir
    void Awake()
    {
        enemyManager = FindFirstObjectByType<EnemyManager>();
        _renderer = GetComponent<SpriteRenderer>();
        _material = _renderer.material;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            var projectile = collision.GetComponent<Projectile>();
            if (projectile != null)
            {
                Damage(collision, projectile.data.damage);
            }
        }
    }

    private void Damage(Collider2D collision, int damage)
    {
        if (!isDying) 
        {
            health -= damage;
            StartCoroutine(DamageFlash());
            ObjectPoolManager.ReleaseObject(collision.gameObject, ObjectPoolManager.PoolType.GameObjects);
        }
        
        if (health <= 0 && !isDying)
        {
            SoundManager.PlaySound(deathSound, volume);
            OnDeath.Invoke();
            isDying = true;
            Invoke("Kill", 0.7f);
        }
    }

    private IEnumerator DamageFlash()
    {
        _material.SetColor("_FlashColor", flashColor);
        float elapsedTime = 0f;
        while(elapsedTime < damageTimer)
        {
            elapsedTime += Time.deltaTime * 2;
            float currentFlashAmount = Mathf.Lerp(1f, 0f, elapsedTime / damageTimer);
            _material.SetFloat("_Amount", currentFlashAmount);
            yield return null;
        }
    }

    private void Kill()
    { 
        Destroy(gameObject);
        enemyManager.RemoveEnemy(gameObject);
        isDying = false;
    }
}
