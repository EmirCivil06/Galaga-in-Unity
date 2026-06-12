using Unity.VisualScripting;
using UnityEngine;

// Değiştirilmiş ızgara betiği
public class GridManager : MonoBehaviour
{
    // State Machine için yön
    private enum Direction
    {
        Left,
        Right,
        Middle,
        None
    }

    // Değişkenler
    [Header("Izgara Hücre Ayarları")]
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private int rows = 2, columns = 8;
    [SerializeField] private float spacing = 1f;
    [SerializeField] private float targetSpacingSize = 1.5f;

    [Header("Izgaranın Hareket Ayarları")]
    [SerializeField] private int lapInterval = 2;
    [SerializeField] private float speed = 0.75f;
    [SerializeField] private float zigZagLength = 2f;
    [SerializeField] private int growAndShrinkLim = 2;
    [SerializeField] private float growShrinkDuration = 1.5f;

    // Sınıfa özel alanlar
    public Cell[,] grid;
    private Vector3 leftPoint, rightPoint, start;
    private int lapCounter = 0;
    private Direction currentDir = Direction.Right;
    private float growShrinkTimer = 0f;
    private bool flag = false;
    private int counter;
    private float originalSize;
    private int sizeActionCount;
    private bool isAnyFilled = false;

    // OnEnable
    void OnEnable()
    {
        // Eventi dinlemeye başla
        Cell.OnAnyEnemyPlaced += StartMoving;
    }

    // OnDisable
    void OnDisable()
    {
        // Eventi dinlemeyi bırak
        Cell.OnAnyEnemyPlaced -= StartMoving;
    }

    // Start
    void Start()
    {
        sizeActionCount = growAndShrinkLim * 2;
        originalSize = spacing;
        leftPoint = transform.position - new Vector3(zigZagLength, 0f, 0f);
        rightPoint = transform.position + new Vector3(zigZagLength, 0f, 0f);
        start = transform.position;
        InitGrid();
    }

    // Update
    void Update()
    {
        ManageGridState();
    }

    // Izgarayı yapılandır
    private void InitGrid()
    {
        grid = new Cell[rows, columns];

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                // Boş ise doldurulacak
                if (grid[r, c] == null)
                {
                    grid[r, c] = Instantiate(cellPrefab, transform);
                    grid[r, c].Size = 1f;
                }
            }
        }
        // Hücreler oluştuktan sonra pozisyonlarını ayarla
        UpdateGrid();
    }

    // Izgarayı güncelle
    private void UpdateGrid()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                if (grid[r, c] != null)
                {
                    // Pozisyonu ve boyutu yeniden hesaplama
                    grid[r, c].transform.localPosition = new Vector3(
                        (c + 0.5f - columns * 0.5f) * spacing,
                        (r + 0.5f - rows * 0.5f) * spacing,
                        0f);
                }
            }
        }
    }

    // Event'in tetikleyeceği metot
    private void StartMoving()
    {
        isAnyFilled = true;
    }

    // Izgaranın beyni
    private void ManageGridState()
    {
        // Kolaylık için state machine
        if (!isAnyFilled) return;
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
                GrowAndShrink(targetSpacingSize, growShrinkDuration);
                break;
        }
    }

    // Sola gitme logic'i
    private void Left(float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, leftPoint, Time.deltaTime * speed);
        if (transform.position == leftPoint)
        {
            lapCounter++;
            // Eğer tur sınırımız kadar tur atarsa ortaya git
            if (lapCounter >= lapInterval)
            {
                currentDir = Direction.Middle;
                lapCounter = 0;
            }
            // Eğer sınıra ulaşmadıysan döngüye devam et
            else currentDir = Direction.Right;
        }
    }

    // Sağa gitme logic'i
    private void Right(float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, rightPoint, Time.deltaTime * speed);
        if (transform.position == rightPoint) currentDir = Direction.Left;
    }

    // Döngüyü tamamladıktan sonra ortaya yönelme
    private void Middle(float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, start, Time.deltaTime * speed);
        if (transform.position == start)
        {
            // Gidecek yönü yok
            currentDir = Direction.None;
            growShrinkTimer = 0f; // Büyüme evresine geçerken büyütme sayacını sıfıtla
        }
    }

    // Hareket döngüsü tamamlandıktan sonra özel hareket
    private void GrowAndShrink(float targetSize, float duration)
    {
        growShrinkTimer += Time.deltaTime;

        // Mathf.PingPong ile sınır değer arasında git gel
        float t = Mathf.PingPong(growShrinkTimer, duration) / duration;
        spacing = Mathf.Lerp(originalSize, targetSize, t);

        UpdateGrid(); // Boyut değiştiği için güncelle

        // Yapılan hareket sayısı
        if (!flag && Mathf.Abs(spacing - targetSize) < 0.01f)
        {
            flag = true;
            counter++;
        }

        if (flag && Mathf.Abs(spacing - originalSize) < 0.01f)
        {
            flag = false;
            counter++;
        }

        // Sayaç sınır değere ulaşırsa baştan başla
        if (counter >= sizeActionCount)
        {
            currentDir = Direction.Right;
            spacing = originalSize;
            counter = 0;
            growShrinkTimer = 0f; // İşlem bitince temizle
        }
    }

    // Boş hücre bulma algoritması
    private Cell FindEmptyCell()
    {
        // Hücreleri tarar ve boş hücreyi bulur
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                if (grid[r, c] != null && !grid[r, c].Occupied && !grid[r, c].Targeted)
                {
                    grid[r, c].Targeted = true;
                    return grid[r, c];
                }
                    
            }
        }
        return null;
    }

    // Boş hücre seçip bu hücrenin konumunu verir
    public Vector3 GiveCell()
    {
        Cell emptyCell = FindEmptyCell();
        return emptyCell.gameObject.transform.position;
    }
}