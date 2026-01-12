using UnityEngine;

public class BatjackAI : MonoBehaviour, IEnemyAI
{
    [SerializeField] private Transform player;
    private ObjectPoolManager objectPoolManager;

    void Start()
    {
        objectPoolManager = FindAnyObjectByType<ObjectPoolManager>();
    }

    public void Attack(int damage)
    {
        throw new System.NotImplementedException();
    }

    public void Move(int speed)
    {
        throw new System.NotImplementedException();
    }

    public void ManageState()
    {
        throw new System.NotImplementedException();
    }
}
