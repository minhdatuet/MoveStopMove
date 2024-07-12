using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInHandController : MonoBehaviour
{
    // Danh sách các vũ khí
    public List<GameObject> weapons = new List<GameObject>();

    // Vũ khí hiện tại
    private GameObject currentWeapon;

    // Tham chiếu đến UI container chứa các nút vũ khí
    public GameObject weaponButtonContainer;

    void Start()
    {
        SetupWeaponButtons();
    }

    // Update được gọi mỗi frame
    void Update()
    {
        // Xử lý cập nhật vũ khí nếu cần
    }

    // Thiết lập các nút vũ khí
    void SetupWeaponButtons()
    {
        foreach (Transform child in weaponButtonContainer.transform)
        {
            Button button = child.GetComponent<Button>();
            if (button != null)
            {
                int index = child.GetSiblingIndex();
                button.onClick.AddListener(() => SelectWeapon(index));
            }
        }
    }

    // Chọn vũ khí theo chỉ số
    void SelectWeapon(int index)
    {
        if (index >= 0 && index < weapons.Count)
        {
            if (currentWeapon != null)
            {
                currentWeapon.SetActive(false); // Tắt vũ khí hiện tại
            }

            currentWeapon = weapons[index];
            currentWeapon.SetActive(true); // Bật vũ khí được chọn
        }
    }
}
