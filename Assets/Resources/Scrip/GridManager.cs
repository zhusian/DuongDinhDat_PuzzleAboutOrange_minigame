using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject cellPrefab;  // Prefab của cell
    public int rows = 4;  // Số hàng trong grid
    public int columns = 4;  // Số cột trong grid
    public float cellWidth = 200f;  // Kích thước cell theo chiều rộng
    public float cellHeight = 200f;  // Kích thước cell theo chiều cao
    public float cellSpacing = 0f;  // Khoảng cách giữa các cell

    private GameObject[,] cells;  // Mảng lưu trữ các cell

    //void Start()
    //{
    //    // Khởi tạo grid
    //    CreateGrid();
    //}

    // Tạo grid với các cell
    public void CreateGrid()
    {
        cells = new GameObject[rows, columns];  // Khởi tạo mảng cells với kích thước rows x columns

        // Tính toán vị trí gốc của grid (trung tâm của GridHolder)
        Vector3 gridCenter = transform.position;

        // Tính toán vị trí các cell sao cho chúng được căn chỉnh với trung tâm
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Tính toán vị trí của cell trong không gian thế giới
                Vector3 position = GetCellWorldPosition(row, col, gridCenter);

                // Sinh ra một cell mới tại vị trí tính toán
                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity);
                cell.transform.SetParent(transform);  // Đặt parent là GridManager để quản lý tốt hơn
                cell.name = $"Cell_{row}_{col}";  // Đặt tên cho cell

                // Lưu cell vào mảng để có thể quản lý sau này
                cells[row, col] = cell;
            }
        }
    }

    // Tính toán vị trí của cell trong không gian thế giới với khoảng cách giữa các cell
    public Vector3 GetCellWorldPosition(int row, int col, Vector3 gridCenter)
    {
        // Tính toán vị trí của cell dựa trên chỉ số hàng và cột, cộng thêm khoảng cách giữa các cell
        float xPos = col * (cellWidth + cellSpacing) - (columns * (cellWidth + cellSpacing)) / 2 + (cellWidth / 2);
        float yPos = -(row * (cellHeight + cellSpacing) - (rows * (cellHeight + cellSpacing)) / 2 + (cellHeight / 2));

        // Trả về vị trí tính toán cho cell với vị trí gốc là gridCenter
        return gridCenter + new Vector3(xPos, yPos, 0f);
    }

    // Chuyển đổi vị trí thế giới (world position) thành vị trí cell trong grid
    public Vector2Int GetCellPosition(Vector3 worldPosition)
    {
        // Tính toán vị trí của cell dựa trên chỉ số hàng và cột
        int x = Mathf.FloorToInt((worldPosition.x - transform.position.x + (columns * (cellWidth + cellSpacing)) / 2) / (cellWidth + cellSpacing));
        int y = Mathf.FloorToInt((-(worldPosition.y - transform.position.y) + (rows * (cellHeight + cellSpacing)) / 2) / (cellHeight + cellSpacing));

        // Đảm bảo rằng vị trí cell nằm trong phạm vi grid
        x = Mathf.Clamp(x, 0, columns - 1);
        y = Mathf.Clamp(y, 0, rows - 1);

        return new Vector2Int(x, y);
    }

    // Gỡ bỏ tất cả các cell trong grid
    public void ClearGrid()
    {
        // Duyệt qua tất cả các cell và hủy chúng
        foreach (GameObject cell in cells)
        {
            if (cell != null)
            {
                Destroy(cell);
            }
        }
    }
}
