﻿using UnityEngine;
using UnityEngine.UI;

public class LevelDisplay : MonoBehaviour
{
    public Transform target; // Đối tượng nhân vật
    public Vector3 offset; // Khoảng cách phía trên đầu

    private Text levelText;

    void Start()
    {
        // Tìm TextMeshPro bên trong đối tượng
        levelText = GetComponentInChildren<Text>();
    }

    void Update()
    {
        // Cập nhật vị trí của TextMeshPro để nó di chuyển theo nhân vật
        if (target != null)
        {
            transform.position = target.position + offset + new Vector3(0, 0.2f, 0);
        } else
        {
            Destroy(gameObject);
        }
    }

    public void SetLevel(int level)
    {
        if (levelText) levelText.text = level.ToString();
    }
}
