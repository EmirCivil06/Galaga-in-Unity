using UnityEngine;

public class EnemyGridExample : MonoBehaviour
{
    private Vector3 target;
    public float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = new Vector3(transform.position.x + 10, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
       transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed); 
    }
}
