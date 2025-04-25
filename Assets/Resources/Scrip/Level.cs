using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Game/Level", order = 1)]
public class Level : ScriptableObject
{
    public string levelName;  // Tên màn chơi
    public int rows = 4;  // Số hàng trong grid
    public int columns = 4;  // Số cột trong grid
    public float timeLimit = 40f;  // Thời gian hoàn thành màn chơi

    // Các mảnh dưa hấu (4 vị trí riêng biệt)
    public Vector2Int[] watermelonPositions = new Vector2Int[4];  // Vị trí các mảnh dưa hấu
    public Sprite[] watermelonSprites = new Sprite[4];   // Prefabs cho 4 mảnh dưa hấu

    // Các chướng ngại vật
    public Vector2Int[] obstaclePositions;  // Vị trí các chướng ngại vật
    public Sprite obstacleSprite;  // Hình ảnh cho chướng ngại vật
}
