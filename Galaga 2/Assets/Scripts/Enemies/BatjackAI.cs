using UnityEngine;

public class BatjackAI : MonoBehaviour, IEnemyAI
{
    // private alanlar
    private State currentState;
    private bool hasTarget = false, isPreparingAttack = false;
    private Vector3 target;
    private float attackCooldown = 0f, preparationTimer;
    private int attackCount = 0, phase = 0;
    private float TargetAreaHeight;
    private float TargetAreaWidth;
    private readonly float error = 0.75f;
    private float particleDuration;
    private Health healthScript;
    
    // erişiliebilir alanlar ki atama yapabilelim
    public float speed;
    [Header("Saldırı Eventi")]
    public GameObject bullet;
    public float attackCooldownDuration = 1.3f;
    public ProjectileData projectileData;
    public int AttackLimit, PhaseLimit;
    public ParticleSystem attackIndicator;

    void Awake()
    {
        healthScript = GetComponent<Health>();
    }


    void Start()
    {
        currentState = State.Moving;
        TargetAreaHeight = 2f * Camera.main.orthographicSize - error;
        TargetAreaWidth = TargetAreaHeight * Camera.main.aspect - error;
        particleDuration = attackIndicator.main.duration;
        attackCooldownDuration += particleDuration;
    }

    void Update()
    {
        attackCooldown -= Time.deltaTime;
        ManageState();
    }

    public void Attack()
    {
        if (currentState != State.Attacking) return;
        if (attackCooldown > 0) return; 

        if (!isPreparingAttack)
        {
            SoundManager.PlaySound(SoundType.BJackPrepare, 1);
            Instantiate(attackIndicator, transform.position, Quaternion.identity);
            preparationTimer = particleDuration;
            isPreparingAttack = true;
        }
    
        preparationTimer -= Time.deltaTime;

        if (preparationTimer <= 0f)
        {
            GameObject bulletInstance = Instantiate(bullet, transform.position, Quaternion.identity);
            SoundManager.PlaySound(SoundType.BJackShoot, 1);
            bulletInstance.GetComponent<EProjectile>().data = projectileData;
            attackCount++;
            attackCooldown = attackCooldownDuration; 
            isPreparingAttack = false; 

            if (attackCount >= AttackLimit)
            {
                attackCount = 0;
                phase++;
                if (phase >= PhaseLimit)
                    currentState = State.Leaving;
                else
                    currentState = State.Moving;
            }
        }
    }

    public void Move()
    {
        if (currentState != State.Moving) return;
        InvokeRootMovingAction(TargetAreaWidth, TargetAreaHeight, false);

        if (transform.position == target)
        {
            currentState = State.Attacking;
            phase++;
            hasTarget = false;
        }
    }

    public void Leave()
    {
        if (currentState != State.Leaving) return;
        InvokeRootMovingAction(TargetAreaWidth * 1.5f, 0, true);

        if (transform.position == target)
        {
            hasTarget = false;
            currentState = State.Idle;
        }
    }

    public void ManageState()
    {
        if (healthScript.isDying) 
        {
            return;
        }
        switch (currentState)
        {
            case State.Attacking:
                Attack();
                break;
            case State.Moving:
                Move();
                break; 
            case State.Leaving:
                Leave();
                break;       
        }
    }

    private Vector3 GetPoint(float width, float height, bool leaving)
    {
        float randomX = Random.Range(-width / 2f, width / 2f);
        float randomY = leaving? Random.Range(-9, -7.5f) : Random.Range(0, height / 2f);
        
        return new Vector3(randomX, randomY);
    }

    private void InvokeRootMovingAction(float width, float height, bool leaving)
    {
        if (!hasTarget)
        {
            target = GetPoint(width, height, leaving);
            hasTarget = true;
        }

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
    }
}
