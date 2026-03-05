using UnityEngine;

public class BatjackProjectile : MonoBehaviour
{
    // Bu merminin her oluşturulduğunda veya her havuzdan alındığında yönünün ve
    // açısının değiştirilmesi lazım.
    public ProjectileData data;
    private GameObject player;
    private Vector3 direction;
    
    // Başlangıçta açıyı ve yönü değiştir
    void Start()
    {
        // ProjectileData için özel atama yapmamız gerekti
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
            renderer.sprite = data.bulletSprite;
        else Debug.Log("RENDERER YOK");
        UpdateTrajectory();
    }

    // Devreye girdiğinde açıyı ve yönü değiştir
    void OnEnable()
    {
        UpdateTrajectory();
    }
    // Açı ve yön değiştirme metodu
    private void UpdateTrajectory()
    {
        // Oyuncuyu bul
        player = GameObject.FindGameObjectWithTag("Player");
        direction = (player.transform.position - transform.position).normalized; // Oyuncunun konumunu al ve bu konumu yeni bir Vector3'e aktar 
        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg; // O Vector3'e göre açıyı değiştir
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

    void Update()
    {
        // Her frame de yöne doğru biraz daha kayıyor gibi düşünün
        transform.position += Time.deltaTime * data.speed * direction;
        if (transform.position.y > Camera.main.orthographicSize * Camera.main.aspect || transform.position.y < -Camera.main.orthographicSize * Camera.main.aspect)
        {
            // Kamera dışına çıktığı an havuza geri dönüyor
            ObjectPoolManager.ReleaseObject(gameObject, ObjectPoolManager.PoolType.GameObjects);
        }
    }
}
