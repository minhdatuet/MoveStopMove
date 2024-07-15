﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameDisplay : MonoBehaviour
{
    public Transform target; // Đối tượng nhân vật
    public Vector3 offset; // Khoảng cách phía trên đầu

    private Text nameText;

    void Start()
    {
        // Tìm Text bên trong đối tượng
        nameText = GetComponent<Text>();
    }

    void Update()
    {
        // Cập nhật vị trí của Text để di chuyển theo nhân vật
        if (target.gameObject.activeInHierarchy)
        {
            transform.position = target.position + offset + new Vector3(0, 0.4f, 0);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SetName(string name)
    {
        nameText = GetComponent<Text>();
        nameText.text = name;
    }

    public string GetName()
    {
        return nameText.text;
    }
}
