using UnityEngine;

public class MovingBackground : MonoBehaviour
{
    // Hareket eden arkaplan için script
    [SerializeField] private float speed;
    [SerializeField] private Renderer bgRenderer;

    // Update is called once per frame
    void Update()
    {
        bgRenderer.material.mainTextureOffset += new Vector2(0, speed * Time.deltaTime);
    }
}
