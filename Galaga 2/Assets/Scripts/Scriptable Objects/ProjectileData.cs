using UnityEngine;

[CreateAssetMenu(fileName = "BulletData", menuName = "Scriptable Objects/BulletData")]
public class ProjectileData : ScriptableObject
{
    public int damage;
    public Sprite bulletSprite;
    public float speed;
}
