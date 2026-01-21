using UnityEngine;

public class EProjectile : MonoBehaviour
{
    public ProjectileData data;
    private GameObject player;
    private Vector3 direction;
    
    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
            renderer.sprite = data.bulletSprite;
        else Debug.Log("RENDERER YOK");

        player = GameObject.FindGameObjectWithTag("Player");
        direction = (player.transform.position - transform.position).normalized;

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Time.deltaTime * data.speed * direction;
        if (transform.position.y > Camera.main.orthographicSize * Camera.main.aspect || transform.position.y < -Camera.main.orthographicSize * Camera.main.aspect)
        {
            Destroy(gameObject);
        }
    }
}
