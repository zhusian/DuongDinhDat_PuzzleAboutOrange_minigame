//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class GameMenu : MonoBehaviour
//{
//    public LevelData level; // Dữ liệu của level (thông qua LevelData script gán từ Inspector)
//    public GridManager gridManager; // Gán GridManager vào đây từ Inspector

//    // Hàm để load level vào grid
//    [ContextMenu("Load Level")]
//    public void LoadLevelIntoGrid()
//    {
//        if (level == null)
//        {
//            Debug.LogError("⚠️ Chưa gán LevelData!");
//            return;
//        }

//        if (gridManager == null)
//        {
//            Debug.LogError("⚠️ Chưa gán GridManager!");
//            return;
//        }

//        // Tạo lưới grid
//        gridManager.CreateGrid();

//        // Tiến hành sinh mảnh dưa hấu
//        foreach (var piece in level.watermelonPieces)
//        {
//            Vector3 pos = gridManager.GridToWorldPos(piece.gridPosition);
//            GameObject obj = Instantiate(piece.watermelonPrefab.gameObject, pos, Quaternion.identity, gridManager.gridHolder);
//            obj.name = $"Watermelon_{piece.gridPosition.x}_{piece.gridPosition.y}";
//        }

//        // Tiến hành sinh chướng ngại vật
//        foreach (var obstacle in level.obstaclePieces)
//        {
//            Vector3 pos = gridManager.GridToWorldPos(obstacle.gridPosition);
//            GameObject obj = Instantiate(obstacle.obstaclePrefab.gameObject, pos, Quaternion.identity, gridManager.gridHolder);
//            obj.name = $"Obstacle_{obstacle.gridPosition.x}_{obstacle.gridPosition.y}";
//        }

//        Debug.Log($"✅ Đã load level '{level.levelName}' vào grid.");
//    }

//    private void Start()
//    {
//        LoadLevelIntoGrid(); // Tự động load level khi game bắt đầu (có thể tắt nếu không muốn)
//    }
//}
