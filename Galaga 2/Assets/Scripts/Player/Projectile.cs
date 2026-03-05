using UnityEngine;

public class Projectile : MonoBehaviour
{
    public ProjectileData data;

    private void Awake()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
            renderer.sprite = data.bulletSprite;
        else Debug.Log("RENDERER YOK");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Update()
    {
        transform.Translate(Vector3.up * data.speed * Time.deltaTime);
        if (transform.position.y > Camera.main.orthographicSize * Camera.main.aspect || transform.position.y < -Camera.main.orthographicSize * Camera.main.aspect)
        {
            ObjectPoolManager.ReleaseObject(gameObject);
        }
    }
}
