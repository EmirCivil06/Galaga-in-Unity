using UnityEngine;

public class GridManager : MonoBehaviour
{
    private enum Direction
    {
        Left,
        Right,
        Middle,
        None
    }
    [Header("Izgaranın Temel Komponentleri")]
    public int rows = 2; // Satır ve sütun sayısı
    public int columns = 4;
    public float cellSize = 1f; // Hücrelerin boyutu
    private Cell[,] cells; // Izgara hücreleri 
    private Vector3 leftPoint, rightPoint, start;
    [Header("Izgaranın Hareket Komponentleri")]
    public int lapInterval = 0;
    public float speed = 0.75f;
    public float zigZagLength = 2f;
    private Direction currentDir = Direction.Right;

    // Hücre pozisyonlarını güncelle
    private void UpdateCellPositions()
    {
        if (cells == null) Debug.LogError($"{gameObject.tag} gridinin hücreleri null");

        // Çizilme noktasının (origin) belirlenmesi
        Vector3 pivotOffset = new Vector3(columns * cellSize * 0.5f, rows * cellSize * 0.5f, 0f);
        Vector3 origin = transform.position - pivotOffset;
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                // Satır ve sütun indeksine göre hücrenin merkezi 
                Vector3 center = origin + new Vector3(c * cellSize + cellSize * 0.5f, r * cellSize + cellSize * 0.5f, 0f);
                
                // Eğer hücre zaten varsa sadece pozisyonu güncelle 
                if (cells[r, c] != null)
                {
                    cells[r, c].origin = center;
                }
                else
                {
                    cells[r, c] = new Cell(cellSize, center);
                }
            }
        }
    }

    // Izgarayı oluştur
    private void InitGrid()
    {
        cells = new Cell[rows, columns];
        UpdateCellPositions();
    }

    // Izgaramızın zig zag şeklinde hareket etmesi lazım
    private void ManageGridState()
    {
        switch (currentDir)
        {
            case Direction.Left:
                Left(speed);
                break;
            case Direction.Right:
                Right(speed);
                break;
            case Direction.Middle:
                Middle(speed);
                break;
            case Direction.None:
                // Izgara büyüyüp küçülmeye çalışacak
                Debug.Log("Not Implemented");
                break;       
        }
    }

    // Sola gitme metodu
    private void Left(float speed)
    {
        if (currentDir != Direction.Left) return;
        transform.position = Vector3.MoveTowards(transform.position, leftPoint, Time.deltaTime * speed);
        if (transform.position == leftPoint) currentDir = Direction.Right;
    }

    // Sağa gitme metodu
    private void Right(float speed)
    {
        if (currentDir != Direction.Right) return;
        transform.position = Vector3.MoveTowards(transform.position, rightPoint, Time.deltaTime * speed);
        if (transform.position == rightPoint) currentDir = Direction.Left;
    }

    // İki tur yapınca ortaya gidecek
    private void Middle(float speed)
    {
        Debug.Log("Not Implemented");
    }

    // Start metodunda oluşturma
    void Start()
    {
        InitGrid();
    }

    // Awake ile ön bellekleme
    void Awake()
    {
        // Noktaları önbellekleyerek daha sonrasında kullanacağız
        leftPoint = transform.position - new Vector3(zigZagLength, 0f, 0f);
        rightPoint = transform.position + new Vector3(zigZagLength, 0f, 0f);
        start = transform.position; 
    }

    // Update metodunda güncelleme 
    void Update()
    {
        ManageGridState();
        if (transform.hasChanged)
        {
            UpdateCellPositions();
            transform.hasChanged = false;
        }
    }

    // Edit modunda debug amaçlı ızgarayı oluştur
    void OnValidate()
    {
        if(rows > 0 && columns > 0) InitGrid();
    }

    // Hücrelerin çizilmesi
    void OnDrawGizmos()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(cells[r, c].origin, new Vector3(cells[r, c].width, cells[r, c].height));
                Gizmos.DrawSphere(cells[r, c].origin, cellSize * 0.05f);
            }
        }
    }


}

// İşleri kolaylaştırmak için Cell (Hücre) sınıfı
public class Cell
{
    // Genişlik, yükseklik, merkez nokta ve dolu mu değil mi
    public float width, height;
    public Vector3 origin;
    public bool isFilled;

    // Yapıcı
    public Cell(float width, float height, Vector3 origin)
    {
        this.width = width;
        this.height = height;
        this.origin = origin;
        isFilled = false;
    }

    // Yapıcı (overload)
    public Cell(float size, Vector3 origin)
    {
        width = size;
        height = size;
        this.origin = origin;
        isFilled = false;
    }
}