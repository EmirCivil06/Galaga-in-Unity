using UnityEngine;

public class FlyerAI : MonoBehaviour , IEnemyAI
{
    private State currentState;
    private Health healthScript;
    private Vector2Int targetCell;
    void Awake()
    {
        healthScript = GetComponent<Health>();
    }

    void Start()
    {
        currentState = State.Moving;
    }

    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    public void Leave()
    {
        throw new System.NotImplementedException();
    }

    public void Move()
    {
        throw new System.NotImplementedException();
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
