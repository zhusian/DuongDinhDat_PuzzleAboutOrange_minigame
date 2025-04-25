using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHolder : MonoBehaviour
{
    public static LevelHolder Instance { get; private set; }

    public Level selectedLevel;

    void Awake()
    {
        // Đảm bảo chỉ có 1 LevelHolder tồn tại
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ lại khi chuyển scene
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
