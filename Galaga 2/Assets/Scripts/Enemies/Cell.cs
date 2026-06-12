using UnityEngine;

public class Cell : MonoBehaviour
{
    // Alanlar
    [SerializeField] private float _size;
    public static System.Action OnAnyEnemyPlaced;
    private bool _isOccupied = false;
    private bool _isTargeted = false;

    public bool Occupied {
        get => _isOccupied;
        set => _isOccupied = value;
    }

    public bool Targeted
    {
        get => _isTargeted;
        set => _isTargeted = value;
    }

    // Erişilebilir boyut alanı
    public float Size
    {
        get => _size;
        set
        {
            _size = value;
            // Boyutu güncelle 
            UpdateCellSize();
        }
    }
    // Boyutu güncelle
    public void UpdateCellSize() {
        transform.localScale = new Vector3(_size, _size, 1f);
    }
    
    // Sahne görünümünde iken çiz
    void OnDrawGizmos(){
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, new Vector3(_size, _size, 0.1f));
        Gizmos.DrawSphere(transform.position, _size * 0.1f);
    }
}