using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject cellPrefab;  
    public int rows = 4;  
    public int columns = 4; 
    public float cellWidth = 200f;  
    public float cellHeight = 200f;  
    public float cellSpacing = 0f; 

    private GameObject[,] cells;  

   
    public void CreateGrid()
    {
        cells = new GameObject[rows, columns];  

        Vector3 gridCenter = transform.position;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 position = GetCellWorldPosition(row, col, gridCenter);

                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity);
                cell.transform.SetParent(transform);  
                cell.name = $"Cell_{row}_{col}"; 
                cells[row, col] = cell;
            }
        }
    }

    // Tính toán vị trí của cell trong không gian thế giới với khoảng cách giữa các cell
    public Vector3 GetCellWorldPosition(int row, int col, Vector3 gridCenter)
    {
        float xPos = col * (cellWidth + cellSpacing) - (columns * (cellWidth + cellSpacing)) / 2 + (cellWidth / 2);
        float yPos = -(row * (cellHeight + cellSpacing) - (rows * (cellHeight + cellSpacing)) / 2 + (cellHeight / 2));

        return gridCenter + new Vector3(xPos, yPos, 0f);
    }

    public Vector2Int GetCellPosition(Vector3 worldPosition)
    {
        // Tính toán vị trí của cell dựa trên chỉ số hàng và cột
        int x = Mathf.FloorToInt((worldPosition.x - transform.position.x + (columns * (cellWidth + cellSpacing)) / 2) / (cellWidth + cellSpacing));
        int y = Mathf.FloorToInt((-(worldPosition.y - transform.position.y) + (rows * (cellHeight + cellSpacing)) / 2) / (cellHeight + cellSpacing));

        x = Mathf.Clamp(x, 0, columns - 1);
        y = Mathf.Clamp(y, 0, rows - 1);

        return new Vector2Int(x, y);
    }

    public void ClearGrid()
    {
        foreach (GameObject cell in cells)
        {
            if (cell != null)
            {
                Destroy(cell);
            }
        }
    }
}
