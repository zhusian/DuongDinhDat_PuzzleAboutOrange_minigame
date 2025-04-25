using UnityEngine;
using UnityEngine.UI;  // Thêm namespace để làm việc với Image

public class LevelManager : MonoBehaviour
{
    public Level levelData;  // Tham chiếu tới đối tượng Level
    public GameObject watermelonPrefab;  // Prefab cho mảnh dưa hấu
    public GameObject obstaclePrefab;    // Prefab cho chướng ngại vật
    private GridManager gridManager;  // Tham chiếu tới GridManager

    void Start()
    {
        // Lấy tham chiếu đến GridManager
        gridManager = FindObjectOfType<GridManager>();

        // Khởi tạo grid
        gridManager.CreateGrid();

        // Sinh các mảnh dưa hấu và chướng ngại vật trong GridManager
        CreateWatermelonsAndObstacles();
    }

    // Tạo các mảnh dưa hấu và chướng ngại vật từ prefab
    void CreateWatermelonsAndObstacles()
    {
        // Tạo các mảnh dưa hấu
        for (int i = 0; i < levelData.watermelonPositions.Length; i++)
        {
            // Lấy vị trí từ levelData
            Vector2Int position = levelData.watermelonPositions[i];

            // Tạo mảnh dưa hấu tại vị trí cụ thể trong grid
            GameObject watermelon = Instantiate(watermelonPrefab, gridManager.GetCellWorldPosition(position.x, position.y, transform.position), Quaternion.identity);
            watermelon.transform.SetParent(gridManager.transform);  // Đặt parent cho mảnh dưa hấu để quản lý tốt hơn

            // Kiểm tra và gán hình ảnh cho mảnh dưa hấu từ mảng watermelonSprites
            Image watermelonImage = watermelon.GetComponent<Image>();
            if (watermelonImage != null)
            {
                if (levelData.watermelonSprites.Length > i)
                {
                    watermelonImage.sprite = levelData.watermelonSprites[i];  // Gán sprite cho mảnh dưa hấu
                    Debug.Log($"Gán sprite {i} cho mảnh dưa hấu.");
                }
                else
                {
                    Debug.LogError($"Sprite tại chỉ số {i} không có trong mảng watermelonSprites!");
                }
            }
            else
            {
                Debug.LogError("Không tìm thấy Image trong prefab watermelon!");
            }
        }

        // Tạo các chướng ngại vật
        foreach (var obstaclePosition in levelData.obstaclePositions)
        {
            // Tạo chướng ngại vật tại vị trí cụ thể trong grid
            GameObject obstacle = Instantiate(obstaclePrefab, gridManager.GetCellWorldPosition(obstaclePosition.x, obstaclePosition.y, transform.position), Quaternion.identity);
            obstacle.transform.SetParent(gridManager.transform);  // Đặt parent cho chướng ngại vật để quản lý tốt hơn
        }
    }
}
