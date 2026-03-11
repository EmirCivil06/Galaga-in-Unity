using UnityEngine;

// Düşmanların yerleşebileceği ızgaranın beyni
public class GridManager : MonoBehaviour
{
    // Gidebileceği tüm yönler
    private enum Direction
    {
        Left,
        Right,
        Middle,
        None
    }

    [Header("Izgaranın Temel Komponentleri")]
    // Satır sütun sayısı
    public int rows = 2;
    public int columns = 4;
    // Hücre boyutu ve hücre matrisi
    public float cellSize = 1f;
    private Cell[,] cells;
    // Tur atma alanının köşe noktaları
    private Vector3 leftPoint, rightPoint, start;

    [Header("Izgaranın Hareket Komponentleri")]
    public int lapInterval = 2; // Özel hareket yapmadan önce atması gereken tur sayısı
    public float speed = 0.75f; // Hareket hızı
    public float zigZagLength = 2f; // Sağ ve sol köşenin ortaya olan uzaklığı
    // Büyüyüp küçülme eyleminin değişkenleri
    public int growAndShrinkLim = 2;
    public float growShrinkDuration = 1.5f;

    // Sınıfa özel fieldlar
    private int lapCounter = 0; // Tur atma sayacı
    private int sizeActionCount; // Yapılabilecek büyüyüp küçülebilme eylemlerinin toplam sayısı
    private Direction currentDir = Direction.Right; // Şu anki yön
    private float originalCellSize;
    private bool flag = false;
    private int counter;

    // Büyüyüp küçülme animasyonu için özel zamanlayıcı
    private float growShrinkTimer = 0f;

    // Awake ile erkenden oluşturma
    void Awake()
    {
        // Değerlerin önbelleklenmesi
        leftPoint = transform.position - new Vector3(zigZagLength, 0f, 0f);
        rightPoint = transform.position + new Vector3(zigZagLength, 0f, 0f);
        start = transform.position;
        originalCellSize = cellSize;
        sizeActionCount = growAndShrinkLim * 2; // 2 defa büyüyüp küçülmesini istiyorsak 2 * 2 = 4
        counter = 0;
    }

    // Start ile oluşturma
    void Start()
    {
        InitGrid();
    }

    // Update ile güncelleme
    void Update()
    {
        // State machine
        if (IsFull()) ManageGridState();

        // Sadece transform değiştiğinde hücreleri güncelle
        if (transform.hasChanged)
        {
            UpdateCellPositions();
            transform.hasChanged = false;
        }
    }

    // Grid dolu mu değil mi kontrolü
    private bool IsFull()
    {
        int filledCells = 0;
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                if (cells[r, c].isFilled)
                {
                    filledCells++;
                }
            }
        }
        if (filledCells == rows * columns)
        {
            return true;
        }
        else return false;
    }

    // Hücreleri oluşturup boş hücreleri dolduruyoruz
    private void InitGrid()
    {
        cells = new Cell[rows, columns];
        UpdateCellPositions();
    }

    private void UpdateCellPositions()
    {
        // Hücreler yoksa güvenlik için eylemde bulunmuyoruz
        if (cells == null) return;

        // Grid merkezini belirliyoruz
        Vector3 pivotOffset = new Vector3(columns * cellSize * 0.5f, rows * cellSize * 0.5f, 0f);
        Vector3 origin = transform.position - pivotOffset;

        // Hücrelere atama yapma
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                Vector3 center = origin + new Vector3(c * cellSize + cellSize * 0.5f, r * cellSize + cellSize * 0.5f, 0f);

                // Cell bir struct olduğu için doğrudan değer atayabiliriz
                cells[r, c].width = cellSize;
                cells[r, c].height = cellSize;
                cells[r, c].origin = center;
                cells[r, c].isFilled = false;
            }
        }
    }
    // State machine
    private void ManageGridState()
    {
        // Gittiği yöne göre Update içinde devamlı olarak metot çağırma
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
                GrowAndShrink(1.5f, growShrinkDuration);
                break;
        }
    }

    // Sola gitme
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
            else currentDir = Direction.Right;
        }
    }

    // Sağa gitme metodu
    private void Right(float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, rightPoint, Time.deltaTime * speed);
        if (transform.position == rightPoint) currentDir = Direction.Left;
    }

    // Ortaya gitme metodu
    private void Middle(float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, start, Time.deltaTime * speed);
        // Ortaya geldiğinde yönü None olarak ayarlanıyor ki başka yere gitmeye çalışmasın
        if (transform.position == start)
        {
            currentDir = Direction.None;
            growShrinkTimer = 0f; // Büyüme evresine geçerken büyütme sayacını sıfıtla
        }
    }

    // Büyüme ve küçülme eyleminden sorumlu metot
    private void GrowAndShrink(float targetSize, float duration)
    {
        // Zamanlayıcıyı manuel olarak artır
        growShrinkTimer += Time.deltaTime;

        // Mathf.PingPong'a Time.time yerine kendi sayacımızı veriyoruz
        float t = Mathf.PingPong(growShrinkTimer, duration) / duration;
        cellSize = Mathf.Lerp(originalCellSize, targetSize, t);

        UpdateCellPositions(); // Boyut değiştiği için güncelle

        // Kesme hatasıyla beraber kaç defa büyüyüp küçüldü onu sayıyoz
        if (!flag && Mathf.Abs(cellSize - targetSize) < 0.01f)
        {
            flag = true;
            counter++;
        }

        if (flag && Mathf.Abs(cellSize - originalCellSize) < 0.01f)
        {
            flag = false;
            counter++;
        }

        // Sayaç sınır değere ulaşırsa baştan başla
        if (counter >= sizeActionCount)
        {
            currentDir = Direction.Right;
            cellSize = originalCellSize;
            counter = 0;
            growShrinkTimer = 0f; // İşlem bitince temizle
        }
    }

    // Editör oynatmıyorken
    void OnValidate()
    {
        if (rows > 0 && columns > 0 && Application.isPlaying)
        {
            // InitGrid'i editör modunda ve editör oynuyor iken çağır
            UpdateCellPositions();
            // Güncelleme metodunu editör modunda çağırmak performansı düşürür. (Gemini)
        }
    }

    void OnDrawGizmos()
    {
        // cells null ise hata vermemesi için güvenlik kontrolü
        if (cells == null) return;

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

// Hücre struct'ı. Normalde sınıf yapısını kullanıyorduk ama Gemini gc açısından struct daha iyi dedi
public struct Cell
{
    public float width, height;
    public Vector3 origin;
    public bool isFilled;

    public Cell(float size, Vector3 origin)
    {
        width = size;
        height = size;
        this.origin = origin;
        isFilled = false;
    }
}