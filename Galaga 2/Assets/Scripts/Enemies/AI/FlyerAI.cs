using UnityEngine;

public class FlyerAI : MonoBehaviour , IEnemyAI
{
    private State currentState;
    private Health healthScript;
 // public GridManager grid;

/*  [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float arriveThreshold = 0.05f;

    private bool hasClaimedCell = false;
    private bool hasFilledCell = false;
    private int claimedRow = -1;
    private int claimedCol = -1;
    private Vector3 claimedCellCenter; */
    void Awake()
    {
        healthScript = GetComponent<Health>();
    }

    void Start()
    {
        currentState = State.Moving;
    }

    void Update(){
        ManageState();
    }

    public void Attack()
    {
        Debug.Log("Metot Kurulmadı");
    }

    public void Leave()
    {
        Debug.Log("Metot Kurulmadı");
    }

    public void Move()
    {
        Debug.Log("Metot Kurulmadı");
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
}