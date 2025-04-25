using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Game/Level", order = 1)]
public class Level : ScriptableObject
{
    public string levelName;
    public int levelNumber;
    public int rows = 4;
    public int columns = 4;
    public float timeLimit = 40f;  

    public Vector2Int[] orangePositions = new Vector2Int[4];  
    public Sprite[] orangeSprites = new Sprite[4];   

    public Vector2Int[] obstaclePositions;  
    public Sprite obstacleSprite; 
}
