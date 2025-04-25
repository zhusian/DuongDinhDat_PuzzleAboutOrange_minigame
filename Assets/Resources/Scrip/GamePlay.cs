using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlay : MonoBehaviour
{
    [SerializeField]
    private Level levelData;
    [SerializeField]
    private GameObject orangePrefab;
    [SerializeField]
    private GameObject obstaclePrefab;
    [SerializeField]
    private GameObject WinMenu;
    [SerializeField]
    private GameObject LoseMenu;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI LevelText;

    private GridManager gridManager;
    private Vector2 touchStartPos;
    private bool isSwiping = false;
    private List<OrangePiece> orangePieces = new List<OrangePiece>();
    private float currentTime;
    private bool isGameOver = false;

    void Awake()
    {
        levelData = LevelHolder.Instance.selectedLevel;
    }
    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        currentTime = levelData.timeLimit;
        LevelText.text = levelData.levelName;
        gridManager.CreateGrid();
        CreateOrangesAndObstacles();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
            isSwiping = true;
        }
        else if (Input.GetMouseButtonUp(0) && isSwiping)
        {
            Vector2 touchEndPos = Input.mousePosition;
            isSwiping = false;

            Vector2 swipeDelta = touchEndPos - touchStartPos;
            HandleSwipe(swipeDelta);
        }
        if (!isGameOver)
        {
            UpdateTimer();
        }
    }

    void UpdateTimer()
    {
        currentTime -= Time.deltaTime;
        currentTime = Mathf.Clamp(currentTime, 0f, levelData.timeLimit);

        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";

        if (currentTime <= 0f)
        {
            isGameOver = true;
            LoseMenu.SetActive(true);
            Debug.Log("Time's up! You lose!");
        }
    }

    void HandleSwipe(Vector2 swipeDelta)
    {
        float absX = Mathf.Abs(swipeDelta.x);
        float absY = Mathf.Abs(swipeDelta.y);

        if (absX > absY)
        {
            if (swipeDelta.x > 0)
                MoveOranges(Vector2Int.up);
            else
                MoveOranges(Vector2Int.down);
        }
        else
        {
            if (swipeDelta.y > 0)
                MoveOranges(Vector2Int.left);
            else
                MoveOranges(Vector2Int.right);
        }
    }

    void MoveOranges(Vector2Int direction)
    {
        foreach (OrangePiece piece in orangePieces)
        {
            Vector2Int nextPos = piece.gridPosition + direction;

            if (IsCellValid(nextPos))
            {
                piece.gridPosition = nextPos;
                piece.gameObject.transform.position = gridManager.GetCellWorldPosition(nextPos.x, nextPos.y, transform.position);
            }
        }

        if (CheckWin())
        {
            Debug.Log("You Win!");
            WinMenu.SetActive(true);

            int currentUnlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
            int currentLevelNumber = LevelHolder.Instance.selectedLevel.levelNumber;

            if (currentLevelNumber >= currentUnlockedLevel)
            {
                PlayerPrefs.SetInt("UnlockedLevel", currentLevelNumber + 1);
                PlayerPrefs.Save();
            }
        }
    }

    bool IsCellValid(Vector2Int cellPos)
    {
        if (cellPos.x < 0 || cellPos.y < 0 || cellPos.x >= levelData.columns || cellPos.y >= levelData.rows)
            return false;

        Vector3 worldPos = gridManager.GetCellWorldPosition(cellPos.x, cellPos.y, transform.position);
        Collider2D block = Physics2D.OverlapCircle(worldPos, 0.1f, LayerMask.GetMask("Block"));
        if (block != null)
        {
         //   Debug.Log("Blocked by obstacle at " + cellPos);
            return false;
        }

        foreach (var piece in orangePieces)
        {
            if (piece.gridPosition == cellPos)
            {
        //        Debug.Log("Occupied by another orange at " + cellPos);
                return false;
            }
        }

        return true;
    }

    void CreateOrangesAndObstacles()
    {
        int idCounter = 1;

        for (int i = 0; i < levelData.orangePositions.Length; i++)
        {
            Vector2Int position = levelData.orangePositions[i];
            Vector3 worldPos = gridManager.GetCellWorldPosition(position.x, position.y, transform.position);

            GameObject orange = Instantiate(orangePrefab, worldPos, Quaternion.identity);
            orange.transform.SetParent(gridManager.transform);
            orange.layer = LayerMask.NameToLayer("Orange");

            Image img = orange.GetComponent<Image>();
            if (img != null && i < levelData.orangeSprites.Length)
                img.sprite = levelData.orangeSprites[i];

            int id = idCounter++;
            orangePieces.Add(new OrangePiece(orange, position, id));
         //   Debug.Log($"Mảnh cam ID: {id} - Vị trí: {position}");
        }

        foreach (var obstaclePosition in levelData.obstaclePositions)
        {
            GameObject obstacle = Instantiate(obstaclePrefab, gridManager.GetCellWorldPosition(obstaclePosition.x, obstaclePosition.y, transform.position), Quaternion.identity);
            obstacle.transform.SetParent(gridManager.transform);
        }
    }

    bool CheckWin()
    {
        OrangePiece piece1 = GetOrangePieceById(1);
        OrangePiece piece2 = GetOrangePieceById(2);
        OrangePiece piece3 = GetOrangePieceById(3);
        OrangePiece piece4 = GetOrangePieceById(4);

        if (piece1 == null || piece2 == null || piece3 == null || piece4 == null)
            return false;

       // Debug.Log($"P1: {piece1.gridPosition}, P2: {piece2.gridPosition}, P3: {piece3.gridPosition}, P4: {piece4.gridPosition}");

        bool isSquareInOrder = IsSquareFormation(piece1, piece2, piece3, piece4);

        if (isSquareInOrder)
        {
            Debug.Log("You Win!");
            return true;
        }

        return false;
    }

    bool IsSquareFormation(OrangePiece piece1, OrangePiece piece2, OrangePiece piece3, OrangePiece piece4)
    {
        Vector2Int p1Pos = piece1.gridPosition;
        Vector2Int p2Pos = piece2.gridPosition;
        Vector2Int p3Pos = piece3.gridPosition;
        Vector2Int p4Pos = piece4.gridPosition;

        bool rowMatch34 = p3Pos.x == p4Pos.x;
        bool rowMatch12 = p1Pos.x == p2Pos.x;
        bool columnMatch31 = p3Pos.y == p1Pos.y;
        bool columnMatch42 = p4Pos.y == p2Pos.y;
        bool correctOrder = p3Pos.x < p1Pos.x && p4Pos.x < p2Pos.x;

       // Debug.Log($"RowMatch34: {rowMatch34}, RowMatch12: {rowMatch12}, ColumnMatch31: {columnMatch31}, ColumnMatch42: {columnMatch42}, CorrectOrder: {correctOrder}");

        return rowMatch34 && rowMatch12 && columnMatch31 && columnMatch42 && correctOrder;
    }

    OrangePiece GetOrangePieceById(int id)
    {
        return orangePieces.Find(piece => piece.id == id);
    }
}
