using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Level levelData;  // Tham chiếu tới đối tượng Level
    public GameObject watermelonPrefab;  // Prefab cho mảnh dưa hấu
    public GameObject obstaclePrefab;    // Prefab cho chướng ngại vật
    private GridManager gridManager;  // Tham chiếu tới GridManager

    private Vector2 touchStartPos;  // Vị trí bắt đầu vuốt
    private bool isSwiping = false;  // Cờ kiểm tra vuốt màn hình
    private List<OrangePiece> orangePieces = new List<OrangePiece>();  // Danh sách mảnh dưa hấu

    void Start()
    {
        // Lấy tham chiếu đến GridManager
        gridManager = FindObjectOfType<GridManager>();

        // Khởi tạo grid
        gridManager.CreateGrid();

        // Sinh các mảnh dưa hấu và chướng ngại vật trong GridManager
        CreateWatermelonsAndObstacles();
    }

    void Update()
    {
        // Kiểm tra thao tác vuốt trên màn hình
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition; // Ghi nhận vị trí bắt đầu vuốt
            isSwiping = true;
        }
        else if (Input.GetMouseButtonUp(0) && isSwiping)
        {
            Vector2 touchEndPos = Input.mousePosition; // Vị trí kết thúc vuốt
            isSwiping = false;

            Vector2 swipeDelta = touchEndPos - touchStartPos; // Tính toán độ lệch vuốt
            HandleSwipe(swipeDelta); // Xử lý vuốt
        }
       
    }

    void HandleSwipe(Vector2 swipeDelta)
    {
        float absX = Mathf.Abs(swipeDelta.x); // Độ lệch tuyệt đối theo trục X
        float absY = Mathf.Abs(swipeDelta.y); // Độ lệch tuyệt đối theo trục Y

        // Kiểm tra vuốt ngang hay dọc
        if (absX > absY)
        {
            // Vuốt ngang (trái/phải)
            if (swipeDelta.x > 0)
            {
                // Vuốt sang phải
                MoveWatermelons(Vector2Int.up);
            }
            else
            {
                // Vuốt sang trái
                MoveWatermelons(Vector2Int.down);
            }
        }
        else
        {
            // Vuốt dọc (lên/xuống)
            if (swipeDelta.y > 0)
            {
                // Vuốt lên
                MoveWatermelons(Vector2Int.left);
            }
            else
            {
                // Vuốt xuống
                MoveWatermelons(Vector2Int.right);
            }
        }
    }

    void MoveWatermelons(Vector2Int direction)
    {
        foreach (OrangePiece piece in orangePieces)
        {
            Vector2Int nextPos = piece.gridPosition + direction;

            if (IsCellValid(nextPos))
            {
                piece.gridPosition = nextPos; // Cập nhật tọa độ logic
                piece.gameObject.transform.position = gridManager.GetCellWorldPosition(nextPos.x, nextPos.y, transform.position); // Cập nhật vị trí trên màn hình
            }
        }
        if (CheckWin())
        {
            Debug.Log("You Win!");
            // Xử lý khi người chơi thắng, ví dụ như chuyển sang màn hình chiến thắng hoặc tạo hiệu ứng
        }
    }

    bool IsCellValid(Vector2Int cellPos)
    {
        // Kiểm tra xem vị trí có hợp lệ hay không (nằm trong phạm vi grid)
        if (cellPos.x < 0 || cellPos.y < 0 || cellPos.x >= levelData.columns || cellPos.y >= levelData.rows)
            return false;

        Vector3 worldPos = gridManager.GetCellWorldPosition(cellPos.x, cellPos.y, transform.position);

        // Kiểm tra xem có vật cản nào ở vị trí đó không (chướng ngại vật)
        Collider2D block = Physics2D.OverlapCircle(worldPos, 0.1f, LayerMask.GetMask("Block")); // Kiểm tra các đối tượng có layer "Block"
        if (block != null)
        {
            Debug.Log("Blocked by obstacle at " + cellPos);
            return false;
        }

        // Kiểm tra xem có mảnh dưa nào khác ở vị trí đó không
        foreach (var piece in orangePieces)
        {
            if (piece.gridPosition == cellPos)
            {
                Debug.Log("Occupied by another watermelon at " + cellPos);
                return false;
            }
        }

        return true;
    }


    void CreateWatermelonsAndObstacles()
    {
        int idCounter = 1;  // Biến đếm bắt đầu từ 1

        // Tạo các mảnh dưa hấu
        for (int i = 0; i < levelData.watermelonPositions.Length; i++)
        {
            Vector2Int position = levelData.watermelonPositions[i];
            Vector3 worldPos = gridManager.GetCellWorldPosition(position.x, position.y, transform.position);

            GameObject watermelon = Instantiate(watermelonPrefab, worldPos, Quaternion.identity);
            watermelon.transform.SetParent(gridManager.transform);
            watermelon.layer = LayerMask.NameToLayer("Orange");

            // Gán sprite nếu có
            Image img = watermelon.GetComponent<Image>();
            if (img != null && i < levelData.watermelonSprites.Length)
                img.sprite = levelData.watermelonSprites[i];

            // Gán ID cho mảnh dưa hấu
            int id = idCounter++;  // Tăng ID sau khi gán

            // Lưu trữ thông tin mảnh dưa
            orangePieces.Add(new OrangePiece(watermelon, position, id));  // Thêm ID vào khi tạo OrangePiece
            Debug.Log($"Mảnh dưa hấu ID: {id} - Vị trí: {position}");
        }

        // Tạo các chướng ngại vật
        foreach (var obstaclePosition in levelData.obstaclePositions)
        {
            // Tạo chướng ngại vật tại vị trí cụ thể trong grid
            GameObject obstacle = Instantiate(obstaclePrefab, gridManager.GetCellWorldPosition(obstaclePosition.x, obstaclePosition.y, transform.position), Quaternion.identity);
            obstacle.transform.SetParent(gridManager.transform);  // Đặt parent cho chướng ngại vật để quản lý tốt hơn
        }
    }
    // Kiểm tra các mảnh dưa hấu đã hợp thành quả cam hoàn chỉnh
    bool CheckWin()
    {
        // Lấy các mảnh dưa hấu theo ID
        OrangePiece piece1 = GetOrangePieceById(1);
        OrangePiece piece2 = GetOrangePieceById(2);
        OrangePiece piece3 = GetOrangePieceById(3);
        OrangePiece piece4 = GetOrangePieceById(4);

        // Kiểm tra nếu không tìm thấy mảnh nào
        if (piece1 == null || piece2 == null || piece3 == null || piece4 == null)
            return false;

        Debug.Log($"P1: {piece1.gridPosition}, P2: {piece2.gridPosition}, P3: {piece3.gridPosition}, P4: {piece4.gridPosition}");

        // Kiểm tra các mảnh có tạo thành hình vuông theo thứ tự 34 và 12
        bool isSquareInOrder = IsSquareFormation(piece1, piece2, piece3, piece4);

        if (isSquareInOrder)
        {
            Debug.Log("You Win!");
            return true;
        }

        return false;
    }

    // Kiểm tra các mảnh có tạo thành hình vuông theo thứ tự 34 và 12
    bool IsSquareFormation(OrangePiece piece1, OrangePiece piece2, OrangePiece piece3, OrangePiece piece4)
    {
        // Lấy vị trí của các mảnh
        Vector2Int p1Pos = piece1.gridPosition;
        Vector2Int p2Pos = piece2.gridPosition;
        Vector2Int p3Pos = piece3.gridPosition;
        Vector2Int p4Pos = piece4.gridPosition;

        // Kiểm tra 2 mảnh P3 và P4 phải cùng một hàng
        bool rowMatch34 = p3Pos.x == p4Pos.x;

        // Kiểm tra 2 mảnh P1 và P2 phải cùng một hàng
        bool rowMatch12 = p1Pos.x == p2Pos.x;

        // Kiểm tra mảnh P3 và P1 phải cùng một cột
        bool columnMatch31 = p3Pos.y == p1Pos.y;

        // Kiểm tra mảnh P4 và P2 phải cùng một cột
        bool columnMatch42 = p4Pos.y == p2Pos.y;

        // Kiểm tra các mảnh phải ở đúng thứ tự 34 và 12
        bool correctOrder = p3Pos.x < p1Pos.x && p4Pos.x < p2Pos.x;

        Debug.Log($"RowMatch34: {rowMatch34}, RowMatch12: {rowMatch12}, ColumnMatch31: {columnMatch31}, ColumnMatch42: {columnMatch42}, CorrectOrder: {correctOrder}");

        // Kiểm tra tất cả các điều kiện để tạo thành hình vuông theo thứ tự 34, 12
        if (rowMatch34 && rowMatch12 && columnMatch31 && columnMatch42 && correctOrder)
        {
            return true;
        }

        return false;
    }




    // Hàm lấy mảnh dưa hấu theo ID
    OrangePiece GetOrangePieceById(int id)
    {
        return orangePieces.Find(piece => piece.id == id);
    }



}

