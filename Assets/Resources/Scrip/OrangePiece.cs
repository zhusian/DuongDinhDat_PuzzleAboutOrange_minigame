using UnityEngine;

public class OrangePiece
{
    public GameObject gameObject;
    public Vector2Int gridPosition;
    public int id;
    public OrangePiece(GameObject obj, Vector2Int pos, int id)
    {
        gameObject = obj;
        gridPosition = pos;
        this.id = id;
    }
}
