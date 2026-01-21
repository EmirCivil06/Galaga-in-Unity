using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [Header("Hareket Kontrolü")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;
    private Vector2 moveDirection;
    [SerializeField] private InputActionReference moveAction;
    [Header("Saldırı Kontrolü")]
    [SerializeField] private InputActionReference attackAction;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.2f;
    [Header("Ortam Değişkenleri")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Renderer Renderer;
    [Header("Manager'lar")]
    [SerializeField] private ObjectPoolManager objectPoolManager;
    [SerializeField] private GameManager gameManager;
    private float currentCooldown;


    private void Awake()
    {
        objectPoolManager = FindFirstObjectByType<ObjectPoolManager>();
    }

    void Start()
    {
        moveAction.action.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = moveAction.action.ReadValue<Vector2>();

        if (currentCooldown <= 0f)
        {
            if (attackAction.action.IsPressed())
            {
                Shoot();
                currentCooldown = fireRate;
            }
        }
        else
        {
            currentCooldown -= Time.deltaTime;
        }
    }

    private void Shoot()
    {
        if (!gameManager.isPaused)
        {
            SoundManager.PlaySound(SoundType.PlayerShoot, 0.5f);
            objectPoolManager.ActivateObject(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * speed;
    }

    // Oyuncunun kamera kapsamı dışına çıkmasını engellemek için LateUpdate içine mantık ifadeleri
    private void LateUpdate()
    {
        float vert = mainCamera.orthographicSize;
        float horiz = vert * mainCamera.aspect;
        Vector3 camPos = mainCamera.transform.position;
        Vector3 ext = Renderer.bounds.extents;

        Vector3 p = transform.position;
        p.x = Mathf.Clamp(p.x, camPos.x - horiz + ext.x, camPos.x + horiz - ext.x);
        p.y = Mathf.Clamp(p.y, camPos.y - vert + ext.y, camPos.y + vert - ext.y);
        transform.position = p;
    }
}
