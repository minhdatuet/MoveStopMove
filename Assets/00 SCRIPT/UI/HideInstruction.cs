using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideInstruction : MonoBehaviour
{
    private bool isHidden = false; // Biến kiểm tra xem object đã bị ẩn chưa

    void Update()
    {
        // Kiểm tra lần chạm đầu tiên trên màn hình điện thoại
        if (Input.touchCount > 0 && !isHidden)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                HideObject();
            }
        }

        // Kiểm tra lần click chuột đầu tiên
        if (Input.GetMouseButtonDown(0) && !isHidden)
        {
            HideObject();
        }
    }

    void HideObject()
    {
        gameObject.SetActive(false); // Ẩn object
        isHidden = true; // Đánh dấu object đã bị ẩn
    }
}
