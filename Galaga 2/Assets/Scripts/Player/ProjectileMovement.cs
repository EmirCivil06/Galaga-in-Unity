using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private ObjectPoolManager objectPoolManager;

    private void Awake()
    {
        objectPoolManager = FindFirstObjectByType<ObjectPoolManager>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        if (transform.position.y > Camera.main.orthographicSize * Camera.main.aspect || transform.position.y < -Camera.main.orthographicSize * Camera.main.aspect)
        {
            objectPoolManager.DeactivateObject(gameObject);
        }
    }
}
